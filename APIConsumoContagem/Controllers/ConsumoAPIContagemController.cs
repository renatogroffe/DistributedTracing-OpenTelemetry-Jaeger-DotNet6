using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using APIConsumoContagem.Clients;
using APIConsumoContagem.Logging;
using APIConsumoContagem.Models;
using APIConsumoContagem.Tracing;

namespace APIConsumoContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsumoAPIContagemController : ControllerBase
{
    private readonly ILogger<ConsumoAPIContagemController> _logger;
    private readonly APIContagemClient _contagemClient;
    private readonly ActivitySource _activitySource;
    private readonly APISaudacaoClient _saudacaoClient;

    public ConsumoAPIContagemController(
        ILogger<ConsumoAPIContagemController> logger,
        APIContagemClient contagemClient,
        APISaudacaoClient saudacaoClient)
    {
        _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(ConsumoAPIContagemController)})");
        activity!.SetTag("horario", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");

        _logger = logger;
        _contagemClient = contagemClient;
        _saudacaoClient = saudacaoClient;
    }

    [HttpGet]
    public ResultadoContador Get()
    {
        using var activity =
            _activitySource.StartActivity($"{nameof(Get)} ({nameof(ConsumoAPIContagemController)})");
        
        var resultadoContador = _contagemClient.ObterDadosContagem();
        _logger.LogValorAtual(resultadoContador.ValorAtual);

        var saudacao = _saudacaoClient.ObterSaudacao();
        resultadoContador.Mensagem +=
            $" | {nameof(APIConsumoContagem)} > APISaudacao - {saudacao.Horario} - {saudacao.Mensagem}";

        activity!.SetTag("valorContador", resultadoContador.ValorAtual);
        activity!.SetTag("origem", OpenTelemetryExtensions.Local);
        activity!.SetTag("producer", resultadoContador.Producer);
        activity!.SetTag("kernel", resultadoContador.Kernel);
        activity!.SetTag("framework", resultadoContador.Framework);
        activity!.SetTag("saudacao", saudacao.Mensagem);
        activity!.SetTag("mensagem", resultadoContador.Mensagem);

        return resultadoContador;
    }
}