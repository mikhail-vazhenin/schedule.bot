using System;
using System.Collections.Generic;

namespace Schedule.Bot.Services.Interfaces
{
    public interface IHolidayStorage
    {
        bool IsHoliday(DateTime date);

        bool IsShortDay(DateTime date);
    }
}