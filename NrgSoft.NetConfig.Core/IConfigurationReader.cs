namespace NrgSoft.NetConfig.Core
{
    public interface IConfigurationReader
    {
        TConfigurationType ReadConfiguration<TConfigurationType>() where TConfigurationType : new();
    }
}
