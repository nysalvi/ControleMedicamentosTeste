using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDeDados
    {
        private static readonly string databaseConnection =
            "(localdb)\\MSSQLLocalDB;Initial Catalog=ControleMedicamentos;" +
            "Integrated Security=True;" +
            "Pooling=False";
        #region SQL Queries
        private const string sqlInserir =
           @"INSERT INTO [TBRequisicao] 
                (
                    [FUNCIONARIO_ID],
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID],
                    [QUANTIDADEMEDICAMENTO],
                    [DATA]
	            )
	            VALUES
                (
                    @FUNCIONARIO_ID,   
                    @PACIENTE_ID,
                    @MEDICAMENTO_ID,
                    @QUANTIDADEMEDICAMENTO,
                    @DATA
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBRequisicao]	
		        SET
			        [FUNCIONARIO_ID] = @FUNCIONARIO_ID,
                    [PACIENTE_ID] = @PACIENTE_ID,
                    [MEDICAMENTO_ID] = @MEDICAMENTO_ID,
                    [QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
			        [DATA] = @DATA
                WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBRequisicao]                                
		        WHERE
			        [ID] = @ID;";
        private const string sqlSelecionarPorNumero =
           @"SELECT
	                [ID],
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO],

                    [P.ID] AS PACIENTE_ID
                    [P.NOME] AS PACIENTE_NOME,
                    [P.CARTAOSUS] AS P_CARTAOSUS,
                    
                    [F.ID] AS FUNCIONARIO_ID
                    [F.NOME] AS FUNCIONARIO_NOME,
                    [F.LOGIN] AS FUNCIONARIO_LOGIN,
                    [F.SENHA] AS FUNCIONARIO_SENHA,

                    [M.NOME] AS MEDICAMENTO_NOME,
                    [M.DESCRICAO] AS MEDICAMENTO_DESCRICAO,
                    [M.LOTE] AS MEDICAMENTO_LOTE,
                    [M.VALIDADE] AS MEDICAMENTO_VALIDADE,
                                        
              FROM 
	                TBRequisicao AS R INNER JOIN
                    TBPaciente AS P ON 
                    R.[PACIENTE_ID] = P.[ID] INNER JOIN
                    TBFuncionario AS F ON R.[FUNCIONARIO_ID] = F.[ID]
                    INNER JOIN TBMedicamento AS M ON
                    R.[ID] = M.[ID]
    
              WHERE 
	                [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT
	                [ID],
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO],

                    [P.NOME] AS PACIENTE_NOME,
                    [P.CARTAOSUS] AS PACIENTE_CARTAOSUS,

                    [F.NOME] AS FUNCIONARIO_NOME,
                    [F.LOGIN] AS FUNCIONARIO_LOGIN,
                    [F.SENHA] AS FUNCIONARIO_SENHA,
                    
                    [M.ID] AS MEDICAMENTO_ID,
                    [M.NOME] AS MEDICAMENTO.NOME,
                    [M.DESCRICAO] AS MEDICAMENTO.DESCRICAO,
                    [M.LOTE] AS MEDICAMENTO.LOTE,
                    [M.VALIDADE] AS MEDICAMENTO.VALIDADE,

              FROM 
	                TBRequisicao AS R INNER JOIN
                    TBPaciente AS P ON 
                    R.[PACIENTE_ID] = P.[ID] INNER JOIN
                    TBFuncionario AS F ON R.[FUNCIONARIO_ID] = F.[ID]
                    INNER JOIN TBMedicamento AS M ON
                    R.[ID] = M.[ID]
    
              WHERE 
	                [ID] = @ID";
        #endregion
        public void Inserir(Requisicao requisicao)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarRequisicao(requisicao, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            requisicao.Numero = Convert.ToInt32(id);
            sqlConnection.Close();
        }
        public void Editar(Requisicao requisicao)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarRequisicao(requisicao, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        public void Excluir(Requisicao requisicao)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", requisicao.Numero);
            sqlCommand.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", 0);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (sqlDataReader.Read())
            {
                Requisicao requisicao = ConverterRequisicao(sqlDataReader);

                requisicoes.Add(requisicao);
            }
            return requisicoes;
        }
        public Requisicao SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(databaseConnection);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorNumero, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Requisicao requisicao = null;

            if (sqlDataReader.Read())
                requisicao = ConverterRequisicao(sqlDataReader);

            sqlConnection.Close();

            return requisicao;
        }

        public static Requisicao ConverterRequisicao(SqlDataReader leitorRequisicao)
        {
            int numero = Convert.ToInt32(leitorRequisicao["ID"]);

            int funcionarioId = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);
            string funcionarioNome = Convert.ToString(leitorRequisicao["FUNCIONARIO_NOME"]);
            string funcionarioLogin = Convert.ToString(leitorRequisicao["FUNCIONARIO_LOGIN"]);
            string funcionarioSenha = Convert.ToString(leitorRequisicao["FUNCIONARIO_SENHA"]);

            Funcionario funcionario = new Funcionario
                (funcionarioNome, funcionarioLogin, funcionarioSenha)
            {
                Numero = funcionarioId
            };

            int pacienteId = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            string pacienteNome = Convert.ToString(leitorRequisicao["PACIENTE_NOME"]);
            string pacienteCARTAOSUS = Convert.ToString(leitorRequisicao["PACIENTE_CARTAOSUS"]);

            Paciente paciente = new Paciente(pacienteNome, pacienteCARTAOSUS)
            {
                Numero = pacienteId
            };

            int medicamentoId = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_ID"]);
            string medicamentoNome = Convert.ToString(leitorRequisicao["MEDICAMENTO_NOME"]);
            string medicamentoDescricao = Convert.ToString(leitorRequisicao["MEDICAMENTO_DESCRICAO"]);
            string medicamentoLote = Convert.ToString(leitorRequisicao["MEDICAMENTO_LOTE"]);
            DateTime medicamentoValidade = Convert.ToDateTime(leitorRequisicao["MEDICAMENTO_VALIDADE"]);
            int medicamentoQtdDisponivel = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_QUANTIDADEDISPONIVEL"]);
            int medicamentoFornecedorId = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_FORNECEDOR_ID"]);
            string medicamentoFornecedorNome = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_NOME"]);
            string medicamentoFornecedorTelefone = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_TELEFONE"]);
            string medicamentoFornecedorEmail = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_EMAIL"]);
            string medicamentoFornecedorCidade = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_CIDADE"]);
            string medicamentoFornecedorEstado = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_ESTADO"]);

            Fornecedor fornecedor = new Fornecedor
                (medicamentoFornecedorNome, medicamentoFornecedorTelefone,
                medicamentoFornecedorEmail, medicamentoFornecedorCidade,
                medicamentoFornecedorEstado)
            {
                Numero = medicamentoFornecedorId
            };

            List<Requisicao> requisicoes = new List<Requisicao>();

            Medicamento medicamento = new Medicamento
                (medicamentoNome, medicamentoDescricao, medicamentoLote, medicamentoValidade)
            {
                Numero = medicamentoId,
                QuantidadeDisponivel = medicamentoQtdDisponivel,
                Fornecedor = fornecedor,
                Requisicoes = requisicoes
            };

            int qntMedicamento = Convert.ToInt32(leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            var requisicao = new Requisicao
            {
                Numero = numero,
                Medicamento = medicamento,
                Paciente = paciente,
                QtdMedicamento = qntMedicamento,
                Data = data,
                Funcionario = funcionario
            };

            return requisicao;
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
