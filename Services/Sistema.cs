using ServicosTecnicos.DAL;
using ServicosTecnicos.Models;

namespace ServicosTecnicos.Services;

public class Sistema
{
    private readonly ClienteDAL _clienteDAL = new();
    private readonly TecnicoDAL _tecnicoDAL = new();
    private readonly OrdemServicoDAL _ordemDAL = new();

    public void Executar()
    {
        string opcao;
        do
        {
            ExibirMenu();
            opcao = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine();
            try
            {
                switch (opcao)
                {
                    case "1": CadastrarCliente(); break;
                    case "2": CadastrarTecnico(); break;
                    case "3": AbrirOrdem(); break;
                    case "4": ExecutarOrdem(); break;
                    case "5": FinalizarOrdem(); break;
                    case "6": ListarClientes(); break;
                    case "7": ListarTecnicos(); break;
                    case "8": ListarOrdens(); break;
                    case "0": Console.WriteLine("Sistema encerrado."); break;
                    default: Console.WriteLine("Opção inválida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível concluir a operação: {ex.Message}");
            }
            if (opcao != "0")
            {
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
                Console.Clear();
            }
        } while (opcao != "0");
    }

    private static void ExibirMenu()
    {
        Console.WriteLine("=== SERVIÇOS TÉCNICOS ===");
        Console.WriteLine("1 - Cadastrar cliente");
        Console.WriteLine("2 - Cadastrar técnico");
        Console.WriteLine("3 - Abrir ordem de serviço");
        Console.WriteLine("4 - Executar ordem de serviço");
        Console.WriteLine("5 - Finalizar ordem de serviço");
        Console.WriteLine("6 - Listar clientes");
        Console.WriteLine("7 - Listar técnicos");
        Console.WriteLine("8 - Listar ordens de serviço");
        Console.WriteLine("0 - Sair");
        Console.Write("Opção: ");
    }

    private void CadastrarCliente()
    {
        string nome = LerTexto("Nome: ");
        string telefone = LerTexto("Telefone: ");
        string email = LerTexto("E-mail: ");
        _clienteDAL.Salvar(new Cliente { Nome = nome, Telefone = telefone, Email = email });
        Console.WriteLine("Cliente cadastrado com sucesso.");
    }

    private void CadastrarTecnico()
    {
        string nome = LerTexto("Nome: ");
        string especialidade = LerTexto("Especialidade: ");
        _tecnicoDAL.Salvar(new Tecnico { Nome = nome, Especialidade = especialidade });
        Console.WriteLine("Técnico cadastrado com sucesso.");
    }

    private void AbrirOrdem()
    {
        List<Cliente> clientes = _clienteDAL.Listar();
        if (clientes.Count == 0) { Console.WriteLine("Cadastre um cliente primeiro."); return; }
        MostrarClientes(clientes);
        int clienteId = LerId("Id do cliente: ");
        if (!clientes.Any(c => c.Id == clienteId)) { Console.WriteLine("Id de cliente inválido."); return; }

        Console.WriteLine("1 - Manutencao (R$ 150,00)");
        Console.WriteLine("2 - Instalacao (R$ 200,00)");
        Console.WriteLine("3 - Suporte (R$ 100,00)");
        Console.Write("Tipo de serviço: ");
        TipoServico? tipo = Console.ReadLine()?.Trim() switch
        {
            "1" => new Manutencao(), "2" => new Instalacao(), "3" => new Suporte(), _ => null
        };
        if (tipo is null) { Console.WriteLine("Tipo de serviço inválido."); return; }

        string descricao = LerTexto("Descrição do problema: ");
        _ordemDAL.AbrirOrdem(new OrdemServico
        {
            ClienteId = clienteId, TipoServico = tipo, Descricao = descricao
        });
        Console.WriteLine("Ordem de serviço aberta com sucesso.");
    }

    private void ExecutarOrdem()
    {
        List<OrdemServico> abertas = _ordemDAL.Listar().Where(o => o.Status == StatusOrdem.Aberta).ToList();
        List<Tecnico> tecnicos = _tecnicoDAL.Listar();
        if (abertas.Count == 0) { Console.WriteLine("Não existem ordens abertas."); return; }
        if (tecnicos.Count == 0) { Console.WriteLine("Cadastre um técnico primeiro."); return; }
        MostrarOrdens(abertas);
        int ordemId = LerId("Id da ordem: ");
        OrdemServico? ordem = abertas.FirstOrDefault(o => o.Id == ordemId);
        if (ordem is null) { Console.WriteLine("Id de ordem inválido ou ordem não está aberta."); return; }
        MostrarTecnicos(tecnicos);
        int tecnicoId = LerId("Id do técnico: ");
        if (!tecnicos.Any(t => t.Id == tecnicoId)) { Console.WriteLine("Id de técnico inválido."); return; }

        if (_ordemDAL.ExecutarOrdem(ordemId, tecnicoId))
        {
            Console.WriteLine(ordem.TipoServico.ExecutarServico());
            Console.WriteLine("Ordem colocada em execução.");
        }
        else Console.WriteLine("A ordem não pôde ser executada.");
    }

    private void FinalizarOrdem()
    {
        List<OrdemServico> emExecucao = _ordemDAL.Listar().Where(o => o.Status == StatusOrdem.EmExecucao).ToList();
        if (emExecucao.Count == 0) { Console.WriteLine("Não existem ordens em execução."); return; }
        MostrarOrdens(emExecucao);
        int ordemId = LerId("Id da ordem: ");
        OrdemServico? ordem = emExecucao.FirstOrDefault(o => o.Id == ordemId);
        if (ordem is null) { Console.WriteLine("Id de ordem inválido ou ordem não está em execução."); return; }

        if (_ordemDAL.FinalizarOrdem(ordemId))
            Console.WriteLine($"Ordem finalizada. Valor a pagar: {ordem.TipoServico.Preco:C}.");
        else Console.WriteLine("A ordem não pôde ser finalizada.");
    }

    private void ListarClientes() => MostrarClientes(_clienteDAL.Listar());
    private void ListarTecnicos() => MostrarTecnicos(_tecnicoDAL.Listar());
    private void ListarOrdens() => MostrarOrdens(_ordemDAL.Listar());

    private static void MostrarClientes(IEnumerable<Cliente> clientes)
    {
        Console.WriteLine("\n--- Clientes ---");
        foreach (Cliente c in clientes) Console.WriteLine($"{c.Id} - {c.Nome} | {c.Telefone} | {c.Email}");
    }

    private static void MostrarTecnicos(IEnumerable<Tecnico> tecnicos)
    {
        Console.WriteLine("\n--- Técnicos ---");
        foreach (Tecnico t in tecnicos) Console.WriteLine($"{t.Id} - {t.Nome} | {t.Especialidade}");
    }

    private static void MostrarOrdens(IEnumerable<OrdemServico> ordens)
    {
        Console.WriteLine("\n--- Ordens de serviço ---");
        foreach (OrdemServico o in ordens)
        {
            string tecnico = o.TecnicoNome ?? "Não atribuído";
            string valor = o.Valor.HasValue ? o.Valor.Value.ToString("C") : "Pendente";
            Console.WriteLine($"{o.Id} - Cliente: {o.ClienteNome} | Técnico: {tecnico}");
            Console.WriteLine($"    {o.TipoServico.Nome} | {o.Status} | Valor: {valor} | {o.Descricao}");
        }
    }

    private static string LerTexto(string mensagem)
    {
        Console.Write(mensagem);
        string valor = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(valor)) throw new ArgumentException("O campo não pode ficar vazio.");
        return valor;
    }

    private static int LerId(string mensagem)
    {
        Console.Write(mensagem);
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            throw new ArgumentException("Id inválido.");
        return id;
    }
}
