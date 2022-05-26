namespace APIRefitConsumoContagem.Logging;

public static partial class ContagemLogging
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information,
        Message = "APIRefitConsumoContagem - Valor atual: {valorAtual}")]
    public static partial void LogValorAtual(
        this ILogger logger, int valorAtual);
}