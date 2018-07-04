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

            Assert.Equal(2, configuration.Version);
            Assert.Equal(100000000000000L, configuration.UsersCount);
            Assert.Equal(0.001d, configuration.Epsilon);
            Assert.Equal(2.5555555555555M, configuration.MinimumTransferAmount);
            Assert.Equal('A', configuration.Revision);
            Assert.Equal(CachingType.Redis, configuration.CachingType);
            Assert.Equal("localhost", configuration.SiteName);
            Assert.Equal("/calculate", configuration.Api.MainEndpoint);
            Assert.Equal(TimeSpan.FromMinutes(30), configuration.Api.SessionLength);
            Assert.Equal("Viktor", configuration.Api.Authentication.Username);
            Assert.Equal(new DateTime(2014, 6, 2, 22, 14, 0), configuration.Api.Authentication.PasswordExpires.ToUniversalTime());
            Assert.True(configuration.Api.Authentication.ShouldChangePassword);
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
