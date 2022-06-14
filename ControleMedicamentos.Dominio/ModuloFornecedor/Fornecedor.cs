namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class Fornecedor : EntidadeBase<Fornecedor>
    {
        public Fornecedor(string nome, string telefone, string email, string cidade, string estado)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Cidade = cidade;
            Estado = estado;
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public override bool Equals(object obj)
        {
            Fornecedor obj2 = obj as Fornecedor;
            if (obj2 == null)
                return false;
            if (this.ToString() != obj2.ToString())
                return false;
            return true;
        }
        public override string ToString()
        {
            return string.Format("|Nome /{0} |Telefone /{1} |Email /{2} |Cidade /{3} |Estado /{4}",
                Nome, Telefone, Email, Cidade, Estado);
        }
    }
}
