using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Schedule.Bot.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Bot.Tests.Services
{
    class ConfigurationTests
    {
        IConfiguration _target;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings-test.json");

            _target = builder.Build();
        }

        [Test]
        public void GetToMetroTitleTests()
        {
            Assert.AreEqual("to-metro-command", _target.ToMetroTitle());
        }

        [Test]
        public void GetGoogleAnalyticsUrlTests()
        {
            Assert.AreEqual("https://www.google-analytics.com", _target.GoogleAnalyticsUrl());
        }

    }
}
