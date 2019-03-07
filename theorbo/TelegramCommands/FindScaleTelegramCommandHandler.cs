using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using theorbo.Logging;
using theorbo.MusicTheory;
using theorbo.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace theorbo.TelegramCommands
{
    public class FindScaleTelegramCommandHandler : ITelegramCommandHandler
    {
        private static readonly ILog logger = Log.Get(typeof(FindScaleTelegramCommandHandler));

        private readonly IanringSource _scales;

        public FindScaleTelegramCommandHandler(IanringSource scales)
        {
            _scales = scales;
        }

        public string Description { get; } = "Определить лад";
        public string Command { get; } = "scale";

        public Task ProcessCommand(Message message, string arguments, TelegramBotService telegramBotService)
        {
            if (string.IsNullOrWhiteSpace(arguments))
                return telegramBotService.SendTextMessageAsync(message.Chat.Id, Resources.ScaleCommand_Hints,
                    replyToMessageId: message.MessageId,
                    parseMode: ParseMode.Markdown);

            var key = arguments.Replace(" ", string.Empty).Replace("w", "2").Replace("h", "1");

            var scales = _scales.IanringScalesByStepsCyclic.Where(k => k.Key.Contains(key)).ToArray();

            if (scales.Length == 0)
                return telegramBotService.SendTextMessageAsync(message.Chat.Id, Resources.NoScalesFound,
                    replyToMessageId: message.MessageId,
                    parseMode: ParseMode.Markdown);

            var builder = new StringBuilder();

            var topScales = scales.OrderByDescending(s => s.Value.Names.Sum(n => n.Length)).Take(5);

            foreach (var scale in topScales)
            {
                var name = string.Join(", ", scale.Value.Names);

                if (string.IsNullOrWhiteSpace(name))
                    name = Resources.UntitledScale;

                builder.AppendLine($"{scale.Key} : *{name}*");
            }

            if (scales.Length > 5)
                builder.AppendLine("...");

            return telegramBotService.SendTextMessageAsync(message.Chat.Id, builder.ToString(),
                replyToMessageId: message.MessageId,
                parseMode: ParseMode.Markdown);
        }
    }
}