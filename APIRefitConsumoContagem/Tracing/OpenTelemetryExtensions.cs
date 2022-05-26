using System.Diagnostics;

namespace APIRefitConsumoContagem.Tracing;

public static class OpenTelemetryExtensions
{
    public static string Local { get; }
    public static string ServiceName { get; }
    public static string ServiceVersion { get; }

    static OpenTelemetryExtensions()
    {
        Local = "APIRefitConsumoContagem";
        ServiceName = typeof(OpenTelemetryExtensions).Assembly.GetName().Name!;
        ServiceVersion = typeof(OpenTelemetryExtensions).Assembly.GetName().Version!.ToString();
    }

    public static ActivitySource CreateActivitySource() =>
        new ActivitySource(ServiceName, ServiceVersion);
}