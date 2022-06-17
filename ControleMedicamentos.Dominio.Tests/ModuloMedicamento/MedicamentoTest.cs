using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using System;
using FluentValidation.Results;
using ControleMedicamento.Dominio.Compartilhado;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class MedicamentoTest
    {
        ValidadorMedicamento validador = new ValidadorMedicamento();

        [TestMethod]
        public void NomeVazioNulo()
        {
            Medicamento med1 = new Medicamento("", "Para dor de Cabeça", "512",
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento(null, "Para dor no Estômago", "241",
                DateTime.Parse("22/06/2022"));

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);

            string[] errors = new string[]
            {
               "Campo 'Nome' não pode ser vazio", "Campo 'Fornecedor' não pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Nome' não pode ser nulo", "Campo 'Nome' não pode ser vazio",
                "Campo 'Fornecedor' não pode ser nulo"
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
               "Campo 'Descricao' não pode ser vazio", "Campo 'Fornecedor' não pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Descricao' não pode ser nulo", "Campo 'Descricao' não pode ser vazio",
               "Campo 'Fornecedor' não pode ser nulo"
            };

            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);

        }
        [TestMethod]
        public void LoteVazioNulo()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabeça", "",
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento("Eno", "Para dor no Estômago", null,
                DateTime.Parse("22/06/2022"));

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);

            string[] errors = new string[]
            {
               "Campo 'Lote' não pode ser vazio", "Campo 'Fornecedor' não pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Lote' não pode ser nulo", "Campo 'Lote' não pode ser vazio",
               "Campo 'Fornecedor' não pode ser nulo"
            };

            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);
        }
        [TestMethod]
        public void ValidadeVazioNulo()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabeça", "512",
                new DateTime());

            Medicamento med2 = new Medicamento("Eno", "Para dor no Estômago", "241",
                DateTime.MinValue);

            Medicamento med3 = new Medicamento("Loratadina", "Para riniter alérgica", "334",
                DateTime.MaxValue);


            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);
            ValidationResult result3 = validador.Validate(med3);

            string[] errors = new string[]
            {
               "Campo 'Validade' não pode ser vazio", 
               "Campo 'Fornecedor' não pode ser nulo"
            };

            string[] errors1 = new string[]
            {
               "Campo 'Validade' não pode ser vazio", "Campo 'Fornecedor' não pode ser nulo"
            };

            string[] errors2 = new string[]
            {
               "Campo 'Fornecedor' não pode ser nulo"
            };
            Assert.AreEqual(FluentValidationExtension.Equals(result, errors), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result2, errors1), true);
            Assert.AreEqual(FluentValidationExtension.Equals(result3, errors2), true);            
        }
        [TestMethod]
        public void Fornecedor()
        {            
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabeça", "512",
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento("Eno", "Para dor no Estômago", "241",
                DateTime.Parse("22/06/2022"));
            Medicamento med3 = new Medicamento("Loratadina", "Para riniter alérgica", "334",
                DateTime.Parse("12/01/2023"));

            ValidationResult result = validador.Validate(med1);
            ValidationResult result2 = validador.Validate(med2);
            ValidationResult result3 = validador.Validate(med3);

            //FluentValidationExtension.Equals();

            Assert.AreEqual(result.Errors[0].ErrorMessage, "Campo 'Validade' não pode ser vazio");
            Assert.AreEqual(result2.Errors[0].ErrorMessage, "Campo 'Validade' não pode ser vazio");
            Assert.AreEqual(result3.Errors[0].ErrorMessage, "Campo 'Validade' não pode ser vazio");

        }

        [TestMethod]
        public void ToSTRING()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabeça", "512", 
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento("Eno", "Para dor no Estômago", "241", 
                DateTime.Parse("22/06/2022"));
            Medicamento med3 = new Medicamento("Loratadina", "Para riniter alérgica", "334",
                DateTime.Parse("12/01/2023"));

            string med1String = "|Nome /Paracetamol |Descrição /Para dor de Cabeça |Lote /512 |Validade /14/07/2022 |QtdDisponivel" +
                "/ \\Requisições / \\Fornecedor |QtdRequisições /0";
            string med2String = "|Nome /Eno |Descrição /Para dor no Estômago |Lote /241 |Validade /22/06/2022 |QtdDisponivel" +
                "/ \\Requisições / \\Fornecedor |QtdRequisições /0";
            string med3String = "|Nome /Eno |Descrição /Para dor no Estômago |Lote /241 |Validade /22/06/2022 |QtdDisponivel" +
                "/ \\Requisições / \\Fornecedor |QtdRequisições /0";

            Assert.AreEqual(med1.ToString(), med1String);
            Assert.AreEqual(med2.ToString(), med2String);
            Assert.AreEqual(med3.ToString(), med3String);
        }
        [TestMethod]
        public void ObjetoEqual()
        {
            Medicamento med1 = new Medicamento("Paracetamol", "Para dor de Cabeça", "512",
                DateTime.Parse("14/07/2022"));
            Medicamento med2 = new Medicamento("Eno", "Para dor no Estômago", "241",
                DateTime.Parse("22/06/2022"));
            Medicamento med3 = new Medicamento("Loratadina", "Para riniter alérgica", "334",
                DateTime.Parse("12/01/2023"));

            Medicamento med11 = med1;
            Medicamento med22 = med2;
            Medicamento med33 = med2;

            Assert.AreEqual(med1.Equals(med22), false);
            Assert.AreEqual(med1.Equals(null), false);
            Assert.AreEqual(med22.Equals(med1), false);
            Assert.AreEqual(med33.Equals(med22), true);
            Assert.AreEqual(med22.Equals(med33), true);

            Medicamento med111 = new Medicamento(med11.Nome, med11.Descricao, med11.Lote, med11.Validade);
            Medicamento med222 = new Medicamento(med22.Nome, med22.Descricao, med22.Lote, med22.Validade);
            med111.Lote = "44236651";

            Assert.AreEqual(med111.Equals(med11), false);
            Assert.AreEqual(med222.Equals(med22), true);
        }

    }
}
