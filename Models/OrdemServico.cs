namespace ServicosTecnicos.Models;

public class OrdemServico
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int? TecnicoId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string? TecnicoNome { get; set; }
    public TipoServico TipoServico { get; set; } = new Suporte();
    public string Descricao { get; set; } = string.Empty;
    public StatusOrdem Status { get; set; } = StatusOrdem.Aberta;
    public DateTime DataAbertura { get; set; }
    public DateTime? DataExecucao { get; set; }
    public DateTime? DataFinalizacao { get; set; }
    public decimal? Valor { get; set; }
}
