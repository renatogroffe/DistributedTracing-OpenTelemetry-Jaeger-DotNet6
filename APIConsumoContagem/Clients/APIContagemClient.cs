using System.Diagnostics;
using System.Net.Http.Headers;
using APIConsumoContagem.Models;
using APIConsumoContagem.Logging;
using APIConsumoContagem.Tracing;

namespace APIConsumoContagem.Clients;

public class APIContagemClient
{
    private HttpClient _client;
    private IConfiguration _configuration;
    private ILogger<APIContagemClient> _logger;
    private readonly ActivitySource _activitySource;

    public APIContagemClient(
        HttpClient client, IConfiguration configuration,
        ILogger<APIContagemClient> logger)
    {
        _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(APIContagemClient)})");
        activity!.SetTag("horario", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");

        _client = client;

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _configuration = configuration;
        _logger = logger;
    }

    public ResultadoContador ObterDadosContagem()
    {
        var urlAPIContagem = _configuration.GetSection("UrlAPIContagem").Value;
        using var activity =
            _activitySource.StartActivity($"Construtor ({nameof(APIContagemClient)})");
        activity!.SetTag("urlConsumo", urlAPIContagem);

        var resultado = _client.GetFromJsonAsync<ResultadoContador>(urlAPIContagem).Result;
        _logger.LogValorAtual(resultado!.ValorAtual);
        resultado.Origem = "APIConsumoContagem";
        resultado.Mensagem = $"{resultado.Origem} > {resultado.Mensagem}";

        return resultado;
    }
}