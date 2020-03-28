using Moq;
using NUnit.Framework;
using Schedule.Bot.Services;
using System;
using System.Linq;

namespace Schedule.Bot.Tests.Services
{
    [TestFixture]
    public class HolidayStorageTests
    {
        private MockRepository mockRepository;

        HolidayStorage _target;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            _target = new HolidayStorage("Holidays.txt");
        }

        [TearDown]
        public void TearDown()
        {
            this.mockRepository.VerifyAll();
        }

        [TestCase("2018-01-01", ExpectedResult = true)]
        [TestCase("2020-03-21", ExpectedResult = true)]
        [TestCase("2020-03-23", ExpectedResult = false)]
        public bool HolidayStorage_GetHolidays(string dateString)
        {
            var date = DateTime.Parse(dateString);
            // Act
            return _target.IsHoliday(date);
        }
    }
}