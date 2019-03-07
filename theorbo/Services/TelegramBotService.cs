using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using theorbo.Config;
using theorbo.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace theorbo.Services
{
    public class TelegramBotService
    {
        private static readonly ILog logger = Log.Get(typeof(TelegramBotService));
        private readonly ITelegramCommandHandler[] _commandHandlers;

        private readonly BotConfiguration _configuration;
        private readonly SemaphoreSlim _messageSemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly ConcurrentBag<DateTime> _messageSendTimes = new ConcurrentBag<DateTime>();

        private TelegramBotClient _bot;
        private User _telegramBotUser;

        public TelegramBotService(BotConfiguration configuration, ITelegramCommandHandler[] handlers)
        {
            _configuration = configuration;
            _commandHandlers = handlers;
        }

        public async void Start()
        {
            logger.Info($"Starting telegram bot");

            try
            {
                _bot = new TelegramBotClient(_configuration.TelegramAnnouncerBotKey.Unprotect());

                _telegramBotUser = await _bot.GetMeAsync();

                _bot.OnMessage += BotOnMessageReceived;
                _bot.OnMessageEdited += BotOnMessageReceived;
                _bot.OnCallbackQuery += BotOnCallbackQueryReceived;
                _bot.OnInlineQuery += BotOnInlineQueryReceived;
                _bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
                _bot.OnReceiveError += BotOnReceiveError;

                _bot.StartReceiving();
            }
            catch (Exception e)
            {
                logger.Error($"Telegram bot initialization error", e);
                logger.Fatal("Terminating service");

                Environment.Exit(1);
            }
        }


        private async Task RunChatCommandAsync(ITelegramCommandHandler matchedCommand, Message message,
            string arguments)
        {
            var chatId = message.Chat.Id;

            try
            {
                await matchedCommand.ProcessCommand(message, arguments, this);
            }
            catch (TimeoutException)
            {
                logger.Error($"{matchedCommand.Command} execution resulted in timeout");
                await SendTextMessageAsync(chatId, ":(", replyMarkup: new ReplyKeyboardRemove());
            }
            catch (TaskCanceledException)
            {
                logger.Error($"{matchedCommand.Command} execution resulted in TaskCanceledException");
                await SendTextMessageAsync(chatId, ":(", replyMarkup: new ReplyKeyboardRemove());
            }
            catch (Exception e)
            {
                logger.Error($"{matchedCommand.Command} execution resulted in exception", e);
            }
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text || string.IsNullOrWhiteSpace(message.Text))
                return;

            var tokens = message.Text.Split(' ').ToArray();

            var command = tokens.First().TrimStart('/');

            var circumflexIdx = command.IndexOf('@');

            if (circumflexIdx != -1)
                command = command.Remove(circumflexIdx);

            var matchedCommand = _commandHandlers.FirstOrDefault(mc => mc.Command == command);

            if (matchedCommand != null)
            {
                var arguments = tokens.Length > 1 ? string.Join(" ", tokens.Skip(1)) : string.Empty;

                await RunChatCommandAsync(matchedCommand, message, arguments).ConfigureAwait(false);
                return;
            }

            await AnnounceUsage(message).ConfigureAwait(false);
        }

        private async Task AnnounceUsage(Message source)
        {
            if (!_commandHandlers.Any())
            {
                await SendTextMessageAsync(source.Chat.Id, Resources.BotNoCommandsAvailable,
                    replyToMessageId: source.MessageId);
                return;
            }

            var builder = new StringBuilder();

            builder.AppendLine(Resources.BotUsageHeader);

            foreach (var commandHandler in _commandHandlers)
                builder.AppendLine($"/{commandHandler.Command} — {commandHandler.Description}");

            await SendTextMessageAsync(source.Chat.Id,
                    builder.ToString(),
                    ParseMode.Markdown,
                    replyToMessageId: source.MessageId,
                    replyMarkup: new ReplyKeyboardRemove())
                .ConfigureAwait(false);
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await _bot.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            logger.Info($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results =
            {
                new InlineQueryResultLocation(
                        "1",
                        40.7058316f,
                        -74.2581888f,
                        "New York") // displayed result
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            40.7058316f,
                            -74.2581888f) // message if result is selected
                    },

                new InlineQueryResultLocation(
                        "2",
                        13.1449577f,
                        52.507629f,
                        "Berlin") // displayed result
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            13.1449577f,
                            52.507629f) // message if result is selected
                    }
            };

            await _bot.AnswerInlineQueryAsync(
                inlineQueryEventArgs.InlineQuery.Id,
                results,
                isPersonal: true,
                cacheTime: 0);
        }

        public Task SendChatActionAsync(long chatChatId, ChatAction chatAction,
            CancellationToken token = default(CancellationToken))
        {
            return _bot.SendChatActionAsync(chatChatId, chatAction, token);
        }

        public async Task<Message> SendTextMessageAsync(ChatId chatId, string text,
            ParseMode parseMode = ParseMode.Default, bool disableWebPagePreview = false,
            bool disableNotification = false, int replyToMessageId = 0, IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            const int guardExtraDelayMs = 500;

            await _messageSemaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var lastSecondCount = 0;
                var timeWindowStart = DateTime.Now.AddSeconds(-1);
                var lastMessageTimesOrdered =
                    _messageSendTimes.Where(time => time >= timeWindowStart).OrderBy(t => t).ToArray();

                lastSecondCount = lastMessageTimesOrdered.Length;

                if (lastSecondCount >= _configuration.TelegramMaxMessagesPerSecond)
                {
                    var skipQty = lastSecondCount - _configuration.TelegramMaxMessagesPerSecond;

                    var mostOffendingMessage = lastMessageTimesOrdered.Skip(skipQty).FirstOrDefault();

                    //How much to wait till most offending message leaves time window
                    var clearance = (mostOffendingMessage - timeWindowStart).TotalMilliseconds +
                                    guardExtraDelayMs;

                    await Task.Delay((int) clearance, cancellationToken).ConfigureAwait(false);
                }

                _messageSendTimes.Add(DateTime.Now);

                return await _bot.SendTextMessageAsync(chatId, text, parseMode, disableWebPagePreview,
                    disableNotification,
                    replyToMessageId, replyMarkup, cancellationToken);
            }
            catch (Exception e)
            {
                logger.Error($"Telegram bot error", e);

                return null;
            }
            finally
            {
                _messageSemaphoreSlim.Release();

                if (_messageSendTimes.Count > _configuration.TelegramMaxMessagesPerSecond)
                    _messageSendTimes.TryTake(out var _);
            }
        }

        private static void BotOnChosenInlineResultReceived(object sender,
            ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            logger.Info($"Received inline result: " +
                        $"{chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            logger.Warn($"Received error: {receiveErrorEventArgs.ApiRequestException.ErrorCode} " +
                        $"— {receiveErrorEventArgs.ApiRequestException.Message}");
        }

        public void Stop()
        {
            logger.Info("Bot shutdown");
            _bot.StopReceiving();
        }
    }
}