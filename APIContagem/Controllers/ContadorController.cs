using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using StackExchange.Redis;
using APIContagem.Models;
using APIContagem.Logging;
using APIContagem.Tracing;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class ContadorController : ControllerBase
{
    private readonly ILogger<ContadorController> _logger;
    private readonly ConnectionMultiplexer _connectionRedis;
    private readonly ActivitySource _activitySource;

    public ContadorController(ILogger<ContadorController> logger,
        ConnectionMultiplexer connectionRedis)
    {
        _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(ContadorController)})");
        activity!.SetTag("horario", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");

        _logger = logger;
        _connectionRedis = connectionRedis;
    }

    [HttpGet]
    public ResultadoContador Get()
    {
        using var activity =
            _activitySource.StartActivity($"{nameof(Get)} ({nameof(ContadorController)})");

        var valorAtualContador =
            (int)_connectionRedis.GetDatabase().StringIncrement("APIContagem");;

        _logger.LogValorAtual(valorAtualContador);

        activity!.SetTag("valorContador", valorAtualContador);
        activity!.SetTag("producer", OpenTelemetryExtensions.Local);
        activity!.SetTag("kernel", OpenTelemetryExtensions.Kernel);
        activity!.SetTag("framework", OpenTelemetryExtensions.Framework);
        activity!.SetTag("mensagem", OpenTelemetryExtensions.Local);

        return new ()
        {
            ValorAtual = valorAtualContador,
            Producer = OpenTelemetryExtensions.Local,
            Kernel = OpenTelemetryExtensions.Kernel,
            Framework = OpenTelemetryExtensions.Framework,
            Mensagem = OpenTelemetryExtensions.Local
        };
    }
}