using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Bot.Services.Builder
{
    public class MessageBuilder
    {
        public const string TooLate = "Сегодня уже только пешком";
        public const string TodayIsHoliday = "Сегодня у нас выходной.\nРады будем видеть Вас в понедельник";

        StringBuilder _builder = new StringBuilder();

        public MessageBuilder AddRoute(string route)
        {
            _builder.AppendLine($"{route} едем в");
            return this;
        }

        public MessageBuilder AddTimes(TimeSpan[] times, string duration = null)
        {
            for (var i = 0; i < times.Length; i++)
            {
                var timeline = $"  • {times[i].ToString(@"hh\:mm")}";

                if (i == 0 && !string.IsNullOrEmpty(duration))
                    timeline += $" (≈{duration} в пути)";

                _builder.AppendLine(timeline);
            }
            _builder.AppendLine();
            return this;
        }

        public MessageBuilder AddShortDay(bool shortDay)
        {
            if (shortDay) _builder.AppendLine("*Сегодня сокращенный рабочий день");
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

    }
}
