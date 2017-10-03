using System;
namespace UIA_Web { 
public static class ExternalConfiguration
{
    private static readonly Lazy<ExternalConfigurationManager> configuredInstance = new Lazy<ExternalConfigurationManager>(
        () =>
        {
            return new ExternalConfigurationManager();
        });

    public static ExternalConfigurationManager Instance => configuredInstance.Value;
}
}
