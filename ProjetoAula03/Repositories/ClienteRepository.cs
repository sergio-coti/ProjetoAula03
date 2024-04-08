using Dapper;
using ProjetoAula03.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAula03.Repositories
{
    /// <summary>
    /// Classe de repositório de banco de dados para cliente.
    /// </summary>
    public class ClienteRepository
    {
        /// <summary>
        /// Método para inserir um registro de cliente e endereço no banco de dados
        /// </summary>
        /// <param name="cliente">Objeto da classe de entidade cliente.</param>
        public void Inserir(Cliente cliente)
        {
            //abrindo conexão com o banco de dados
            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BDProjetoAula03;Integrated Security=True;"))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    #region Gravar os dados do endereço do banco de dados

                    var enderecoId = connection.ExecuteScalar<int>(@"
                        INSERT INTO ENDERECO(LOGRADOURO, COMPLEMENTO, NUMERO, BAIRRO, LOCALIDADE, UF, CEP) OUTPUT INSERTED.ID                        
                        VALUES(@Logradouro, @Complemento, @Numero, @Bairro, @Localidade, @Uf, @Cep)
                    ", new
                    {
                        @Logradouro = cliente.Endereco.Logradouro,
                        @Complemento = cliente.Endereco.Complemento,
                        @Numero = cliente.Endereco.Numero,
                        @Bairro = cliente.Endereco.Bairro,
                        @Localidade = cliente.Endereco.Localidade,
                        @Uf = cliente.Endereco.Uf,
                        @Cep = cliente.Endereco.Cep
                    }, transaction);

                    #endregion

                    #region Gravar os dados do cliente no banco de dados

                    connection.Execute(@"
                        INSERT INTO CLIENTE(NOME, CPF, DATANASCIMENTO, ENDERECO_ID)
                        VALUES(@Nome, @Cpf, @DataNascimento, @Endereco_Id)
                    ", new
                    {
                        @Nome = cliente.Nome,
                        @Cpf = cliente.Cpf,
                        @DataNascimento = cliente.DataNascimento,
                        @Endereco_Id = enderecoId
                    }, transaction);

                    #endregion

                    transaction.Commit();
                }
            }
        }
    }
}
