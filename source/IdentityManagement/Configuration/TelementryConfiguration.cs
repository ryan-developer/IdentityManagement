using System;

namespace IdentityManagement.Configuration
{
    public class TelementryConfiguration
    {
        public TelementryConfiguration() => 
            TelemetryKey = Environment.GetEnvironmentVariable(
                "APPINSIGHTS_INSTRUMENTATIONKEY", 
                EnvironmentVariableTarget.Process | EnvironmentVariableTarget.Machine);

        public string TelemetryKey { get; }
    }
}
