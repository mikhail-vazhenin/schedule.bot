using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Bot.Services.Interfaces
{
    public interface IMapService
    {
        Task<string> GetDurationToMetro();

        Task<string> GetDurationToWork();
    }
}
