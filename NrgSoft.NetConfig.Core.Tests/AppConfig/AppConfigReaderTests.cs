using System;
using Xunit;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    public class AppConfigReaderTests
    {
        [Fact]
        public void ReadConfiguration_Not_Throws_With_Valid_Configuration()
        {
            var configurationReader = new AppConfigReader(TestData.ValidConfigurationValues);
            var configuration = configurationReader.ReadConfiguration<Configuration>();

            Assert.Equal(configuration.Version, 2);
            Assert.Equal(configuration.UsersCount, 100000000000000L);
            Assert.Equal(configuration.Epsilon, 0.001d);
            Assert.Equal(configuration.MinimumTransferAmount, 2.5555555555555M);
            Assert.Equal(configuration.Revision, 'A');
            Assert.Equal(configuration.CachingType, CachingType.Redis);
            Assert.Equal(configuration.SiteName, "localhost");
            Assert.Equal(configuration.Api.MainEndpoint, "/calculate");
            Assert.Equal(configuration.Api.SessionLength, TimeSpan.FromMinutes(30));
            Assert.Equal(configuration.Api.Authentication.Username, "Viktor");
            Assert.Equal(configuration.Api.Authentication.PasswordExpires.ToUniversalTime(), new DateTime(2014, 6, 2, 22, 14, 0));
            Assert.Equal(configuration.Api.Authentication.ShouldChangePassword, true);
        }

        [Fact]
        public void ReadConfiguration_Should_Throw_With_Invalid_Configuration()
        {
            var configurationReader = new AppConfigReader(TestData.InvalidConfigurationValues);

            Assert.Throws<InvalidOperationException>(() =>
            {
                configurationReader.ReadConfiguration<Configuration>();
            });
        }

        [Fact]
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
