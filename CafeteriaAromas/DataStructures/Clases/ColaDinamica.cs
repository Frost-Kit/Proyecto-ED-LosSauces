using System;
using System.Collections.Generic;
using CafeteriaAromas.Interfaces;

namespace CafeteriaAromas.DataStructures.Clases
{
    public class ColaDinamica<T> : ICola<T>
    {
        private class Nodo
        {
            public T Elemento { get; set; }
            public Nodo Siguiente { get; set; }

            public Nodo(T elemento)
            {
                Elemento = elemento;
                Siguiente = null;
            }
        }

        private Nodo _frente;
        private Nodo _final;
        private int _contador;

        public ColaDinamica()
        {
            _frente = null;
            _final = null;
            _contador = 0;
        }

        public bool IsEmpty() => _contador == 0;

        public bool IsFull() => false; // Al ser dinámica, no se llena

        public void Enqueue(T elemento)
        {
            Nodo nuevoNodo = new Nodo(elemento);
            if (IsEmpty())
            {
                _frente = nuevoNodo;
            }
            else
            {
                _final.Siguiente = nuevoNodo;
            }
            _final = nuevoNodo;
            _contador++;
        }

        public T Dequeue()
        {
            if (IsEmpty()) throw new InvalidOperationException("La cola está vacía.");
            
            T elemento = _frente.Elemento;
            _frente = _frente.Siguiente;
            _contador--;

            if (IsEmpty()) _final = null;

            return elemento;
        }

        public T Peek()
        {
            if (IsEmpty()) throw new InvalidOperationException("La cola está vacía.");
            return _frente.Elemento;
        }

        public int Size() => _contador;

        // Método auxiliar para que la vista HTML pueda dibujar la tabla sin romper la Cola
        public List<T> ToList()
        {
            List<T> lista = new List<T>();
            Nodo actual = _frente;
            while (actual != null)
            {
                lista.Add(actual.Elemento);
                actual = actual.Siguiente;
            }
            return lista;
        }
    }
}
