using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using static NrgSoft.NetConfig.Core.AppConfig.TypeHelper;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    public class AppConfigReader : IConfigurationReader
    {
        private readonly IDictionary<string, string> _configurationKeyValueDictionary;

        public AppConfigReader(IDictionary<string, string> configurationKeyValueDictionary)
        {
            _configurationKeyValueDictionary = new Dictionary<string, string>(configurationKeyValueDictionary);
        }

        public TConfigurationType ReadConfiguration<TConfigurationType>() where TConfigurationType : new()
        {
            var configuration = new TConfigurationType();
            var configurationType = typeof(TConfigurationType);

            InstantiateConfiguration(configuration, configurationType);
            var configurationPropertyInfos = GetConfigurationPropertyInfos(configuration, configurationType, string.Empty)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            ThrowIfThereIsNotExistingValues(configurationPropertyInfos);

            SetConfigurationValues(configurationPropertyInfos);

            return configuration;
        }

        private static void InstantiateConfiguration(object configuration, Type configurationType)
        {
            var properties = configurationType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var propertyInfo in properties)
            {
                if (IsPrimitiveSupportedType(propertyInfo.PropertyType)) continue; // not needed to do smth

                var innerInstance = Activator.CreateInstance(propertyInfo.PropertyType);
                propertyInfo.SetValue(configuration, innerInstance);
                InstantiateConfiguration(innerInstance, innerInstance.GetType());
            }
        }

        private static IEnumerable<KeyValuePair<string, PropertyInstanceInfo>> GetConfigurationPropertyInfos(object configuration, Type configurationType, string @namespace)
        {
            foreach (var propertyInfo in configurationType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyType = propertyInfo.PropertyType;
                if (IsPrimitiveSupportedType(propertyType) || propertyType.IsEnum)
                {
                    yield return ProducePrimitiveTypeElement(configuration, propertyInfo, @namespace);
                }
                else // is custom class
                {
                    var innerNamespace = $"{@namespace}{propertyInfo.Name}.";
                    var innerPropertyInstanceInfos = GetConfigurationPropertyInfos(propertyInfo.GetValue(configuration), propertyInfo.PropertyType, innerNamespace);
                    foreach (var innerNode in innerPropertyInstanceInfos)
                    {
                        yield return innerNode;
                    }
                }
            }
        }

        private static KeyValuePair<string, PropertyInstanceInfo> ProducePrimitiveTypeElement(object configuration, PropertyInfo propertyInfo, string @namespace)
        {
            var key = $"{@namespace}{propertyInfo.Name}";
            var info = new PropertyInstanceInfo { Instance = configuration, PropertyInfo = propertyInfo };
            return new KeyValuePair<string, PropertyInstanceInfo>(key, info);
        }

        private void ThrowIfThereIsNotExistingValues(Dictionary<string, PropertyInstanceInfo> configurationPropertyInfos)
        {
            var notExistValues = configurationPropertyInfos.Keys.Except(_configurationKeyValueDictionary.Keys).ToList();
            if (notExistValues.Count > 0)
            {
                var errorMessage = $"App.config doesn't contain the following keys:\n{string.Join(";\n", notExistValues)}";
                throw new InvalidOperationException(errorMessage);
            }
        }

        private void SetConfigurationValues(Dictionary<string, PropertyInstanceInfo> configurationPropertyInfos)
        {
            foreach (var pair in configurationPropertyInfos)
            {
                var propertyRawValue = _configurationKeyValueDictionary[pair.Key];
                var propertyInfo = pair.Value.PropertyInfo;

                try
                {
                    var propertyValue = ConvertValue(propertyRawValue, propertyInfo.PropertyType);
                    propertyInfo.SetValue(pair.Value.Instance, propertyValue);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"Can't convert {propertyRawValue} to type {propertyInfo.PropertyType.Name}", e);
                }
            }
        }

        private object ConvertValue(string propertyRawValue, Type propertyType)
        {
            if (propertyType == typeof(int))
            {
                return int.Parse(propertyRawValue);
            }
            else if (propertyType == typeof(long))
            {
                return long.Parse(propertyRawValue);
            }
            else if (propertyType == typeof(double))
            {
                return Convert.ToDouble(propertyRawValue, CultureInfo.InvariantCulture);
            }
            else if (propertyType == typeof(decimal))
            {
                return Convert.ToDecimal(propertyRawValue, CultureInfo.InvariantCulture);
            }
            else if (propertyType == typeof(char))
            {
                return propertyRawValue.First();
            }
            else if (propertyType == typeof(bool))
            {
                return bool.Parse(propertyRawValue);
            }
            else if (propertyType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(propertyRawValue);
            }
            else if (propertyType == typeof(DateTime))
            {
                return Convert.ToDateTime(propertyRawValue, DateTimeFormatInfo.InvariantInfo);
            }
            else if (propertyType == typeof(string))
            {
                return propertyRawValue;
            }
            else if (propertyType.IsEnum)
            {
                return Enum.Parse(propertyType, propertyRawValue);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported primitive type {propertyType.Name}");
            }
        }
    }
}
