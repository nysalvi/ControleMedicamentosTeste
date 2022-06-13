using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
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
                    [F.NOME] AS FORNECEDOR_NOME,
                    [F.TELEFONE],
                    [F.EMAIL], 
                    [F.CIDADE],
                    [F.ESTADO]
              FROM 
	                TBMedicamento AS M INNER JOIN TBFornecedor AS F
                    ON M.[FORNECEDOR_ID] = F.[ID]
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
                    [FORNECEDOR_ID],
                    [F.NOME] AS FORNECEDOR_NOME,
                    [F.TELEFONE],
                    [F.EMAIL], 
                    [F.CIDADE],
                    [F.ESTADO]
              FROM 
	                [TBMedicamento] AS M INNER JOIN ON 
                    [TBFORNECEDOR] AS F ON M.[FORNECEDOR_ID] = F.[ID]";

        private const string sqlSelecionarRequisicaoTodos =
            @"SELECT                
                R.[ID],
                R.[FUNCIONARIO_ID],
                
                R.[PACIENTE_ID],
               
                F.[NOME],
                F.[LOGIN],
                F.[SENHA],

                P.[NOME],
                P.[CARTAOSUS],

                R.[QUANTIDADEMEDICAMENTO],
                R.[DATA]
            FROM
                TBRequisicao AS R INNER JOIN 
                TBPaciente AS P ON R.[ID] = P.[ID]
                INNER JOIN TBFuncionario ON
                R.[FUNCIONARIO_ID] = F.[ID]";

        private const string sqlSelecionarRequisicaoPorNumero =
        @"SELECT                
                R.[ID],
                R.[FUNCIONARIO_ID],
                
                R.[PACIENTE_ID],
               
                F.[NOME],
                F.[LOGIN],
                F.[SENHA],

                P.[NOME],
                P.[CARTAOSUS],

                R.[QUANTIDADEMEDICAMENTO],
                R.[DATA]
            FROM
                TBRequisicao AS R INNER JOIN 
                TBPaciente AS P ON R.[ID] = P.[ID]
                INNER JOIN TBFuncionario ON
                R.[FUNCIONARIO_ID] = F.[ID]                                             
            WHERE
                R.[MEDICAMENTO_ID] = [MED_ID]";
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
            {
                medicamento = ConverterMedicamento(sqlDataReader);
                SqlCommand sqlCommandRequisicao = new SqlCommand
                    (sqlSelecionarRequisicaoPorNumero, sqlConnection);
                sqlCommandRequisicao.Parameters.AddWithValue("MED_ID", medicamento.Numero);
                SqlDataReader sqlDataReaderRequisicao = sqlCommandRequisicao.ExecuteReader();
                List<Requisicao> requisicoes = new List<Requisicao>();
                while (sqlDataReaderRequisicao.Read())
                {

                }
            }
            


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

            var medicamento = new Medicamento
                (nome, descricao, lote, validade)
            {
                Numero = numero,                
                QuantidadeDisponivel = quantidadeDisponivel
            };

            return medicamento;
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
