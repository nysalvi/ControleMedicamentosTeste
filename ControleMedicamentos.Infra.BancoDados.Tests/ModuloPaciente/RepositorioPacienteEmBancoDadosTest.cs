using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation.Results;
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
        RepositorioPacienteEmBancoDeDados repo = new RepositorioPacienteEmBancoDeDados();
        ValidadorPaciente val = new ValidadorPaciente();

        [TestMethod]
        public void Inserir()
        {
            RepositorioPacienteEmBancoDeDados repo = new RepositorioPacienteEmBancoDeDados();

            Paciente pac1 = new Paciente(default, "34141");
            Paciente pac2 = new Paciente("Carlos", null);
            Paciente pac3 = new Paciente("Paulo", "");

            ValidationResult val1 = val.Validate(pac1);
            ValidationResult val2 = val.Validate(pac2);
            ValidationResult val3 = val.Validate(pac3);

            //Assert.AreEqual(repo.Inserir(pac1), val1);
            //Assert.AreEqual(repo.Inserir(pac2), val2);
            //Assert.AreEqual(repo.Inserir(pac3), val3);

            Assert.AreEqual(repo.Inserir(pac1).IsValid, val1.IsValid);
            Assert.AreEqual(repo.Inserir(pac2).IsValid, val2.IsValid);
            Assert.AreEqual(repo.Inserir(pac3).IsValid, val3.IsValid);

        }
        [TestMethod]
        public void Editar()
        {
            Paciente paciente = new Paciente("Roberto", "221")
            {
                Numero = 1
            };

            repo.Editar(paciente);

            Paciente pac = repo.SelecionarPorNumero(1);

            Assert.AreEqual(paciente.ToString(), pac.ToString());

        }
        [TestMethod]
        public void Exluir()
        {
            Paciente paciente = new Paciente("Roberto", "3443")
            {
                Numero = 1
            };

            repo.Inserir(paciente);

            Assert.AreEqual(repo.Excluir(paciente), 1);
        }
        [TestMethod]
        public void SelecionarTodos()
        {
            List<Paciente> pac = new List<Paciente>()
            {
                new Paciente("Carlos", "21313")
                {
                    Numero = 4
                },
                new Paciente("Jefferson", "1120")
                {
                    Numero = 5
                },
                new Paciente("nome", "cartao")
                {
                    Numero = 6
                }
            };
            List<Paciente> pacientes = repo.SelecionarTodos();

            for (int i = 0; i < pacientes.Count; i++)
            {
                Assert.AreEqual(pac[i].ToString(), pacientes[i].ToString());
            }
            
            //Assert.AreEqual(pac, pacientes);
            //Assert.AreEqual(pac.Equals(pacientes), true);
        }
        [TestMethod]
        public void SelecionarPorNumero()
        {
            Paciente pac = new Paciente("nome", "cartao");
            pac.Numero = 6;
            //repo.Inserir(pac);
            Paciente paciente = repo.SelecionarPorNumero(pac.Numero);
            Assert.AreEqual(pac, paciente);
        }
    }
}
