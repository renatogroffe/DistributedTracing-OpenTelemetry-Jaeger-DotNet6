using Microsoft.AspNetCore.Mvc;
using APISaudacao.Models;

namespace APISaudacao.Controllers;

[ApiController]
[Route("[controller]")]
public class SaudacaoController : ControllerBase
{
    private readonly ILogger<SaudacaoController> _logger;

    public SaudacaoController(ILogger<SaudacaoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public Saudacao Get()
    {
        var saudacao = new Saudacao();
        _logger.LogInformation($"{saudacao.Horario} - {saudacao.Mensagem}");
        return saudacao;
    }
}