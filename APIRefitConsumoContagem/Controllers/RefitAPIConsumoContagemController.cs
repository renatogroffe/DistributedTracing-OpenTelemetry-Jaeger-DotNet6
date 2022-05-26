using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using APIRefitConsumoContagem.Clients;
using APIRefitConsumoContagem.Logging;
using APIRefitConsumoContagem.Models;
using APIRefitConsumoContagem.Tracing;

namespace APIRefitConsumoContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class RefitAPIConsumoContagemController : ControllerBase
{
    private readonly ILogger<RefitAPIConsumoContagemController> _logger;
    private readonly IAPIConsumoContagem _clientApiConsumoContagem;
    private readonly ActivitySource _activitySource;

    public RefitAPIConsumoContagemController(ILogger<RefitAPIConsumoContagemController> logger,
        IAPIConsumoContagem clientApiConsumoContagem)
    {
        _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(RefitAPIConsumoContagemController)})");
        activity!.SetTag("horario", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");

        _logger = logger;
        _clientApiConsumoContagem = clientApiConsumoContagem;
    }

    [HttpGet]
    public ResultadoContador Get()
    {
        using var activity =
            _activitySource.StartActivity($"{nameof(Get)} ({nameof(RefitAPIConsumoContagemController)})");

        var resultado = _clientApiConsumoContagem.Get().Result;
        resultado.Caller = OpenTelemetryExtensions.Local;
        _logger.LogValorAtual(resultado.ValorAtual);
        resultado.Mensagem = $"{resultado.Origem} > {resultado.Mensagem}";

        activity!.SetTag("valorContador", resultado.ValorAtual);
        activity!.SetTag("caller", resultado.Caller);
        activity!.SetTag("origem", resultado.Origem);
        activity!.SetTag("producer", resultado.Producer);
        activity!.SetTag("kernel", resultado.Kernel);
        activity!.SetTag("framework", resultado.Framework);
        activity!.SetTag("mensagem", resultado.Mensagem);

        return resultado;
    }
}