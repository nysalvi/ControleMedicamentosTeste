using ControleMedicamentos.Dominio.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Dominio.Tests.ModuloFornecedor
{
    [TestClass]    
    public class FornecedorTest
    {
        [TestMethod]
        public void ToSTRING()
        {
            Fornecedor func1 = new Fornecedor
                ("Roberto", "(49) 9 9758 - 4475", "rbo21@gmail.com", "Lages", "SC");

            Fornecedor func2 = new Fornecedor("Carlos", "(51) 9 8847 - 5514",
                "carloc@yahoo.com", "Porto Alegre", "RS");
            Fornecedor func3 = new Fornecedor("Paulo", "54 9 8547 - 1125", 
                "paul.cs@hotmail.com", "Lages", "SC");

            string fornec1String = "|Nome /Roberto |Telefone /(49) 9 9758 - 4475 " +
                "|Email /rbo21@gmail.com |Cidade /Lages |Estado /SC";

            string fornecString = "|Nome /Carlos |Telefone /(51) 9 8847 - 5514 " +
                "|Email /carloc@yahoo.com |Cidade /Porto Alegre |Estado /RS";

            string fornec3String = "|Nome /Paulo |Telefone /54 9 8547 - 1125 " +
                "|Email /paul.cs@hotmail.com |Cidade /Lages |Estado /SC";

            Assert.AreEqual(func1.ToString(), fornec1String);
            Assert.AreEqual(func2.ToString(), fornecString);
            Assert.AreEqual(func3.ToString(), fornec3String);
        }
        [TestMethod]
        public void ObjetoEqual()
        {
            Fornecedor fornec1 = new Fornecedor
                ("Roberto", "(49) 9 9758 - 4475", "rbo21@gmail.com", "Lages", "SC");

            Fornecedor fornec2 = new Fornecedor("Carlos", "(51) 9 8847 - 5514",
                "carloc@yahoo.com", "Porto Alegre", "RS");
            Fornecedor fornec3 = new Fornecedor("Paulo", "54 9 8547 - 1125",
                "paul.cs@hotmail.com", "Lages", "SC");

            Fornecedor fornec11 = fornec1;
            Fornecedor fornec22 = fornec2;
            Fornecedor fornec33 = fornec2;

            Assert.AreEqual(fornec1.Equals(fornec22), false);
            Assert.AreEqual(fornec1.Equals(null), false);
            Assert.AreEqual(fornec22.Equals(fornec1), false);
            Assert.AreEqual(fornec33.Equals(fornec22), true);
            Assert.AreEqual(fornec22.Equals(fornec33), true);

            Fornecedor fornec111 = new Fornecedor(fornec11.Nome, fornec11.Telefone, fornec11.Email,
                fornec11.Cidade, fornec11.Estado);
                      
            Fornecedor fornec222 = new Fornecedor(fornec22.Nome, fornec22.Telefone, fornec22.Email,
                fornec22.Cidade, fornec22.Estado);
            fornec111.Telefone = "(49) 9 9238 - 1121";

            Assert.AreEqual(fornec111.Equals(fornec11), false);
            Assert.AreEqual(fornec222.Equals(fornec22), true);
        }
    }
}
