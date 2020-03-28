using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Schedule.Bot.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserName(this User user)
        {
            return $"{user.FirstName} {user.LastName}";
        }
    }
}
