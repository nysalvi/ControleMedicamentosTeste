using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDeDados
    {
        private static readonly string databaseConnection =
        "(localdb)\\MSSQLLocalDB;Initial Catalog=ControleMedicamentos;" +
        "Integrated Security=True;" +
        "Pooling=False";

        #region SQL Queries
        private const string sqlInserir =
           @"INSERT INTO [TBPaciente] 
                (
                    [NOME],
                    [CARTAOSUS]
	            )
	            VALUES
                (
                    @NOME,
                    @CARTAOSUS
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBPaciente]	
		        SET
			        [NOME] = @NOME,
			        [CARTAOSUS] = @CARTAOSUS		        
                WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBPaciente]
		        WHERE
			        [ID] = @ID";
        private const string sqlSelecionarPorNumero =
           @"SELECT
	                [ID],
                    [NOME],
                    [CARTAOSUS]
              FROM 
	                [TBPaciente]
              WHERE 
	                [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT
	                [ID],
                    [NOME],
                    [CARTAOSUS]
              FROM 
	                [TBPaciente]";
        #endregion
        public void Inserir(Paciente paciente)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarPaciente(paciente, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            paciente.Numero = Convert.ToInt32(id);
            sqlConnection.Close();
        }
        public void Editar(Paciente paciente)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarPaciente(paciente, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        public void Excluir(Paciente paciente)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", paciente.Numero);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<Paciente> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Paciente> pacientes = new List<Paciente>();

            while (sqlDataReader.Read())
            {
                Paciente paciente = ConverterPaciente(sqlDataReader);

                pacientes.Add(paciente);
            }
            return pacientes;
        }
        public Paciente SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorNumero, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Paciente paciente = null;

            if (sqlDataReader.Read())
                paciente = ConverterPaciente(sqlDataReader);

            sqlConnection.Close();

            return paciente;
        }
        public static Paciente ConverterPaciente(SqlDataReader leitorPaciente)
        {
            int numero = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = Convert.ToString(leitorPaciente["NOME"]);
            string cartaosus = Convert.ToString(leitorPaciente["CARTAOSUS"]);

            var paciente = new Paciente(nome, cartaosus)
            {
                Numero = numero
            };

            return paciente;
        }
        public static void ConfigurarPaciente
            (Paciente paciente, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", paciente.Numero);
            sqlCommand.Parameters.AddWithValue("NOME", paciente.Nome);
            sqlCommand.Parameters.AddWithValue("CARTAOSUS", paciente.CartaoSUS);
        }
    }
}