using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using ControleMedicamento.Dominio.Compartilhado;
using ControleMedicamento.Infra.BancoDados.Compartilhado;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        RepositorioPacienteEmBancoDeDados repo = new RepositorioPacienteEmBancoDeDados();

        ValidadorPaciente val = new ValidadorPaciente();

        Paciente pac1;
        Paciente pac2;
        Paciente pac3;
        Paciente pac4;
        Paciente pac5;

        public RepositorioPacienteEmBancoDadosTest()
        {
            db.ComandoSql("DELETE FROM TBPaciente; DBCC CHECKIDENT (TBPaciente, RESEED, 0)");

            pac1 = new Paciente(default,"SC");
            pac2 = new Paciente("Carlos", null);
            pac3 = new Paciente("Paulo", "221415");
            pac4 = new Paciente("Roberto", "22131");
            pac5 = new Paciente("João", "3345");

        }
        [TestMethod]
        public void Inserir()
        {
            ValidationResult val1 = val.Validate(pac1);
            ValidationResult val2 = val.Validate(pac2);
            ValidationResult val3 = val.Validate(pac3);
            ValidationResult val4 = val.Validate(pac4);
            ValidationResult val5 = val.Validate(pac5);

            ValidationResult val11 = repo.Inserir(pac1);
            ValidationResult val22 = repo.Inserir(pac2);
            ValidationResult val33 = repo.Inserir(pac3);
            ValidationResult val44 = repo.Inserir(pac4);
            ValidationResult val55 = repo.Inserir(pac5);


            Assert.AreEqual(FluentValidationExtension.Equals(val1, val11), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val2, val22), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val3, val33), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val4, val44), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val5, val55), true);

            Paciente paciente1 = repo.SelecionarPorNumero(pac1.Numero);
            Paciente paciente2 = repo.SelecionarPorNumero(pac2.Numero);
            Paciente paciente3 = repo.SelecionarPorNumero(pac3.Numero);
            Paciente paciente4 = repo.SelecionarPorNumero(pac4.Numero);
            Paciente paciente5 = repo.SelecionarPorNumero(pac5.Numero);

            Assert.AreEqual(Paciente.Equals(paciente1, pac1), false);
            Assert.AreEqual(Paciente.Equals(paciente2, pac2), false);
            Assert.AreEqual(Paciente.Equals(paciente3, pac3), true);
            Assert.AreEqual(Paciente.Equals(paciente4, pac4), true);
            Assert.AreEqual(Paciente.Equals(paciente5, pac5), true);
        }
        [TestMethod]
        public void Editar()
        {
            repo.Inserir(pac3);
            repo.Inserir(pac4);
            repo.Inserir(pac5);

            Paciente pac55 = new Paciente("João", "425")
            {
                Numero = pac5.Numero
            };

            repo.Editar(pac55);
            Paciente paciente55 = repo.SelecionarPorNumero(pac55.Numero);

            Assert.AreEqual(Paciente.Equals(pac55, paciente55), true);

            Paciente pac555 = new Paciente("João", null)
            {
                Numero = pac5.Numero
            };            
            repo.Editar(pac555);
            Paciente paciente555 = repo.SelecionarPorNumero(pac555.Numero);

            Assert.AreEqual(Paciente.Equals(pac555, paciente555), false);
        }
        [TestMethod]
        public void Excluir()
        {
            repo.Inserir(pac3);
            repo.Inserir(pac4);
            repo.Inserir(pac5);

            int quantidadeExcluidos = repo.Excluir(pac5);

            Assert.AreEqual(quantidadeExcluidos, 1);

            List<Paciente> pacientes = repo.SelecionarTodos();

            List<Paciente> _pacientes = new List<Paciente>()
            {
                pac3, pac4
            };

            Assert.AreEqual(_pacientes.Count, pacientes.Count);

            for (int i = 0; i < pacientes.Count; i++)
            {
                Assert.AreEqual(pacientes[i].Equals(_pacientes[i]), true);
            }
        }
        [TestMethod]
        public void SelecionarTodos()
        {
            repo.Inserir(pac3);
            repo.Inserir(pac4);
            repo.Inserir(pac5);

            List<Paciente> pacientes = new List<Paciente>()
            {
                pac3, pac4, pac5
            };
            List<Paciente> _pacientes = repo.SelecionarTodos();

            for (int i = 0; i < _pacientes.Count; i++)
            {
                Assert.AreEqual(pacientes[i].Equals(_pacientes[i]), true);
            }

        }
        [TestMethod]
        public void SelecionarPorNumero()
        {
            repo.Inserir(pac3);
            repo.Inserir(pac4);
            repo.Inserir(pac5);
            
            Paciente paciente3 = repo.SelecionarPorNumero(pac3.Numero);
            Assert.AreEqual(pac3.Equals(paciente3), true);

            Paciente paciente4 = repo.SelecionarPorNumero(pac4.Numero);
            Assert.AreEqual(pac4.Equals(paciente4), true);

            Paciente paciente5 = repo.SelecionarPorNumero(pac5.Numero);
            Assert.AreEqual(pac5.Equals(paciente5), true);

        }
    }
}
