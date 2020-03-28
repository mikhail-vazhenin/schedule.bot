using System;

namespace Schedule.Bot.Services.Interfaces
{
    public interface ITimeStorage
    {
        TimeSpan[] GetNearestToMetro(DateTime currentDateTime);
        TimeSpan[] GetNearestToWork(DateTime currentDateTime);
    }
}