using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Schedule.Bot.Services.Interfaces
{
    public interface ITelegramService
    {
        Task ReceivedMessage(Update update, CancellationToken cancellationToken);
    }
}