namespace ServicosTecnicos.Models;

public class Instalacao : TipoServico
{
    public override string Nome => "Instalacao";
    public override decimal Preco => 200.00m;
    public override string ExecutarServico() => "Instalando e configurando o equipamento.";
}
