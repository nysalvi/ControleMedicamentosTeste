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
        [DataRow(new Funcionario("Roberto", "rob", "rbo21"))]
        [DataRow()]
        [DataRow()]
        public void Instancias(Funcionario funcionario, String stringHash)
        {
            Funcionario func1 = new Funcionario("Roberto", "rob", "rbo21");
            Funcionario func2 = new Funcionario("Carlos", "carloc", "carlos.c32");
            Funcionario func3 = new Funcionario("Paulo", "paul.cs", "dcj3fh");
        }
    }
}
