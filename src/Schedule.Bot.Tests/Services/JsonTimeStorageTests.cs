using NUnit.Framework;
using Schedule.Bot.Services;
using System;

namespace Schedule.Bot.Tests
{
    [TestFixture]
    public class JsonTimeStorageTests
    {
        private JsonTimeStorage _target = new JsonTimeStorage("Schedule.json");

        [TestCase(10, "17:10:00", "17:20:00", "17:30:00")]
        [TestCase(14, "17:10:00", "17:15:00", "17:20:00")]
        public void JsonTimeStorage_GetScheduleToMetro(int day, string expectedTime1, string expectedTime2, string expectedTime3)
        {
            var actualTimes = _target.GetNearestToMetro(new DateTime(2018, 12, day, 17, 7, 0));

            Assert.IsNotEmpty(actualTimes);
            Assert.AreEqual(actualTimes.Length, 3);
            Assert.AreEqual(actualTimes[0].ToString(), expectedTime1);
            Assert.AreEqual(actualTimes[1].ToString(), expectedTime2);
            Assert.AreEqual(actualTimes[2].ToString(), expectedTime3);

        }



        [TestCase(18, "17:20:00", "17:30:00", "17:35:00")]
        [TestCase(20, "17:20:00", "17:25:00", "17:30:00")]
        public void JsonTimeStorage_GetScheduleToWork(int day, string expectedTime1, string expectedTime2, string expectedTime3)
        {
            var actualTimes = _target.GetNearestToWork(new DateTime(2020, 3, day, 17, 17, 0));

            Assert.IsNotEmpty(actualTimes);
            Assert.AreEqual(actualTimes.Length, 3);
            Assert.AreEqual(actualTimes[0].ToString(), expectedTime1);
            Assert.AreEqual(actualTimes[1].ToString(), expectedTime2);
            Assert.AreEqual(actualTimes[2].ToString(), expectedTime3);

        }
    }
}
