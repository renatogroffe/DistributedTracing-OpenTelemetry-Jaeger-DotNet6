using Refit;
using APIRefitConsumoContagem.Models;

namespace APIRefitConsumoContagem.Clients;

public interface IAPIConsumoContagem
{
    [Get("/consumoapicontagem")]
    Task<ResultadoContador> Get();
}