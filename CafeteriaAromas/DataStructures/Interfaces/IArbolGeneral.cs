using CafeteriaAromas.DataStructures.Clases;

namespace CafeteriaAromas.DataStructures.Interfaces;

public interface IArbolGeneral<T>
{
    NodoGeneral<T> BuscarNodo(NodoGeneral<T> nodo, T valor);
    bool AgregarNodo(T valorPadre, T nuevoValor);
    void Mostrar(NodoGeneral<T> nodo, string indent = "");
    bool EliminarNodo(T valor);
    bool EliminarRecursivo(NodoGeneral<T> nodo, T valor);
}