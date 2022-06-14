namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class Paciente : EntidadeBase<Paciente>
    {
        public Paciente(string nome, string cartaoSUS)
        {
            Nome = nome;
            CartaoSUS = cartaoSUS;
        }

        public string Nome { get; set; }
        public string CartaoSUS { get; set; }
        public override bool Equals(object obj)
        {
            Paciente obj2 = obj as Paciente;
            if (obj2 == null)
                return false;
            if (this.ToString() != obj2.ToString())
                return false;
            return true;
        }
        public override string ToString()
        {
            return string.Format("|Nome /{0} |CartaoSUS /{1}", Nome, CartaoSUS);
        }
    }
}
