using Microsoft.Data.SqlClient;
using ServicosTecnicos.Models;

namespace ServicosTecnicos.DAL;

public class ClienteDAL
{
    public void Salvar(Cliente cliente)
    {
        const string sql = "INSERT INTO Clientes (Nome, Telefone, Email) VALUES (@Nome, @Telefone, @Email)";
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        comando.Parameters.AddWithValue("@Nome", cliente.Nome);
        comando.Parameters.AddWithValue("@Telefone", cliente.Telefone);
        comando.Parameters.AddWithValue("@Email", cliente.Email);
        conexao.Open();
        comando.ExecuteNonQuery();
    }

    public List<Cliente> Listar()
    {
        const string sql = "SELECT Id, Nome, Telefone, Email FROM Clientes ORDER BY Nome";
        var clientes = new List<Cliente>();
        using SqlConnection conexao = Conexao.Criar();
        using SqlCommand comando = new(sql, conexao);
        conexao.Open();
        using SqlDataReader leitor = comando.ExecuteReader();
        while (leitor.Read())
            clientes.Add(new Cliente { Id = leitor.GetInt32(0), Nome = leitor.GetString(1), Telefone = leitor.GetString(2), Email = leitor.GetString(3) });
        return clientes;
    }
}
