using Microsoft.Data.SqlClient;
using ServicosTecnicos.Models;

namespace ServicosTecnicos.DAL;

public class TecnicoDAL
{
    public void Salvar(Tecnico tecnico)
    {
        const string sql = "INSERT INTO Tecnicos (Nome, Especialidade) VALUES (@Nome, @Especialidade)";
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        comando.Parameters.AddWithValue("@Nome", tecnico.Nome);
        comando.Parameters.AddWithValue("@Especialidade", tecnico.Especialidade);
        conexao.Open();
        comando.ExecuteNonQuery();
    }

    public List<Tecnico> Listar()
    {
        const string sql = "SELECT Id, Nome, Especialidade FROM Tecnicos ORDER BY Nome";
        var tecnicos = new List<Tecnico>();
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        conexao.Open();
        using SqlDataReader leitor = comando.ExecuteReader();
        while (leitor.Read())
            tecnicos.Add(new Tecnico { Id = leitor.GetInt32(0), Nome = leitor.GetString(1), Especialidade = leitor.GetString(2) });
        return tecnicos;
    }
}
