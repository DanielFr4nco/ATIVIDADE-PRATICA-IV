namespace ServicosTecnicos.Models;

public abstract class TipoServico
{
    public abstract string Nome { get; }
    public abstract decimal Preco { get; }
    public abstract string ExecutarServico();

    public static TipoServico Criar(string nome) => nome switch
    {
        "Manutencao" => new Manutencao(),
        "Instalacao" => new Instalacao(),
        "Suporte" => new Suporte(),
        _ => throw new ArgumentException("Tipo de serviço inválido.")
    };
}
