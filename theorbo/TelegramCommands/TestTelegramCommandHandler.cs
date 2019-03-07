using System.Threading.Tasks;
using theorbo.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace theorbo.TelegramCommands
{
    public class TestTelegramCommandHandler : ITelegramCommandHandler
    {
        public string Description { get; } = "Получить мудрость Способина";
        public string Command { get; } = "sposobine";

        public Task ProcessCommand(Message message, string arguments, TelegramBotService telegramBotService)
        {
            return telegramBotService.SendTextMessageAsync(message.Chat.Id,
                "Ощущение, по определению В. И. Ленина, «есть превращение энрегии внешнего раздражения в факт сознания». " +
                "Таким образом, источник звука, звуковые волны и работа слухового аппарата существуют объективно.",
                replyToMessageId: message.MessageId,
                parseMode: ParseMode.Markdown);
        }
    }
}