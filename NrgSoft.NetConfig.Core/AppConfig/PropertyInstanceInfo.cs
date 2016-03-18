using System.Reflection;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    internal class PropertyInstanceInfo
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object Instance { get; set; }
    }
}
