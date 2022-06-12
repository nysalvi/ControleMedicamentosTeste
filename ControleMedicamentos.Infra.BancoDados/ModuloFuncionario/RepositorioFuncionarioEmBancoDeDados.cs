using ControleMedicamentos.Dominio.ModuloFuncionario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDeDados
    {
        private static readonly string databaseConnection =
            "(localdb)\\MSSQLLocalDB;Initial Catalog=ControleMedicamentos;" +
            "Integrated Security=True;" +
            "Pooling=False";
        #region SQL Queries
        private const string sqlInserir =
           @"INSERT INTO [TBFuncionario] 
                (
                    [NOME],
                    [LOGIN],
                    [SENHA]
	            )
	            VALUES
                (
                    @NOME,
                    @LOGIN,   
                    @SENHA
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBFuncionario]	
		        SET
			        [NOME] = @NOME,
                    [LOGIN] = @LOGIN,
			        [SENHA] = @SENHA
                WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBFuncionario]
		        WHERE
			        [ID] = @ID";
        private const string sqlSelecionarPorNumero =
           @"SELECT
	                [ID],
                    [NOME],
                    [LOGIN],
                    [SENHA]
              FROM 
	                [TBFuncionario]
              WHERE 
	                [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT
	                [ID],
                    [NOME],
                    [LOGIN],
                    [SENHA]
              FROM 
	                [TBFuncionario]";

        #endregion

        public void Inserir(Funcionario funcionario)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarPaciente(funcionario, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            funcionario.Numero = Convert.ToInt32(id);
            sqlConnection.Close();
        }
        public void Editar(Funcionario funcionario)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarPaciente(funcionario, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        public void Excluir(Funcionario funcionario)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", funcionario.Numero);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<Funcionario> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Funcionario> funcionarios = new List<Funcionario>();

            while (sqlDataReader.Read())
            {
                Funcionario funcionario = ConverterFuncionario(sqlDataReader);

                funcionarios.Add(funcionario);
            }
            return funcionarios;
        }
        public Funcionario SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorNumero, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Funcionario funcionario = null;

            if (sqlDataReader.Read())
                funcionario = ConverterFuncionario(sqlDataReader);

            sqlConnection.Close();

            return funcionario;
        }
        public static Funcionario ConverterFuncionario(SqlDataReader leitorPaciente)
        {
            int numero = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = Convert.ToString(leitorPaciente["NOME"]);
            string login = Convert.ToString(leitorPaciente["LOGIN"]);
            string senha = Convert.ToString(leitorPaciente["SENHA"]);

            var funcionario = new Funcionario(nome, login, senha)
            {
                Numero = numero
            };

            return funcionario;
        }
        public static void ConfigurarPaciente
            (Funcionario funcionario, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", funcionario.Numero);
            sqlCommand.Parameters.AddWithValue("NOME", funcionario.Nome);
            sqlCommand.Parameters.AddWithValue("LOGIN", funcionario.Login);
            sqlCommand.Parameters.AddWithValue("SENHA", funcionario.Senha);
        }
    }
}
