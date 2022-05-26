using System.Diagnostics;
using System.Net.Http.Headers;
using APIConsumoContagem.Models;
using APIConsumoContagem.Tracing;

namespace APIConsumoContagem.Clients;

public class APISaudacaoClient
{
    private HttpClient _client;
    private IConfiguration _configuration;
    private ILogger<APISaudacaoClient> _logger;
    private readonly ActivitySource _activitySource;

    public APISaudacaoClient(
        HttpClient client, IConfiguration configuration,
        ILogger<APISaudacaoClient> logger)
    {
        _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(APISaudacaoClient)})");
        activity!.SetTag("horario", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");

        _client = client;

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _configuration = configuration;
        _logger = logger;
    }

    public Saudacao ObterSaudacao()
    {
        var urlAPISaudacao = _configuration.GetSection("UrlAPISaudacao").Value;
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(APISaudacaoClient)})");
        activity!.SetTag("urlConsumo", urlAPISaudacao);

        var resultado = _client.GetFromJsonAsync<Saudacao>(urlAPISaudacao).Result;
        _logger.LogInformation($"{resultado!.Horario} - {resultado!.Mensagem}");

        return resultado;
    }
}