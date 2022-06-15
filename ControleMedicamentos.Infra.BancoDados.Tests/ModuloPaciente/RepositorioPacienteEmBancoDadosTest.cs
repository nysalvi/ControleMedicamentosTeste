using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        [TestMethod]
        public void Inserir()
        {
            RepositorioPacienteEmBancoDeDados repo = new RepositorioPacienteEmBancoDeDados();

            Paciente pac1 = new Paciente(default, "34141");
            Paciente pac2 = new Paciente("Carlos", null);
            Paciente pac3 = new Paciente("Paulo", "");
            
            Assert.AreEqual(repo.Inserir(pac1), "Campo 'Nome' Não pode ser vazio");
            Assert.AreEqual(repo.Inserir(pac2), "Campo 'CartaoSUS' Não pode ser null");
            Assert.AreEqual(repo.Inserir(pac3), "Campo 'CartaoSUS' Não pode ser vazio");
        }
        [TestMethod]
        public void Editar()
        {

        }
        [TestMethod]
        public void Exluir()
        {

        }
        [TestMethod]
        public void SelecionarTodos()
        {

        }
        [TestMethod]
        public void SelecionarPorNumero()
        {

        }
    }
}
