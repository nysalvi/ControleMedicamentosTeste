using ControleMedicamento.Dominio.Compartilhado;
using ControleMedicamento.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        RepositorioFuncionarioEmBancoDeDados repo = new RepositorioFuncionarioEmBancoDeDados();

        ValidadorFuncionario val = new ValidadorFuncionario();

        Funcionario fun1;
        Funcionario fun2;
        Funcionario fun3;
        Funcionario fun4;
        Funcionario fun5;
        Funcionario fun6;
        public RepositorioFuncionarioEmBancoDadosTest()
        {
            db.ComandoSql("DELETE FROM TBFuncionario; DBCC CHECKIDENT (TBFuncionario, RESEED, 0)");

            fun1 = new Funcionario(default, "SC", "zcxza");
            fun2 = new Funcionario("Carlos", null, "@3assaf");
            fun3 = new Funcionario("Paulo", "paulo.login", "");

            fun4 = new Funcionario("Roberto", "roberto.login", "zxczxcxz");
            fun5 = new Funcionario("João", "joao.login", "Ada1471#fF3");
            fun6 = new Funcionario("Paulo", "paulo.login", "#fsdfA");
        }
        [TestMethod]
        public void Inserir()
        {
            ValidationResult val1 = val.Validate(fun1);
            ValidationResult val2 = val.Validate(fun2);
            ValidationResult val3 = val.Validate(fun3);
            ValidationResult val4 = val.Validate(fun4);
            ValidationResult val5 = val.Validate(fun5);
            ValidationResult val6 = val.Validate(fun6);

            ValidationResult val11 = repo.Inserir(fun1);
            ValidationResult val22 = repo.Inserir(fun2);
            ValidationResult val33 = repo.Inserir(fun3);
            ValidationResult val44 = repo.Inserir(fun4);
            ValidationResult val55 = repo.Inserir(fun5);
            ValidationResult val66 = repo.Inserir(fun6);

            Assert.AreEqual(FluentValidationExtension.Equals(val1, val11), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val2, val22), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val3, val33), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val4, val44), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val5, val55), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val6, val66), true);

            Funcionario Funcionario1 = repo.SelecionarPorNumero(fun1.Numero);
            Funcionario Funcionario2 = repo.SelecionarPorNumero(fun2.Numero);
            Funcionario Funcionario3 = repo.SelecionarPorNumero(fun3.Numero);
            Funcionario Funcionario4 = repo.SelecionarPorNumero(fun4.Numero);
            Funcionario Funcionario5 = repo.SelecionarPorNumero(fun5.Numero);
            Funcionario Funcionario6 = repo.SelecionarPorNumero(fun6.Numero);

            Assert.AreEqual(Funcionario.Equals(Funcionario1, fun1), false);
            Assert.AreEqual(Funcionario.Equals(Funcionario2, fun2), false);
            Assert.AreEqual(Funcionario.Equals(Funcionario3, fun3), false);
            Assert.AreEqual(Funcionario.Equals(Funcionario4, fun4), true);
            Assert.AreEqual(Funcionario.Equals(Funcionario5, fun5), true);
            Assert.AreEqual(Funcionario.Equals(Funcionario6, fun6), true);
        }
        [TestMethod]
        public void Editar()
        {
            repo.Inserir(fun4);
            repo.Inserir(fun5);
            repo.Inserir(fun6);

            Funcionario fun55 = new Funcionario("João", "joao.login", "senha")
            {
                Numero = fun5.Numero
            };

            repo.Editar(fun55);
            Funcionario Funcionario55 = repo.SelecionarPorNumero(fun55.Numero);

            Assert.AreEqual(Funcionario.Equals(fun55, Funcionario55), true);

            Funcionario fun555 = new Funcionario("João", null, "adad")
            {
                Numero = fun5.Numero
            };
            repo.Editar(fun555);
            Funcionario Funcionario555 = repo.SelecionarPorNumero(fun555.Numero);

            Assert.AreEqual(Funcionario.Equals(fun555, Funcionario555), false);
        }
        [TestMethod]
        public void Excluir()
        {
            repo.Inserir(fun4);
            repo.Inserir(fun5);
            repo.Inserir(fun6);

            int quantidadeExcluidos = repo.Excluir(fun5);

            Assert.AreEqual(quantidadeExcluidos, 1);

            List<Funcionario> Funcionarios = repo.SelecionarTodos();

            List<Funcionario> _Funcionarios = new List<Funcionario>()
            {
                fun4, fun6
            };

            Assert.AreEqual(_Funcionarios.Count, Funcionarios.Count);

            for (int i = 0; i < Funcionarios.Count; i++)
            {
                Assert.AreEqual(Funcionarios[i].Equals(_Funcionarios[i]), true);
            }
        }
        [TestMethod]
        public void SelecionarTodos()
        {
            repo.Inserir(fun4);
            repo.Inserir(fun5);
            repo.Inserir(fun6);

            List<Funcionario> Funcionarios = new List<Funcionario>()
            {
                fun4, fun5, fun6
            };
            List<Funcionario> _Funcionarios = repo.SelecionarTodos();

            for (int i = 0; i < _Funcionarios.Count; i++)
            {
                Assert.AreEqual(Funcionarios[i].Equals(_Funcionarios[i]), true);
            }

        }
        [TestMethod]
        public void SelecionarPorNumero()
        {
            repo.Inserir(fun4);
            repo.Inserir(fun5);
            repo.Inserir(fun6);

            Funcionario Funcionario4 = repo.SelecionarPorNumero(fun4.Numero);
            Assert.AreEqual(fun4.Equals(Funcionario4), true);

            Funcionario Funcionario5 = repo.SelecionarPorNumero(fun5.Numero);
            Assert.AreEqual(fun5.Equals(Funcionario5), true);

            Funcionario Funcionario6 = repo.SelecionarPorNumero(fun6.Numero);
            Assert.AreEqual(fun6.Equals(Funcionario6), true);

        }
    }
}