using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace theorbo.Services
{
    public interface ITelegramCommandHandler
    {
        string Description { get; }
        string Command { get; }

        Task ProcessCommand(Message chatMessageSignalSink, string arguments, TelegramBotService telegramBotService);
    }
}