using Schedule.Bot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Bot.Services
{
    public class HolidayStorage : IHolidayStorage
    {
        private readonly IDictionary<int, DayInfo[][]> _holidayTable = new Dictionary<int, DayInfo[][]>();
        public HolidayStorage(string fileName)
        {

            using (StreamReader file = File.OpenText(fileName))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var parts = line.Split(";");
                    var year = int.Parse(parts[0]);
                    var months = parts.Skip(1)
                        .Select(m => m.Split(","))
                        .Select(m => m.Select(d =>
                        {
                            if (d.Contains("*")) return new DayInfo { Day = int.Parse(d.Trim('*')), Type = DayType.Short };
                            else return new DayInfo { Day = int.Parse(d.Trim('*')), Type = DayType.Holiday };

                        }).ToArray()).ToArray();

                    _holidayTable.Add(year, months);
                }
            }
        }

        public bool IsHoliday(DateTime date)
        {
            return GetHolidays(date.Year, date.Month).Contains(date.Day);
        }

        public bool IsShortDay(DateTime date)
        {
            return GetShortDays(date.Year, date.Month).Contains(date.Day);
        }

        private IEnumerable<int> GetShortDays(int year, int month)
        {
            return _holidayTable[year][month - 1].Where(d => d.Type == DayType.Short).Select(d => d.Day);
        }
        private IEnumerable<int> GetHolidays(int year, int month)
        {
            return _holidayTable[year][month - 1].Where(d => d.Type == DayType.Holiday).Select(d => d.Day);
        }

        public class DayInfo
        {
            public int Day { get; set; }

            public DayType Type { get; set; }
        }

        public enum DayType
        {
            Short,
            Holiday
        }
    }
}
