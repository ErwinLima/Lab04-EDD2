namespace Lab04
{
    public class Nodo<T>
    {
        public T Value { get; set; }
        public Nodo<T>? Left { get; set; }
        public Nodo<T>? Right { get; set; }
        public int Height { get; set; }

        public Nodo(T value)
        {
            this.Value = value;
            Left = null;
            Right = null;
            Height = 1;
        }
    }
}
