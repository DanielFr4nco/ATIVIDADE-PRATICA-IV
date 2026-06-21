namespace ServicosTecnicos.Models;

public class Suporte : TipoServico
{
    public override string Nome => "Suporte";
    public override decimal Preco => 100.00m;
    public override string ExecutarServico() => "Prestando suporte técnico ao cliente.";
}
