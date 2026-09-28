using CafeteriaAromas.DataStructures.Interfaces;

namespace CafeteriaAromas.DataStructures.Clases;

public class ArbolGeneral<T> : IArbolGeneral<T>
{
    public NodoGeneral<T> Raiz { get; set; }

    public ArbolGeneral(T valorRaiz)
    {
        Raiz = new NodoGeneral<T>(valorRaiz);
    }

    public bool AgregarNodo(T valorPadre, T nuevoValor)
    {
        NodoGeneral<T> Padre = BuscarNodo(Raiz, valorPadre);
        if (Padre == null)
            return false;
        Padre.Hijos.Add(new NodoGeneral<T>(nuevoValor));
        return true;
    }

    public NodoGeneral<T> BuscarNodo(NodoGeneral<T> nodo, T valor)
    {
        if (nodo == null) return null;

        if (nodo.Valor.Equals(valor))
            return nodo;
        foreach (NodoGeneral<T> item in nodo.Hijos)
        {
            NodoGeneral<T> nodoEncontrado = BuscarNodo(item, valor);
            if (nodoEncontrado != null)
                return nodoEncontrado;
        }

        return null;
    }

    public bool EliminarNodo(T valor)
    {
        if (Raiz == null)
            return false;
        if (Raiz.Valor.Equals(valor))
        {
            Raiz = null;
            return true;
        }

        return EliminarRecursivo(Raiz, valor);
    }

    public bool EliminarRecursivo(NodoGeneral<T> nodo, T valor)
    {
        foreach (NodoGeneral<T> hijo in nodo.Hijos)
        {
            if (hijo.Valor.Equals(valor))
            {
                nodo.Hijos.Remove(
                    hijo);
                return true;
            }

            if (EliminarRecursivo(hijo, valor))
                return true;
        }

        return false;
    }

    public void MostrarArbol()
    {
        Mostrar(Raiz);
    }

    public void Mostrar(NodoGeneral<T> nodo, string indent = "")
    {
        Console.WriteLine(indent + nodo.Valor);
        Console.WriteLine("\n-");
        foreach (var item in nodo.Hijos)
        {
            string pre = indent + "|--";

            Mostrar(item, pre);
        }
    }
}