using Xunit;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    public class AppConfigTemplateGeneratorTests
    {
        [Fact]
        public void GenerateTemplateTest()
        {
            var generator = new AppConfigTemplateGenerator();
            var template = generator.GenerateTemplate<Configuration>();

            Assert.Equal(TestData.XmlConfiguration, TestsHelper.GetPrintedXml(template));
        }
    }
}
