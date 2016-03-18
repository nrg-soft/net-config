using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using static NrgSoft.NetConfig.Core.AppConfig.TypeHelper;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    public class AppConfigTemplateGenerator : IConfigurationTemplateGenerator<XmlDocument>
    {
        public XmlDocument GenerateTemplate<TConfigurationType>()
        {
            return GenerateTemplate(typeof(TConfigurationType));
        }

        public XmlDocument GenerateTemplate(Type configurationType)
        {
            var document = new XmlDocument();

            var configurationNodes = CreateTypeConfigurationNodes(configurationType, document);

            var root = document.CreateElement("appSettings");
            foreach (var node in configurationNodes)
            {
                root.AppendChild(node);
            }
            document.AppendChild(root);

            return document;
        }

        private static IEnumerable<XmlNode> CreateTypeConfigurationNodes(Type configurationType, XmlDocument document, string @namespace = "")
        {
            foreach (var propertyInfo in configurationType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyType = propertyInfo.PropertyType;
                if (IsPrimitiveSupportedType(propertyType))
                {
                    yield return ProducePrimitiveTypeElement(document, propertyInfo, @namespace);
                }
                else if (propertyType.IsEnum)
                {
                    yield return ProduceEnumTypeElement(document, propertyInfo, @namespace);
                }
                else // is custom class
                {
                    var innerNamespace = $"{@namespace}{propertyInfo.Name}.";
                    var innerConfigurationNodes = CreateTypeConfigurationNodes(propertyInfo.PropertyType, document, innerNamespace);
                    foreach (var innerNode in innerConfigurationNodes)
                    {
                        yield return innerNode;
                    }
                }
            }
        }

        private static XmlNode ProducePrimitiveTypeElement(XmlDocument document, PropertyInfo propertyInfo, string @namespace)
        {
            var child = document.CreateElement("add");
            child.SetAttribute("key", $"{@namespace}{propertyInfo.Name}");
            child.SetAttribute("value", propertyInfo.PropertyType.Name);
            return child;
        }

        private static XmlNode ProduceEnumTypeElement(XmlDocument document, PropertyInfo propertyInfo, string @namespace)
        {
            var child = document.CreateElement("add");
            child.SetAttribute("key", $"{@namespace}{propertyInfo.Name}");

            var elements = new LinkedList<object>();
            foreach (var element in propertyInfo.PropertyType.GetEnumValues())
            {
                elements.AddLast(element);
            }

            var availableEnumValues = string.Join(" | ", elements);
            child.SetAttribute("value", availableEnumValues);

            return child;
        }
    }
}
