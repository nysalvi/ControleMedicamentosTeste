using ControleMedicamentos.Dominio.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloFuncionario
{
    [TestClass]
    public class FuncionarioTest
    {
        [TestMethod]
        public void ToSTRING()
        {
            Funcionario func1 = new Funcionario("Roberto", "rob", "rbo21");
            Funcionario func2 = new Funcionario("Carlos", "carloc", "carlos.c32");
            Funcionario func3 = new Funcionario("Paulo", "paul.cs", "dcj3fh");

            string func1String = "|Nome /Roberto |Login /rob |Senha /rbo21";
            string func2String = "|Nome /Carlos |Login /carloc |Senha /carlos.c32";
            string func3String = "|Nome /Paulo |Login /paul.cs |Senha /dcj3fh";

            Assert.AreEqual(func1.ToString(), func1String);
            Assert.AreEqual(func2.ToString(), func2String);
            Assert.AreEqual(func3.ToString(), func3String);            
        }
        [TestMethod]
        public void ObjetoEqual()
        {
            Funcionario func1 = new Funcionario("Roberto", "rob", "rbo21");
            Funcionario func2 = new Funcionario("Carlos", "carloc", "carlos.c32");
            Funcionario func3 = new Funcionario("Paulo", "paul.cs", "dcj3fh");

            Funcionario func11 = func1;
            Funcionario func22 = func2;
            Funcionario func33 = func2;

            Assert.AreEqual(func1.Equals(func22), false);
            Assert.AreEqual(func1.Equals(null), false);
            Assert.AreEqual(func22.Equals(func1), false);
            Assert.AreEqual(func33.Equals(func22), true);
            Assert.AreEqual(func22.Equals(func33), true);

            Funcionario func111 = new Funcionario(func11.Nome, func11.Login, func11.Senha);            
            Funcionario func222 = new Funcionario(func22.Nome, func22.Login, func22.Senha);
            func111.Senha = "dasdas";

            Assert.AreEqual(func111.Equals(func11), false);
            Assert.AreEqual(func222.Equals(func22), true);
        }
    }
}
