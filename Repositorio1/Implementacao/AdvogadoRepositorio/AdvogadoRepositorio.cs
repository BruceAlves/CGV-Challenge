using Dominio;
using MySqlConnector;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Repository.Implementacao
{
    public class AdvogadoRepositorio : MySqlServerRepositorio, IAdvogadoRepositorio
    {
        private MySqlConnection conexao;

        public AdvogadoRepositorio(string connectionString) : base(connectionString)
        {
            conexao = new MySqlConnection(connectionString);
        }

        public void Atualizar(Advogado advogado)
        {
            try
            {
                AbrirConexao();

                conexao.Open();
                MySqlTransaction transacao = conexao.BeginTransaction();

                try
                {
                    using (MySqlCommand cmdAdvogado = new MySqlCommand("UPDATE Advogados SET Nome = @Nome, Senioridade = @Senioridade WHERE Id = @Id", conexao, transacao))
                    {
                        cmdAdvogado.Parameters.AddWithValue("@Nome", advogado.Nome);
                        cmdAdvogado.Parameters.AddWithValue("@Senioridade", advogado.Senioridade);
                        cmdAdvogado.Parameters.AddWithValue("@Id", advogado.ID);
                        cmdAdvogado.ExecuteNonQuery();
                    }

                    using (MySqlCommand cmdEndereco = new MySqlCommand("UPDATE Endereco SET Logradouro = @Logradouro, Bairro = @Bairro, Estado = @Estado, Cep = @Cep, Numero = @Numero, Complemento = @Complemento WHERE AdvogadoId = @AdvogadoId", conexao, transacao))
                    {
                        cmdEndereco.Parameters.AddWithValue("@Logradouro", advogado.Logradouro);
                        cmdEndereco.Parameters.AddWithValue("@Bairro", advogado.Bairro);
                        cmdEndereco.Parameters.AddWithValue("@Estado", advogado.Estado);
                        cmdEndereco.Parameters.AddWithValue("@Cep", advogado.Cep);
                        cmdEndereco.Parameters.AddWithValue("@Numero", advogado.Numero);
                        cmdEndereco.Parameters.AddWithValue("@Complemento", advogado.Complemento);
                        cmdEndereco.Parameters.AddWithValue("@AdvogadoId", advogado.ID);
                        cmdEndereco.ExecuteNonQuery();
                    }

                    transacao.Commit();
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
            finally
            {
                FecharConexao();
            }
        }


        public Advogado BuscarPorId(int pIntadvogado)
        {
            Advogado advogado = null;

            try
            {
                AbrirConexao();

                conexao.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT a.Nome, a.Senioridade, e.Cep, e.Bairro, e.Logradouro, e.Estado, e.Numero, e.Complemento FROM Advogados a inner join Endereco e on e.AdvogadoId = a.ID WHERE Id = @Id", conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", pIntadvogado);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            advogado = new Advogado
                            {
                                Nome = (string)reader["Nome"],
                                Senioridade = (SenioridadeEnum)reader["Senioridade"],
                                Cep = (string)reader["Cep"],
                                Estado = (string)reader["Estado"],
                                Logradouro = (string)reader["Logradouro"],
                                Bairro = (string)reader["Bairro"],
                                Numero = (string)reader["Numero"],
                                Complemento = (string)reader["Complemento"]
                            };
                        }
                    }
                }
            }
            finally
            {
                FecharConexao();
            }

            return advogado;
        }


        public void Excluir(int pIntadvogado)
        {
            try
            {
                AbrirConexao();
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Advogados WHERE Id = @Id", conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", pIntadvogado);
                    cmd.ExecuteNonQuery();
                }

                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Endereco WHERE AdvogadoId = @Id", conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", pIntadvogado);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public void Incluir(Advogado pObjadvogado)
        {
            try
            {
                AbrirConexao();

                conexao.Open();
                MySqlTransaction transacao = conexao.BeginTransaction();

                try
                {
                    using (MySqlCommand cmdAdvogado = new MySqlCommand("INSERT INTO Advogados (Nome, Senioridade) VALUES (@Nome, @Senioridade); SELECT LAST_INSERT_ID();", conexao, transacao))
                    {
                        cmdAdvogado.Parameters.AddWithValue("@Nome", pObjadvogado.Nome);
                        cmdAdvogado.Parameters.AddWithValue("@Senioridade", pObjadvogado.Senioridade);
                        pObjadvogado.ID = Convert.ToInt32(cmdAdvogado.ExecuteScalar());
                    }

                    using (MySqlCommand cmdEndereco = new MySqlCommand("INSERT INTO Endereco (Logradouro, Bairro, Estado, Cep, Numero, Complemento, AdvogadoId) VALUES (@Logradouro, @Bairro, @Estado, @Cep, @Numero, @Complemento, @AdvogadoId);", conexao, transacao))
                    {
                        cmdEndereco.Parameters.AddWithValue("@Logradouro", pObjadvogado.endereco.Logradouro);
                        cmdEndereco.Parameters.AddWithValue("@Bairro", pObjadvogado.endereco.Bairro);
                        cmdEndereco.Parameters.AddWithValue("@Estado", pObjadvogado.endereco.Estado);
                        cmdEndereco.Parameters.AddWithValue("@Cep", pObjadvogado.endereco.Cep);
                        cmdEndereco.Parameters.AddWithValue("@Numero", pObjadvogado.endereco.Numero);
                        cmdEndereco.Parameters.AddWithValue("@Complemento", pObjadvogado.endereco.Complemento);
                        cmdEndereco.Parameters.AddWithValue("@AdvogadoId", pObjadvogado.ID);
                        cmdEndereco.ExecuteNonQuery();
                    }

                    transacao.Commit();
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public IEnumerable<Advogado> ListarAdvogadoComEndereco()
        {
            List<Advogado> advogados = new List<Advogado>();

            AbrirConexao();


            conexao.Open();
            using (MySqlCommand cmdAdvogado = new MySqlCommand("SELECT * FROM Advogados", conexao))
            using (MySqlDataReader readerAdvogado = cmdAdvogado.ExecuteReader())
            {
                while (readerAdvogado.Read())
                {
                    int idAdvogado = Convert.ToInt32(readerAdvogado["ID"]);
                    Advogado advogado = new Advogado
                    {
                        ID = Convert.ToInt32(readerAdvogado["ID"]),
                        Nome = readerAdvogado["Nome"].ToString(),
                        Senioridade = (SenioridadeEnum)Enum.Parse(typeof(SenioridadeEnum), readerAdvogado["Senioridade"].ToString())
                    };

                    using (MySqlCommand cmdEndereco = new MySqlCommand("SELECT * FROM Endereco WHERE AdvogadoId = @idAdvogado", conexao))
                    {
                        cmdEndereco.Parameters.AddWithValue("@idAdvogado", idAdvogado);
                        using (MySqlDataReader readerEndereco = cmdEndereco.ExecuteReader())
                        {
                            if (readerEndereco.Read())
                            {
                                advogado.endereco = new Endereco
                                {
                                    Logradouro = readerEndereco["Logradouro"].ToString(),
                                    Bairro = readerEndereco["Bairro"].ToString(),
                                    Estado = readerEndereco["Estado"].ToString(),
                                    Cep = readerEndereco["Cep"].ToString(),
                                    Complemento = readerEndereco["Complemento"].ToString(),
                                    Numero = readerEndereco["Numero"].ToString()
                                };
                            }
                        }
                    }

                    advogados.Add(advogado);
                }
            }

            return advogados.AsEnumerable();
        }
    }
}

