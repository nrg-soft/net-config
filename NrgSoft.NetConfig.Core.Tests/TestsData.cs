using System;
using System.Collections.Generic;

namespace NrgSoft.NetConfig.Core
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public static class TestData
    {
        public const string XmlConfiguration =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<appSettings>
  <add key=""Version"" value=""Int32"" />
  <add key=""UsersCount"" value=""Int64"" />
  <add key=""Epsilon"" value=""Double"" />
  <add key=""MinimumTransferAmount"" value=""Decimal"" />
  <add key=""Revision"" value=""Char"" />
  <add key=""CachingType"" value=""InMemory | Redis | Memcached"" />
  <add key=""SiteName"" value=""String"" />
  <add key=""Api.MainEndpoint"" value=""String"" />
  <add key=""Api.SessionLength"" value=""TimeSpan"" />
  <add key=""Api.Authentication.Username"" value=""String"" />
  <add key=""Api.Authentication.PasswordExpires"" value=""DateTime"" />
  <add key=""Api.Authentication.ShouldChangePassword"" value=""Boolean"" />
</appSettings>";

        public static readonly Dictionary<string, string> ValidConfigurationValues = new Dictionary<string, string>
        {
            ["Version"] = "2",
            ["UsersCount"] = "100000000000000",
            ["Epsilon"] = "0.001",
            ["MinimumTransferAmount"] = "2.5555555555555",
            ["Revision"] = "A",
            ["CachingType"] = "Redis",
            ["SiteName"] = "localhost",
            ["Api.MainEndpoint"] = "/calculate",
            ["Api.SessionLength"] = "00:30:00",
            ["Api.Authentication.Username"] = "Viktor",
            ["Api.Authentication.PasswordExpires"] = "2014-06-02T22:14:00.0000000Z",
            ["Api.Authentication.ShouldChangePassword"] = "true"
        };

        public static readonly Dictionary<string, string> InvalidConfigurationValues = new Dictionary<string, string>
        {
            ["Version"] = "2",
            ["UsersCount"] = "100000000000000",
            ["Epsilon"] = "0.001",
            ["MinimumTransferAmount"] = "2.5555555555555",
            ["Revision"] = "A",
            ["CachingType"] = "Redis",
            ["SiteName"] = "localhost",
            ["Api.Authentication.PasswordExpires"] = "2014-06-02T22:14:00.0000000Z",
            ["Api.Authentication.ShouldChangePassword"] = "true"
        };

        public static readonly Dictionary<string, string> UnsupportedConfigurationValues = new Dictionary<string, string>
        {
            ["Version"] = "2"
        };
    }

    public class Configuration
    {
        public int Version { get; set; }
        public long UsersCount { get; set; }
        public double Epsilon { get; set; }
        public decimal MinimumTransferAmount { get; set; }
        public char Revision { get; set; }

        public CachingType CachingType { get; set; }
        public string SiteName { get; set; }

        public ApiConfiguration Api { get; set; }
    }

    public class ApiConfiguration
    {
        public string MainEndpoint { get; set; }
        public TimeSpan SessionLength { get; set; }
        public AuthenticationConfiguration Authentication { get; set; }
    }

    public class AuthenticationConfiguration
    {
        public string Username { get; set; }
        public DateTime PasswordExpires { get; set; }
        public bool ShouldChangePassword { get; set; }
    }

    public enum CachingType
    {
        InMemory,
        Redis,
        Memcached
    }

    public class UnsupportedConfiguration
    {
        public short Version { get; set; }
    }
}
