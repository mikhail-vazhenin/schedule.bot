using System;
using System.Threading.Tasks;

namespace Schedule.Bot.Services.Interfaces
{
    public interface IMessageService
    {
        Task<string> GetNearestTimeMessage(string clientId, string message, DateTime? date = null);
    }
}