using NUnit.Framework;
using System;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    [TestFixture]
    public class AppConfigReaderTests
    {
        [Test]
        public void ReadConfiguration_Not_Throws_With_Valid_Configuration()
        {
            var configurationReader = new AppConfigReader(TestData.ValidConfigurationValues);
            var configuration = configurationReader.ReadConfiguration<Configuration>();

            Assert.AreEqual(configuration.Version, 2);
            Assert.AreEqual(configuration.UsersCount, 100000000000000L);
            Assert.AreEqual(configuration.Epsilon, 0.001d);
            Assert.AreEqual(configuration.MinimumTransferAmount, 2.5555555555555M);
            Assert.AreEqual(configuration.Revision, 'A');
            Assert.AreEqual(configuration.CachingType, CachingType.Redis);
            Assert.AreEqual(configuration.SiteName, "localhost");
            Assert.AreEqual(configuration.Api.MainEndpoint, "/calculate");
            Assert.AreEqual(configuration.Api.SessionLength, TimeSpan.FromMinutes(30));
            Assert.AreEqual(configuration.Api.Authentication.Username, "Viktor");
            Assert.AreEqual(configuration.Api.Authentication.PasswordExpires.ToUniversalTime(), new DateTime(2014, 6, 2, 22, 14, 0));
            Assert.AreEqual(configuration.Api.Authentication.ShouldChangePassword, true);
        }

        [Test]
        public void ReadConfiguration_Should_Throw_With_Invalid_Configuration()
        {
            var configurationReader = new AppConfigReader(TestData.InvalidConfigurationValues);

            Assert.Throws<InvalidOperationException>(() =>
            {
                configurationReader.ReadConfiguration<Configuration>();
            });
        }

        [Test]
        public void ReadConfiguration_Should_Throw_With_Unsupported_Configuration()
        {
            var configurationReader = new AppConfigReader(TestData.UnsupportedConfigurationValues);

            Assert.Throws<InvalidOperationException>(() =>
            {
                configurationReader.ReadConfiguration<UnsupportedConfiguration>();
            });
        }
    }
}
