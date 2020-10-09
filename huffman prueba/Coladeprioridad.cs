using System;
using System.Collections.Generic;
using System.Text;

namespace huffman_prueba
{
    public class Coladeprioridad<T>
    {
        class Nodo //Creación del nodo
        {
            public double prioridad { get; set; }
            public T Letra { get; set; }
        }

        List<Nodo> encolar = new List<Nodo>();

        public int Tamañoheap = -1; //Se encuentra vacío
        public int contar { get { return encolar.Count; } } //contar nodos que hay

        public Coladeprioridad(Coladeprioridad<T> x)
        {
            encolar.AddRange(x.encolar); // Clonar el objeto es un constructor (Sirve para la las colas auxiliares)
            Tamañoheap = x.Tamañoheap; //Clona el tamaño
        }

        public Coladeprioridad()
        {
        }


        public void Enqueue(double prioridad, T letra)
        {
            Nodo nodo = new Nodo() { prioridad = prioridad, Letra = letra }; //Crea nodo
            encolar.Add(nodo); //Agrega a la lista
            Tamañoheap++; // como se agrego uno nuevo, se suma al nodo
            OrdenarHeapMin(Tamañoheap);

        }

        public T Dequeue()
        {
            prioridadMinValor(0); // Revisa la prioiridad más alta posición [0]
            if (Tamañoheap > -1)  // si existen mas dato
            {
                var valordevolver = encolar[0].Letra;
                encolar[0] = encolar[Tamañoheap];
                encolar.RemoveAt(Tamañoheap);
                Tamañoheap--;
                return valordevolver; //devuelve datos
            }
            else
                return default; //Si no, es null
        }


        private void OrdenarHeapMin(int i)
        {
            int aux = i;
            while (i >= 0 && encolar[(i - 1) / 2].prioridad > encolar[i].prioridad) // mientras que la posicion sea mayor o igual 0 y (se va al nodo izquierdo y lo compara con el nodo actual)
            {
                Cambiar(i, (i - 1) / 2); // si el izquierdo es mayor al actual que se acaba de ingresar se cambian 
                i = (i - 1) / 2; //luego i adquiere el valor del nodo izquierdo para entrar nuevamente al while
            }



        }


        private void prioridadMinValor(int i)
        {
            int izquierda = hijoiz(i); //Declara hijos izquierdos
            int derecha = hijoder(i); //Declara hijos derechos

            int pequeño = i;

            if (izquierda <= Tamañoheap && encolar[pequeño].prioridad > encolar[izquierda].prioridad) //Se verifica si esta lleno el heap, en la tabla se compara la prioridad mas alta con la de la del hijo izquierdo
                pequeño = izquierda;
            else if (izquierda <= Tamañoheap && encolar[pequeño].prioridad == encolar[izquierda].prioridad)
            { // Si las prioridades son iguales entonces se comparan las fechas
                prioridadMinValor(izquierda);
            }
            if (derecha <= Tamañoheap && encolar[pequeño].prioridad > encolar[derecha].prioridad) //Se verifica si esta lleno el heap, en la tabla se compara la prioridad mas alta con la de la del hijo derecho
                pequeño = derecha;
            else if (derecha <= Tamañoheap && encolar[pequeño].prioridad == encolar[derecha].prioridad)
            { // Si las prioridades son iguales entonces se comparan las fechas
                prioridadMinValor(derecha);
            }
            if (pequeño != i)
            {
                Cambiar(pequeño, i);
                prioridadMinValor(pequeño); //recursion
            }
        }

        private void Cambiar(int i, int j) //i es nuevo nodo y j el nodo izquierdo
        {
            var aux = encolar[i];
            encolar[i] = encolar[j];
            encolar[j] = aux;

        }

        private int hijoiz(int i)
        {
            return i * 2 + 1; //hijo izquierdo en el array
        }
        private int hijoder(int i)
        {
            return i * 2 + 2; //hijo derecho en el array
        }
    }

}
