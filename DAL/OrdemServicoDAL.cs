using Microsoft.Data.SqlClient;
using ServicosTecnicos.Models;

namespace ServicosTecnicos.DAL;

public class OrdemServicoDAL
{
    public void AbrirOrdem(OrdemServico ordem)
    {
        const string sql = @"INSERT INTO OrdensServico
            (ClienteId, TipoServico, Descricao, Status, DataAbertura)
            VALUES (@ClienteId, @TipoServico, @Descricao, @Status, @DataAbertura)";
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        comando.Parameters.AddWithValue("@ClienteId", ordem.ClienteId);
        comando.Parameters.AddWithValue("@TipoServico", ordem.TipoServico.Nome);
        comando.Parameters.AddWithValue("@Descricao", ordem.Descricao);
        comando.Parameters.AddWithValue("@Status", StatusOrdem.Aberta.ToString());
        comando.Parameters.AddWithValue("@DataAbertura", DateTime.Now);
        conexao.Open();
        comando.ExecuteNonQuery();
    }

    public bool ExecutarOrdem(int ordemId, int tecnicoId)
    {
        const string sql = @"UPDATE OrdensServico SET TecnicoId = @TecnicoId,
            Status = @NovoStatus, DataExecucao = @DataExecucao
            WHERE Id = @Id AND Status = @StatusAtual";
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        comando.Parameters.AddWithValue("@TecnicoId", tecnicoId);
        comando.Parameters.AddWithValue("@NovoStatus", StatusOrdem.EmExecucao.ToString());
        comando.Parameters.AddWithValue("@DataExecucao", DateTime.Now);
        comando.Parameters.AddWithValue("@Id", ordemId);
        comando.Parameters.AddWithValue("@StatusAtual", StatusOrdem.Aberta.ToString());
        conexao.Open();
        return comando.ExecuteNonQuery() > 0;
    }

    public bool FinalizarOrdem(int ordemId)
    {
        const string consulta = "SELECT TipoServico FROM OrdensServico WHERE Id = @Id AND Status = @Status";
        using SqlConnection conexao = Conexao.Criar();
        conexao.Open();
        using SqlCommand comandoConsulta = new(consulta, conexao);
        comandoConsulta.Parameters.AddWithValue("@Id", ordemId);
        comandoConsulta.Parameters.AddWithValue("@Status", StatusOrdem.EmExecucao.ToString());
        object? tipoNome = comandoConsulta.ExecuteScalar();
        if (tipoNome is null) return false;

        TipoServico tipo = TipoServico.Criar((string)tipoNome);
        const string atualizacao = @"UPDATE OrdensServico SET Status = @Status,
            DataFinalizacao = @DataFinalizacao, Valor = @Valor WHERE Id = @Id";
        using SqlCommand comando = new(atualizacao, conexao);
        comando.Parameters.AddWithValue("@Status", StatusOrdem.Finalizada.ToString());
        comando.Parameters.AddWithValue("@DataFinalizacao", DateTime.Now);
        comando.Parameters.AddWithValue("@Valor", tipo.Preco);
        comando.Parameters.AddWithValue("@Id", ordemId);
        return comando.ExecuteNonQuery() > 0;
    }

    public List<OrdemServico> Listar()
    {
        const string sql = @"SELECT o.Id, o.ClienteId, o.TecnicoId, c.Nome, t.Nome,
            o.TipoServico, o.Descricao, o.Status, o.DataAbertura, o.DataExecucao,
            o.DataFinalizacao, o.Valor FROM OrdensServico o
            INNER JOIN Clientes c ON c.Id = o.ClienteId
            LEFT JOIN Tecnicos t ON t.Id = o.TecnicoId ORDER BY o.Id DESC";
        var ordens = new List<OrdemServico>();
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        conexao.Open();
        using SqlDataReader leitor = comando.ExecuteReader();
        while (leitor.Read())
        {
            ordens.Add(new OrdemServico
            {
                Id = leitor.GetInt32(0), ClienteId = leitor.GetInt32(1),
                TecnicoId = leitor.IsDBNull(2) ? null : leitor.GetInt32(2),
                ClienteNome = leitor.GetString(3), TecnicoNome = leitor.IsDBNull(4) ? null : leitor.GetString(4),
                TipoServico = TipoServico.Criar(leitor.GetString(5)), Descricao = leitor.GetString(6),
                Status = Enum.Parse<StatusOrdem>(leitor.GetString(7)), DataAbertura = leitor.GetDateTime(8),
                DataExecucao = leitor.IsDBNull(9) ? null : leitor.GetDateTime(9),
                DataFinalizacao = leitor.IsDBNull(10) ? null : leitor.GetDateTime(10),
                Valor = leitor.IsDBNull(11) ? null : leitor.GetDecimal(11)
            });
        }
        return ordens;
    }
}
