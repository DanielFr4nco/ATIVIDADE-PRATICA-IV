using Microsoft.Data.SqlClient;

namespace ServicosTecnicos.DAL;

public static class Conexao
{
    private const string ConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=ServicosTecnicosDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection Criar() => new(ConnectionString);
}
