using ControleMedicamento.Dominio.Compartilhado;

using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamento.Infra.BancoDados.Compartilhado;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        RepositorioFornecedorEmBancoDeDados repo = new RepositorioFornecedorEmBancoDeDados();        

        ValidadorFornecedor val = new ValidadorFornecedor();

        Fornecedor for1;
        Fornecedor for2;
        Fornecedor for3;
        Fornecedor for4;
        Fornecedor for5;

        public RepositorioFornecedorEmBancoDadosTest()
        {
            db.ComandoSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            for1 = new Fornecedor(default, "34141", "asdad@gmail.com", "LAGES", "SC");
            for2 = new Fornecedor("Carlos", "54 9 9578 - 4428", null, "LAGES", "SC");
            for3 = new Fornecedor("Paulo", "22131", "casdas@gmail.com", "LAGES", "SC");
            for4 = new Fornecedor("Roberto", "999547758", "gmail.com", "LAGES", "SC");
            for5 = new Fornecedor("João", "+55 (49) 9 5574-2217", "joao_jao@hotmail.com", "LAGES", "SC");

            for1.Numero = 1;
            for2.Numero = 1;
            for3.Numero = 1;
            for4.Numero = 1;
            for5.Numero = 1;

        }
        [TestMethod]
        public void Inserir()
        {
            ValidationResult val1 = val.Validate(for1);
            ValidationResult val2 = val.Validate(for2);
            ValidationResult val3 = val.Validate(for3);
            ValidationResult val4 = val.Validate(for4);
            ValidationResult val5 = val.Validate(for5);

            ValidationResult val11 = repo.Inserir(for1);
            ValidationResult val22 = repo.Inserir(for2);
            ValidationResult val33 = repo.Inserir(for3);
            ValidationResult val44 = repo.Inserir(for4);
            ValidationResult val55 = repo.Inserir(for5);           
            
            
            Assert.AreEqual(FluentValidationExtension.Equals(val1, val11), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val22, val2), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val3, val33), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val44, val44), true);
            Assert.AreEqual(FluentValidationExtension.Equals(val5, val55), true);

            Fornecedor fornecedor1 = repo.SelecionarPorNumero(for1.Numero);
            Fornecedor fornecedor2 = repo.SelecionarPorNumero(for2.Numero);
            Fornecedor fornecedor3 = repo.SelecionarPorNumero(for3.Numero);
            Fornecedor fornecedor4 = repo.SelecionarPorNumero(for4.Numero);
            Fornecedor fornecedor5 = repo.SelecionarPorNumero(for5.Numero);

            Assert.AreEqual(fornecedor1.Equals(for1), false);
            Assert.AreEqual(fornecedor2.Equals(for2), false);
            Assert.AreEqual(fornecedor3.Equals(for3), false);
            Assert.AreEqual(fornecedor4.Equals(for4), false);
            Assert.AreEqual(fornecedor5.Equals(for5), true);
        }
        [TestMethod]
        public void Editar()
        {
            repo.Inserir(for5);
            
            Fornecedor forn5 = new Fornecedor("João", "999547758", "joao_jao@hotmail.com", "LAGES", "SC");
            forn5.Numero = 1;

            ValidationResult val1 = val.Validate(forn5);            

            Assert.AreEqual(FluentValidationExtension.Equals(repo.Editar(forn5), val1), true);

            Fornecedor forn55 = new Fornecedor("João", null, "joao_jao@hotmail.com", "LAGES", "SC");
            ValidationResult val2 = new ValidationResult();
            val2 = val.Validate(forn55);
            Assert.AreEqual(FluentValidationExtension.Equals(repo.Editar(forn55), val2), true);           

            Fornecedor forne5 = repo.SelecionarPorNumero(forn5.Numero);

            Assert.AreEqual(forn55.Equals(forne5), false);
            
        }
        [TestMethod]
        public void Excluir()
        {
            repo.Inserir(for5);
            Fornecedor forn5 = new Fornecedor("João", "999547758", "joao_jao@hotmail.com", "LAGES", "SC");
            repo.Inserir(forn5);

            int quantidadeExcluidos = repo.Excluir(for5);

            Assert.AreEqual(quantidadeExcluidos, 1);

            List<Fornecedor> fornecedores = repo.SelecionarTodos();

            Assert.AreEqual(fornecedores.Count, 1);

            List<Fornecedor> _fornecedores = new List<Fornecedor>()
            {
                forn5
            };

            Assert.AreEqual(_fornecedores.Count, fornecedores.Count);


            for (int i = 0; i < fornecedores.Count; i++)
            {
                Assert.AreEqual(fornecedores[i].Equals(_fornecedores[i]), true);
            }
        }
        [TestMethod]
        public void SelecionarTodos()
        {
            repo.Inserir(for5);

            List<Fornecedor> forne = new List<Fornecedor>()
            {
                for5
            };
            List<Fornecedor> fornecedor = repo.SelecionarTodos();

            for (int i = 0; i < fornecedor.Count; i++)
            {
                Assert.AreEqual(forne[i].Equals(fornecedor[i]), true);
            }

        }
        [TestMethod]
        public void SelecionarPorNumero()
        {
            repo.Inserir(for5);

            repo.SelecionarPorNumero(1);

            Fornecedor forne = repo.SelecionarPorNumero(for5.Numero);
            Assert.AreEqual(for5, forne);
        }
    }
}
