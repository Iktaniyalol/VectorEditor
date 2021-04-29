using System;
using System.Collections.Generic;

namespace VectorEditor
{
    //Моя личная модификация LinkedList, добавлены индексы элементов, хранящихся в списке.
    public class MyList<T> : LinkedList<T>
    {
        //Функция получения индекса данного элемента в списке, подается T.
        //Возвращает -1, если данного элемента нет в списке
        public int IndexOf(T element)
        {
            //Получаем самый первый узел, его индекс будет 0
            LinkedListNode<T> node = First;
            if (node == null) return -1; //Если первого узла в списке нет, список пустой, возвращаем -1
            if (element == null) //Если искомый элемент null, ищем в списке любой null и возвращаем его индекс
            {
                for (int i = 0; i <= Count; i++)
                {
                    if (node == null) return i; //Если узел null, возвращаем i, мы нашли null элемент в списке
                    if (node.Value == null) return i; //Если значение в узле null, возвращаем i, мы нашли null элемент в списке
                    node = node.Next;
                }
            } else //Ищем элемент в списке и возвращаем насчитанный индекс
            {
                for (int i = 0; i <= Count; i++)
                {
                    if (node == null) return -1; //Если узел null, мы уже вышли за пределы списка, возвращаем -1, такого элемента в списке нет
                    if (node.Value.Equals(element)) return i; //Сравниваем, эквивалентны ли значения/объекты, если да, мы нашли индекс нужного элемента
                    node = node.Next;
                }
            }
            return -1;
        }

        //Функция получения индекса данного элемента в списке, подается LinkedListNode<T>
        public int IndexOf(LinkedListNode<T> element)
        {
            return IndexOf(element.Value);
        }

        //Функция получения последнего индекса данного элемента в списке, подается T.
        //Возвращает -1, если данного элемента нет в списке
        public int LastIndexOf(T element)
        {
            //Получаем самый последний узел, его индекс будет Count-1
            LinkedListNode<T> node = Last;
            if (node == null) return -1; //Если последнего узла в списке нет, список пустой, возвращаем -1
            if (element == null) //Если искомый элемент null, берем индекс элемента после последнего элемента в списке, он будет последним null
            {
                return Count;
            }
            else //Ищем элемент в списке и возвращаем насчитанный индекс
            {
                for (int i = Count; i >= 0; i--)
                {
                    if (node == null) return -1; //Если узел null, мы уже вышли за пределы списка, возвращаем -1, такого элемента в списке нет
                    if (node.Value.Equals(element)) return i; //Сравниваем, эквивалентны ли значения/объекты, если да, мы нашли индекс нужного элемента
                    node = node.Previous;
                }
            }
            return -1;
        }

        //Функция получения последнего индекса данного элемента в списке, подается LinkedListNode<T>
        public int LastIndexOf(LinkedListNode<T> element)
        {
            return LastIndexOf(element.Value);
        }

        //Функция получения элемента из списка по индексу.
        public LinkedListNode<T> Get(int index)
        {
            //Получаем самый первый узел
            LinkedListNode<T> node = First;
            if (node == null) return null; //Если первого узла в списке нет, список пустой, возвращаем null
            if (index + 1 > Count) return null; //Если искомый индекс+1(у нас нумерация с 0) больше, чем количество элементов в списке, возвращаем null.
            //Счетчик, начинаем с нуля
            int i = 0;
            while (i < index)
            { //Если счетчик меньше, чем нужный нам индекс, значит получаем следующий узел
                node = node.Next;
                i++;
            }
            return node;
        }
    }
}
