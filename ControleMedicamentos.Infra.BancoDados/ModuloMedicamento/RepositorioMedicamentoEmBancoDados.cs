using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        private static readonly string databaseConnection =
            "(localdb)\\MSSQLLocalDB;Initial Catalog=ControleMedicamentos;" +
            "Integrated Security=True;" +
            "Pooling=False";
        #region SQL Queries
        private const string sqlInserir =
           @"INSERT INTO [TBMedicamento] 
                (
                    [ID],
                    [NOME],
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNECEDOR_ID]  
	            )
	            VALUES
                (
                    @ID,
                    @NOME,
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,
                    @FORNECEDOR_ID  
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBMedicamento]	
		        SET
			        [ID] = @ID,
                    [NOME] = @NOME,
                    [DESCRICAO] = @DESCRICAO,
                    [LOTE] = @LOTE,
                    [VALIDADE] = @VALIDADE,
                    [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                    [FORNECEDOR_ID] = @FORNECEDOR_ID  
                WHERE
			        [ID] = @ID";


        private const string sqlExcluir =
           @"DELETE FROM [TBMedicamento]
		        WHERE
			        [ID] = @ID";
        private const string sqlSelecionarPorNumero =
           @"SELECT
	                [ID],
                    [NOME],
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNECEDOR_ID]
              FROM 
	                [TBMedicamento]
              WHERE 
	                [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT
	                [ID],
                    [NOME],
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNECEDOR_ID]
              FROM 
	                [TBMedicamento]";
        #endregion

        public void Inserir(Medicamento medicamento)
        {            
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarMedicamento(medicamento, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            medicamento.Numero = Convert.ToInt32(id);
            sqlConnection.Close();
        }
        public void Editar(Medicamento medicamento)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarMedicamento(medicamento, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        public void Excluir(Medicamento medicamento)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", medicamento.Numero);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Medicamento> medicamentos = new List<Medicamento>();

            while (sqlDataReader.Read())
            {
                Medicamento medicamento = ConverterMedicamento(sqlDataReader);

                medicamentos.Add(medicamento);
            }
            return medicamentos;
        }
        public Medicamento SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorNumero, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Medicamento medicamento = null;

            if (sqlDataReader.Read())
                medicamento = ConverterMedicamento(sqlDataReader);

            sqlConnection.Close();

            return medicamento;
        }
        public static Medicamento ConverterMedicamento(SqlDataReader leitorMedicamento)
        {
            int numero = Convert.ToInt32(leitorMedicamento["ID"]);
            string nome = Convert.ToString(leitorMedicamento["NOME"]);
            string descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            string lote = Convert.ToString(leitorMedicamento["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);
            int quantidadeDisponivel = Convert.ToInt32(leitorMedicamento["QUANTIDADEDISPONIVEL"]);
            int fornecedorId = Convert.ToInt32(leitorMedicamento["FORNECEDOR_ID"]);

            var fornecedor = new Medicamento
                (nome, descricao, lote, validade)
            {
                Numero = numero,                
                QuantidadeDisponivel = quantidadeDisponivel
            };

            return fornecedor;
        }
        public static void ConfigurarMedicamento
            (Medicamento medicamento, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", medicamento.Numero);
            sqlCommand.Parameters.AddWithValue("NOME", medicamento.Nome);
            sqlCommand.Parameters.AddWithValue("DESCRICAO", medicamento.Descricao);
            sqlCommand.Parameters.AddWithValue("LOTE", medicamento.Lote);
            sqlCommand.Parameters.AddWithValue("VALIDADE", medicamento.Validade);
            sqlCommand.Parameters.AddWithValue
                ("QUANTIDADEDISPONIVEL", medicamento.QuantidadeDisponivel);
            sqlCommand.Parameters.AddWithValue
                ("FORNECEDOR_ID", medicamento.Fornecedor.Numero);
        }
    }
}
