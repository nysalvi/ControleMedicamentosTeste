using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class Medicamento : EntidadeBase<Medicamento>
    {        
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public int QuantidadeDisponivel { get; set; }

        public List<Requisicao> Requisicoes { get; set; }

        public Fornecedor Fornecedor{ get; set; }

        public int QuantidadeRequisicoes { get { return Requisicoes.Count; } }

        public Medicamento(string nome, string descricao, string lote, DateTime validade)
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            Validade = validade;
            Requisicoes = new List<Requisicao>();
        }
        public override string ToString()
        {
            string requString = "";
            Requisicoes.ForEach(x => requString +=x.ToString());
            return string.Format("|Nome /{0} |Descrição /{1} |Lote /{2} |Validade /{3}" +
            "|QtdDisponivel /{4} \\Requisições /{5} \\Fornecedor /{6} |QtdRequisições /{7}",
            
            Nome, Descricao, Lote, Validade, QuantidadeDisponivel, requString, 
                    Fornecedor, QuantidadeRequisicoes);
        }
    }
}
