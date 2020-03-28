using Newtonsoft.Json;
using Schedule.Bot.Services.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Schedule.Bot.Services
{
    public class JsonTimeStorage : ITimeStorage
    {
        private readonly TimeTable _timeTable;

        public JsonTimeStorage(string fileName)
        {
            _timeTable = JsonConvert.DeserializeObject<TimeTable>(File.ReadAllText(fileName));
        }

        public TimeSpan[] GetNearestToMetro(DateTime currentDateTime)
        {
            return GetNearestTimeFromSource(_timeTable.ToMetro, currentDateTime);
        }

        public TimeSpan[] GetNearestToWork(DateTime currentDateTime)
        {
            return GetNearestTimeFromSource(_timeTable.ToWork, currentDateTime);
        }

        private TimeSpan[] GetNearestTimeFromSource(TimeItem[] collection, DateTime datetime)
        {
            var time = datetime.TimeOfDay;
            var dayOfWeek = DayToInt(datetime.DayOfWeek);

            if (!collection.Any()) throw new Exception("Хранилище расписания движения не найдено");
            return collection
                .Where(t => t.Days.Contains("*") || t.Days.Contains(dayOfWeek.ToString()))
                .SkipWhile(t => t.Time < time)
                .Take(3)
                .Select(t => t.Time)
                .ToArray();
        }

        private static int DayToInt(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday) return 7;
            else return (int)dayOfWeek;
        }

        class TimeTable
        {
            public TimeItem[] ToMetro { get; set; }
            public TimeItem[] ToWork { get; set; }
        }

        class TimeItem
        {
            public TimeSpan Time { get; set; }
            public string Days { get; set; }
        }

    }
}
