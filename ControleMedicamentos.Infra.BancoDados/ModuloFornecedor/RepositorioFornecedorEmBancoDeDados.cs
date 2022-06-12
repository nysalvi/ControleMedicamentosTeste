using ControleMedicamentos.Dominio.ModuloFornecedor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDeDados
    {
        private static readonly string databaseConnection =
            "(localdb)\\MSSQLLocalDB;Initial Catalog=ControleMedicamentos;" +
            "Integrated Security=True;" +
            "Pooling=False";
        #region SQL Queries
        private const string sqlInserir =
           @"INSERT INTO [TBFornecedor] 
                (
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
	            )
	            VALUES
                (
                    @NOME,
                    @TELEFONE,   
                    @EMAIL,
                    @CIDADE,
                    @ESTADO
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBFornecedor]	
		        SET
			        [NOME] = @NOME,
                    [TELEFONE] = @TELEFONE,
                    [EMAIL] = @EMAIL,
                    [CIDADE] = @CIDADE,
			        [ESTADO] = @ESTADO
                WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBFornecedor]
		        WHERE
			        [ID] = @ID";
        private const string sqlSelecionarPorNumero =
           @"SELECT
	                [ID],
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
              FROM 
	                [TBFornecedor]
              WHERE 
	                [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT
	                [ID],
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
              FROM 
	                [TBFornecedor]";
        #endregion

        public void Inserir(Fornecedor fornecedor)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarFornecedor(fornecedor, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            fornecedor.Numero = Convert.ToInt32(id);
            sqlConnection.Close();
        }
        public void Editar(Fornecedor fornecedor)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarFornecedor(fornecedor, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        public void Excluir(Fornecedor fornecedor)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", fornecedor.Numero);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<Fornecedor> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Fornecedor> fornecedores = new List<Fornecedor>();

            while (sqlDataReader.Read())
            {
                Fornecedor fornecedor = ConverterFornecedor(sqlDataReader);

                fornecedores.Add(fornecedor);
            }
            return fornecedores;
        }
        public Fornecedor SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorNumero, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Fornecedor fornecedor = null;

            if (sqlDataReader.Read())
                fornecedor = ConverterFornecedor(sqlDataReader);

            sqlConnection.Close();

            return fornecedor;
        }
        public static Fornecedor ConverterFornecedor(SqlDataReader leitorPaciente)
        {
            int numero = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = Convert.ToString(leitorPaciente["NOME"]);
            string telefone = Convert.ToString(leitorPaciente["TELEFONE"]);
            string email = Convert.ToString(leitorPaciente["EMAIL"]);
            string cidade = Convert.ToString(leitorPaciente["CIDADE"]);
            string estado = Convert.ToString(leitorPaciente["ESTADO"]);

            var fornecedor = new Fornecedor(nome, telefone, email, cidade, estado)
            {
                Numero = numero
            };

            return fornecedor;
        }
        public static void ConfigurarFornecedor
            (Fornecedor fornecedor, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", fornecedor.Numero);
            sqlCommand.Parameters.AddWithValue("NOME", fornecedor.Nome);
            sqlCommand.Parameters.AddWithValue("TELEFONE", fornecedor.Telefone);
            sqlCommand.Parameters.AddWithValue("EMAIL", fornecedor.Email);
            sqlCommand.Parameters.AddWithValue("CIDADE", fornecedor.Cidade);
            sqlCommand.Parameters.AddWithValue("ESTADO", fornecedor.Estado);
        }

    }
}
