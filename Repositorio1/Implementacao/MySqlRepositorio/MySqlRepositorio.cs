using System.Data;
using System.Data.SqlClient;

namespace Repository.Implementacao
{
    public class MySqlServerRepositorio
    {
        private SqlConnection conexao;

        public MySqlServerRepositorio(string connectionString)
        {
            conexao = new SqlConnection(connectionString);
        }

        public void AbrirConexao()
        {
            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();
            }
        }

        public void FecharConexao()
        {
            if (conexao.State == ConnectionState.Open)
            {
                conexao.Close();
            }
        }
    }
}
