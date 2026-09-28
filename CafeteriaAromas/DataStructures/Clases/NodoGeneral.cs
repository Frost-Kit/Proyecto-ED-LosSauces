namespace CafeteriaAromas.DataStructures.Clases;

public class NodoGeneral<T>
{
    public T Valor { get; set; }
    public List<NodoGeneral<T>> Hijos { get; set; }

    public NodoGeneral(T valor)
    {
        Valor = valor;
        Hijos = new List<NodoGeneral<T>>();
    }
}