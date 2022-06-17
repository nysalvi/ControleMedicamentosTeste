using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using System;
using FluentValidation.Results;
using ControleMedicamento.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFornecedor;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class MedicamentoTest
    {
        ValidadorMedicamento validador = new ValidadorMedicamento();

        [TestMethod]
        public void NomeVazioNulo()
        {
            Medicamento med1 = new Medicamento("", "Para dor de Cabe�a", "512",
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento(null, "Para dor no Est�mago", "241",
                DateTime.Parse("22/06/2022"));

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);

            string[] errors = new string[]
            {
               "Campo 'Nome' n�o pode ser vazio", "Campo 'Fornecedor' n�o pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Nome' n�o pode ser nulo", "Campo 'Nome' n�o pode ser vazio",
                "Campo 'Fornecedor' n�o pode ser nulo"
            };

            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);
            
        }
        [TestMethod]
        public void DescricaoVazioNulo()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "", "512", 
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento("Eno", null, "241",
                DateTime.Parse("22/06/2022"));

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);

            string[] errors = new string[]
            {
               "Campo 'Descricao' n�o pode ser vazio", "Campo 'Fornecedor' n�o pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Descricao' n�o pode ser nulo", "Campo 'Descricao' n�o pode ser vazio",
               "Campo 'Fornecedor' n�o pode ser nulo"
            };

            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);

        }
        [TestMethod]
        public void LoteVazioNulo()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabe�a", "",
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento("Eno", "Para dor no Est�mago", null,
                DateTime.Parse("22/06/2022"));

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);

            string[] errors = new string[]
            {
               "Campo 'Lote' n�o pode ser vazio", "Campo 'Fornecedor' n�o pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Lote' n�o pode ser nulo", "Campo 'Lote' n�o pode ser vazio",
               "Campo 'Fornecedor' n�o pode ser nulo"
            };

            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);
        }
        [TestMethod]
        public void ValidadeVazioNulo()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabe�a", "512",
                new DateTime());

            Medicamento med2 = new Medicamento("Eno", "Para dor no Est�mago", "241",
                DateTime.MinValue);

            Medicamento med3 = new Medicamento("Loratadina", "Para riniter al�rgica", "334",
                DateTime.MaxValue);


            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);
            ValidationResult result3 = validador.Validate(med3);

            string[] errors = new string[]
            {
               "Campo 'Validade' n�o pode ser vazio", 
               "Campo 'Fornecedor' n�o pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Validade' n�o pode ser vazio", "Campo 'Fornecedor' n�o pode ser nulo"
            };

            string[] errors2 = new string[]
            {
               "Campo 'Fornecedor' n�o pode ser nulo"
            };
            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result3, errors2), true);            
        }
        [TestMethod]
        public void Fornecedor()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabe�a", "213",
                DateTime.Parse("14/07/2022"));
            med1.QuantidadeDisponivel = -1;
            Medicamento med2 = new Medicamento("Eno", "Para dor no Est�mago", "5546",
                DateTime.Parse("22/06/2022"));
            Medicamento med3 = new Medicamento("Eno", "Para dor no Est�mago", "514126",
                DateTime.Parse("14/08/2022"));

            Fornecedor for1 = new Fornecedor
            ("Roberto", "(49) 9 9758 - 4475", "rbo21@gmail.com", "Lages", "SC");

            Fornecedor for2 = new Fornecedor("", "(51) 9 8847 - 5514",
                "carloc@yahoo.com", "Porto Alegre", "RS");
            Fornecedor for3 = new Fornecedor("Paulo", "54 9 8547 - 1125",
                "paul.cs@hotmail.com", "Lages", "SC");

            med2.Fornecedor = for1;
            med3.Fornecedor = for2;

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);

            ValidationResult result3 = validador.Validate(med3);

            string[] errors = new string[]
            {
               "Campo 'Fornecedor' n�o pode ser nulo",
               "Campo 'QuantidadeDisponivel' n�o pode ser negativo"
               
            };

            string[] errors1 = new string[]
            {

            };
            string[] errors2 = new string[]
            {
               "Campo 'Lote' n�o pode ser nulo", "Campo 'Lote' n�o pode ser vazio",
               "Campo 'Fornecedor' n�o pode ser nulo"
            };

            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);
        }
    }
}
