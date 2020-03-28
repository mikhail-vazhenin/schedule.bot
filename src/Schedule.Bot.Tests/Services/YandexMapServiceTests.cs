using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using Schedule.Bot.Services;
using System;

namespace Schedule.Bot.Tests
{
    [TestFixture]
    public class YandexMapServiceTests
    {
        Mock<IMemoryCache> _memoryCacheMock;
        YandexMapService _target;

        [SetUp]
        public void Init()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _memoryCacheMock.Setup(s => s.CreateEntry(It.IsAny<object>())).Returns(new Mock<ICacheEntry>().Object);
            _target = new YandexMapService(_memoryCacheMock.Object, null);
        }

        [Ignore("Not working")]
        public void YandexMapService_GetDurationToMetro()
        {
            var actualDuration = _target.GetDurationToMetro().Result;

            Assert.NotNull(actualDuration);
        }



        [Ignore("Not working")]
        public void YandexMapService_GetDurationToWork()
        {
            var actualDuration = _target.GetDurationToWork().Result;

            Assert.NotNull(actualDuration);

        }
    }
}
