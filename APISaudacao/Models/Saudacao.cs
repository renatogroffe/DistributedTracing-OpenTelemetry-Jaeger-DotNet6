namespace APISaudacao.Models;

public class Saudacao
{
    public string Horario { get; init; }
    public string Mensagem { get; init; }

    public Saudacao()
    {
        var horarioAtual = DateTime.Now;
        Horario = $"{horarioAtual:HH:mm:ss}";
        
        if (horarioAtual.Hour >= 18)
            Mensagem = "Boa noite";
        if (horarioAtual.Hour >= 12)
            Mensagem = "Boa tarde";
        else
            Mensagem = "Bom dia";
    }
}