using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System.Linq;
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
                    [NOME],
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNECEDOR_ID]  
	            )
	            VALUES
                (
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
                    [NOME] = @NOME,
                    [DESCRICAO] = @DESCRICAO,
                    [LOTE] = @LOTE,
                    [VALIDADE] = @VALIDADE,
                    [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                    [FORNECEDOR_ID] = @FORNECEDOR_ID  
                WHERE
			        [ID] = @ID";


        private const string sqlExcluir =
           @"UPDATE FROM [TBMedicamento]
                SET
                    [QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
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
                
                F.[NOME] AS FUNCIONARIO_NOME,
                F.[LOGIN],
                F.[SENHA],

                P.[NOME] AS PACIENTE_NOME,
                P.[CARTAOSUS],

                R.[QUANTIDADEMEDICAMENTO],
                R.[DATA]
            FROM
                TBRequisicao AS R INNER JOIN 
                TBPaciente AS P ON R.[PACIENTE_ID] = P.[ID]
                INNER JOIN TBFuncionario ON
                R.[FUNCIONARIO_ID] = F.[ID]";

        private const string sqlSelecionarRequisicaoPorNumero =
        @"SELECT                
                R.[ID],
                R.[FUNCIONARIO_ID],
                
                R.[PACIENTE_ID],
               
                F.[NOME] AS FUNCIONARIO_NOME,
                F.[LOGIN],
                F.[SENHA],

                P.[NOME] AS PACIENTE_NOME,
                P.[CARTAOSUS],

                R.[QUANTIDADEMEDICAMENTO],
                R.[DATA]
            FROM
                TBRequisicao AS R INNER JOIN 
                TBPaciente AS P ON R.[PACIENTE_ID] = P.[ID]
                INNER JOIN TBFuncionario ON
                R.[FUNCIONARIO_ID] = F.[ID]                                             
            WHERE
                R.[MEDICAMENTO_ID] = @MED_ID";
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
            sqlCommand.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", 0);
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
            List<Requisicao> todasRequisicoes = new List<Requisicao>();

            SqlCommand sqlCommandRequisicao = new SqlCommand
            (sqlSelecionarRequisicaoTodos, sqlConnection);

            SqlDataReader sqlDataReaderRequisicao = sqlCommandRequisicao.ExecuteReader();

            while (sqlDataReaderRequisicao.Read())
                todasRequisicoes.Add(ConverterRequisicao(sqlDataReaderRequisicao));
            todasRequisicoes.OrderBy(x => x.Medicamento.Numero);

            int i = 0;
            while (sqlDataReader.Read())
            {
                Medicamento medicamento = ConverterMedicamento(sqlDataReader);

                List<Requisicao> requisicoes = new List<Requisicao>();

                //Requisicao requisicao = null;
                
                while (todasRequisicoes[i].Numero == medicamento.Numero)
                {
                    todasRequisicoes[i].Medicamento = medicamento;
                    requisicoes.Add(todasRequisicoes[i]);
                    i++;
                }                               

                medicamento.Requisicoes = requisicoes;
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
                    //Requisicao requisicao = null;
                    Requisicao requisicao = ConverterRequisicao(sqlDataReaderRequisicao);
                    requisicao.Medicamento = medicamento;
                    requisicoes.Add(requisicao);
                }
                medicamento.Requisicoes = requisicoes;
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
            string fornecedorNome = Convert.ToString(leitorMedicamento["FORNECEDOR_NOME"]);
            string fornecedorTelefone = Convert.ToString(leitorMedicamento["TELEFONE"]);
            string fornecedorEmail = Convert.ToString(leitorMedicamento["EMAIL"]);
            string fornecedorCidade = Convert.ToString(leitorMedicamento["CIDADE"]);
            string fornecedorEstado = Convert.ToString(leitorMedicamento["ESTADO"]);

            var medicamento = new Medicamento
                (nome, descricao, lote, validade)
            {
                Numero = numero,                
                QuantidadeDisponivel = quantidadeDisponivel,
                Fornecedor = new Fornecedor
                (fornecedorNome, fornecedorTelefone, fornecedorEmail, 
                fornecedorCidade, fornecedorEstado)
                {
                    Numero = fornecedorId
                }
            };

            return medicamento;
        }
        public static Requisicao ConverterRequisicao(SqlDataReader leitorRequisicao)
        {
            int numero = Convert.ToInt32(leitorRequisicao["ID"]);

            int funcionarioID = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);

            string funcionarioNome = Convert.ToString(leitorRequisicao["FUNCIONARIO_NOME"]);
            string funcionarioLogin = Convert.ToString(leitorRequisicao["LOGIN"]);
            string funcionarioSenha = Convert.ToString(leitorRequisicao["SENHA"]);

            int pacienteID = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            string pacienteNome = Convert.ToString(leitorRequisicao["PACIENTE_NOME"]);
            string pacienteSUS = Convert.ToString(leitorRequisicao["PACIENTE_SUS"]);
            
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);
            int quantidadeMedicamento = Convert.ToInt32
                (leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            var requisicao = new Requisicao
            {
                Numero = numero,
                Funcionario = new Funcionario
                            (funcionarioNome, funcionarioLogin, funcionarioSenha)
                {
                    Numero = funcionarioID
                },

                Paciente = new Paciente(pacienteNome, pacienteSUS)
                {
                    Numero = pacienteID
                },
                QtdMedicamento = quantidadeMedicamento,
                Data = data                
            };

            return requisicao;
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
        public static void ConfigurarRequisicao
        (Requisicao requisicao, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", requisicao.Numero);            
            sqlCommand.Parameters.AddWithValue("FUNCIONARIO_ID", requisicao.Funcionario.Numero);
            sqlCommand.Parameters.AddWithValue("PACIENTE_ID", requisicao.Paciente.Numero);
            sqlCommand.Parameters.AddWithValue("MEDICAMENTO_ID", requisicao.Medicamento.Numero);
            sqlCommand.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", requisicao.QtdMedicamento);
            sqlCommand.Parameters.AddWithValue("DATA", requisicao.Data);
        }
    }
}   
