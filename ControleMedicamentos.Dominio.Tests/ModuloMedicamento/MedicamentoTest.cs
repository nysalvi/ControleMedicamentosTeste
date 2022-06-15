using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using System;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class MedicamentoTest
    {
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
