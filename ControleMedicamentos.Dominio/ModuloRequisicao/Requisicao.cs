using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {   

        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public ModuloFuncionario.Funcionario Funcionario { get; set; }

        public override string ToString()
        {
            return string.Format
            ("\\Medicamento /{0} \\Paciente /{1} |QtdMedicamento /{2}" + "|Data /{3} \\Funcionario /{4}"
                , Medicamento, Paciente, QtdMedicamento, Data, Funcionario);
        }
    }
}
