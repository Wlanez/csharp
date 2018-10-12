// Copyright © Microsoft Corporation. Reservados todos los derechos.
// Este código se ha publicado de acuerdo con los términos de la 
// licencia pública de Microsoft (MS-PL, http://opensource.org/licenses/ms-pl.html).
//
// (C) Microsoft Corporation. Reservados todos los derechos.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Generics_CSharp
{
    //Escribir parámetro T entre corchetes angulares.
    public class MyList<T> : IEnumerable<T>
    {
        protected Node head;
        protected Node current = null;

        // El tipo anidado es también genérico en T
        protected class Node
        {
            public Node next;
            //T como tipo de datos de miembro privado.
            private T data;
            //T utilizado en un constructor no genérico.
            public Node(T t)
            {
                next = null;
                data = t;
            }
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }
            //T como tipo devuelto de propiedad.
            public T Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        public MyList()
        {
            head = null;
        }

        //T como tipo de parámetro de método.
        public void AddHead(T t)
        {
            Node n = new Node(t);
            n.Next = head;
            head = n;
        }

        // Implementar GetEnumerator para devolver IEnumerator<T> con el fin de habilitar
        // iteración foreach de nuestra lista. Tenga en cuenta que en C# 2.0 
        // no es necesario implementar Current y MoveNext.
        // El compilador creará una clase que implemente IEnumerator<T>.
        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        // Debemos implementar este método porque
        // IEnumerable<T> hereda IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class SortedList<T> : MyList<T> where T : IComparable<T>
    {
        // Sencillo algoritmo de ordenación no optimizado que
        // ordena los elementos de la lista del más bajo al más alto:
        public void BubbleSort()
        {
            if (null == head || null == head.Next)
                return;

            bool swapped;
            do
            {
                Node previous = null;
                Node current = head;
                swapped = false;

                while (current.next != null)
                {
                    //  Puesto que necesitamos llamar a este método, la clase SortedList
                    //  está restringida en IEnumerable<T>
                    if (current.Data.CompareTo(current.next.Data) > 0)
                    {
                        Node tmp = current.next;
                        current.next = current.next.next;
                        tmp.next = current;

                        if (previous == null)
                        {
                            head = tmp;
                        }
                        else
                        {
                            previous.next = tmp;
                        }
                        previous = tmp;
                        swapped = true;
                    }

                    else
                    {
                        previous = current;
                        current = current.next;
                    }

                }// while final
            } while (swapped);
        }
    }

    // Clase sencilla que implementa IComparable<T>
    // utilizándose a sí misma como argumento de tipo. Éste es un
    // modelo de diseño común en objetos que se
    // almacenan en listas genéricas.
    public class Person : IComparable<Person>
    {
        string name;
        int age;

        public Person(string s, int i)
        {
            name = s;
            age = i;
        }

        // Esto dará lugar a que los elementos de la lista
        // se ordenen por valores de edad.
        public int CompareTo(Person p)
        {
            return age - p.age;
        }

        public override string ToString()
        {
            return name + ":" + age;
        }

        // Debe implementar Equals.
        public bool Equals(Person p)
        {
            return (this.age == p.age);
        }
    }

    class Generics
    {
        static void Main(string[] args)
        {
            //Declarar y crear una instancia de una clase SortedList genérica nueva.
            //Person es el argumento de tipo.
            SortedList<Person> list = new SortedList<Person>();

            //Crear valores de nombre y edad para inicializar objetos Person.
            string[] names = new string[] { "Franscoise", "Bill", "Li", "Sandra", "Gunnar", "Alok", "Hiroyuki", "Maria", "Alessandro", "Raul" };
            int[] ages = new int[] { 45, 19, 28, 23, 18, 9, 108, 72, 30, 35 };

            //Rellenar la lista.
            for (int x = 0; x < names.Length; x++)
            {
                list.AddHead(new Person(names[x], ages[x]));
            }

            Console.WriteLine("Unsorted List:");
            //Imprimir la lista desordenada.
            foreach (Person p in list)
            {
                Console.WriteLine(p.ToString());
            }

            //Ordenar la lista.
            list.BubbleSort();

            Console.WriteLine(String.Format("{0}Sorted List:", Environment.NewLine));
            //Imprimir la lista ordenada.
            foreach (Person p in list)
            {
                Console.WriteLine(p.ToString());
            }

            Console.WriteLine("Done");
        }
    }

}
