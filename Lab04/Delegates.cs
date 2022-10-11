namespace Lab04
{
    public class Delegates
    {
        public static System.Comparison<Persona> NameComparison = delegate (Persona p1, Persona p2)
        {
            return p1.name.CompareTo(p2.name);
        };

        public static System.Comparison<Persona> DPIComparison = delegate (Persona p1, Persona p2)
        {
            return p1.dpi.CompareTo(p2.dpi);
        };
    }
}
