namespace ServicosTecnicos.Models;

public class Manutencao : TipoServico
{
    public override string Nome => "Manutencao";
    public override decimal Preco => 150.00m;
    public override string ExecutarServico() => "Realizando diagnóstico e manutenção do equipamento.";
}
