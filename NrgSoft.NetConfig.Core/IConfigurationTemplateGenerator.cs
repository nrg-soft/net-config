using System;

namespace NrgSoft.NetConfig.Core
{
    public interface IConfigurationTemplateGenerator<out TTemplate>
    {
        TTemplate GenerateTemplate<TConfigurationType>();
        TTemplate GenerateTemplate(Type configurationType);
    }
}
