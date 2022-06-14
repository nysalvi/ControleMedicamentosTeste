namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class Funcionario : EntidadeBase<Funcionario>
    {

        public Funcionario(string nome, string login, string senha)
        {
            Nome = nome;
            Login = login;
            Senha = senha;
        }

        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public override bool Equals(object obj)
        {
            Funcionario obj2 = obj as Funcionario;
            if (obj2 == null)
                return false;
            if (this.ToString() != obj2.ToString())
                return false;
            return true;
        }
        public override string ToString()
        {
            return string.Format("|Nome /{0} |Login /{1} |Senha /{2}", Nome, Login, Senha);
        }
    }
}
