namespace ControleMedicamentos.Dominio
{
    public abstract class EntidadeBase<T>
    {
        public int Numero { get; set; }
        public override bool Equals(object obj)
        {
            T obj2 = (T)obj;
            if (obj2 == null)
                return false;
            if (this.ToString() != obj2.ToString())
                return false;
            return true;
        }
    }
}
