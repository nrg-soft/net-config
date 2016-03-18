using NUnit.Framework;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    [TestFixture]
    public class AppConfigTemplateGeneratorTests
    {
        [Test]
        public void GenerateTemplateTest()
        {
            var generator = new AppConfigTemplateGenerator();
            var template = generator.GenerateTemplate<Configuration>();

            Assert.AreEqual(TestData.XmlConfiguration, TestsHelper.GetPrintedXml(template));
        }
    }
}
