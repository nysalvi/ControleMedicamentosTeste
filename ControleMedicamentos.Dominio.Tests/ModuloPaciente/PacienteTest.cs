using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloPaciente
{
    [TestClass]
    public class PacienteTest
    {
        [TestMethod]
        public void ToSTRING()
        {
            Paciente pac1 = new Paciente("Roberto", "1288489");
            Paciente pac2 = new Paciente("Carlos", "554263");
            Paciente pac3 = new Paciente("Paulo", "9996305");

            string pac1String = "|Nome /Roberto |CartaoSUS /1288489";
            string pac2String = "|Nome /Carlos |CartaoSUS /554263";
            string pac3String = "|Nome /Paulo |CartaoSUS /9996305";

            Assert.AreEqual(pac1.ToString(), pac1String);
            Assert.AreEqual(pac2.ToString(), pac2String);
            Assert.AreEqual(pac3.ToString(), pac3String);
        }
        [TestMethod]
        public void ObjetoEqual()
        {
            Paciente pac1 = new Paciente("Roberto", "1288489");
            Paciente pac2 = new Paciente("Carlos", "554263");
            Paciente pac3 = new Paciente("Paulo", "9996305");

            Paciente pac11 = pac1;
            Paciente pac22 = pac2;
            Paciente pac33 = pac2;

            Assert.AreEqual(pac1.Equals(pac22), false);
            Assert.AreEqual(pac1.Equals(null), false);
            Assert.AreEqual(pac22.Equals(pac1), false);
            Assert.AreEqual(pac33.Equals(pac22), true);
            Assert.AreEqual(pac22.Equals(pac33), true);

            Paciente pac111 = new Paciente(pac11.Nome, pac11.CartaoSUS);
            Paciente pac222 = new Paciente(pac22.Nome, pac22.CartaoSUS);
            pac111.CartaoSUS = "44236651";

            Assert.AreEqual(pac111.Equals(pac11), false);
            Assert.AreEqual(pac222.Equals(pac22), true);
        }

    }
}
