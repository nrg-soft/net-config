using System.IO;
using System.Xml;

namespace NrgSoft.NetConfig.Core
{
    public static class TestsHelper
    {
        public static string GetPrintedXml(XmlDocument xml)
        {
            using (var mem = new MemoryStream())
            using (var stream = new StreamWriter(mem))
            {
                xml.Save(stream);

                stream.Flush();
                mem.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(mem))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
