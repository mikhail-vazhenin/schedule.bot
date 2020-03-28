using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Schedule.Bot.Clients.GoogleAnalitics;
using Schedule.Bot.Services;
using Schedule.Bot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schedule.Bot.Tests.Services
{
    [TestFixture]
    public class MessageServiceTests
    {
        Mock<ITimeStorage> _timeStorageMock;
        Mock<IGoogleAnalyticsClient> _googleAnalyticsCLientMock;
        Mock<IHolidayStorage> _holidayStorageMock;
        Mock<IConfiguration> _configurationMock;

        MessageServiceStub _target = null;

        [SetUp]
        public void Init()
        {
            _timeStorageMock = new Mock<ITimeStorage>();
            _googleAnalyticsCLientMock = new Mock<IGoogleAnalyticsClient>();
            _holidayStorageMock = new Mock<IHolidayStorage>();
            _configurationMock = new Mock<IConfiguration>();

            _target = new MessageServiceStub(
                _timeStorageMock.Object,
                _holidayStorageMock.Object,
                _googleAnalyticsCLientMock.Object,
                _configurationMock.Object);
        }

        class MessageServiceStub : MessageService
        {
            public MessageServiceStub(ITimeStorage timeStorage, IHolidayStorage holidayStorage, IGoogleAnalyticsClient googleAnalyticsClient, IConfiguration configuration)
                : base(timeStorage, holidayStorage, null, googleAnalyticsClient, configuration)
            {
            }

            public DateTime UtcNow { get; set; }
            protected override DateTime GetDateTimeNow()
            {
                return UtcNow;
            }
        }
    }
}
