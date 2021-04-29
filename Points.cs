using System;
using System.Drawing;
using System.Collections.Generic;

namespace VectorEditor 
{
    //Статичный класс работы с узлами точек векторных фигур или обычных линий.
    public static class Points
    {
        //Статичный список узлов
        private static MyList<Point> points = new MyList<Point>();

        //Первая точка в списке
        public static Point? GetFirstP()
        {
            //Переменная, хранящая первый узел
            if (points.First == null) return null; //Если у нас нет первого узла, то и нет значения, возвращаем null
            return points.First.Value; //В узле получаем значение
        }

        //Последняя точка в списке
        public static Point? GetLastP()
        {
            //Переменная, хранящая последний узел.
            if (points.Last == null) return null; //Если у нас нет последнего узла, то и нет значения, возвращаем null
            return points.Last.Value; //В узле получаем значение
        }

        //Следующая точка после данной.
        //На вход подается точка.
        public static Point? GetNextP(Point p)
        {
            LinkedListNode<Point> point = points.Find(p);
            if (point == null) return null; //Если у нас нет такого узла в списке, то и следующего после него нет, возвращаем null
            if (point.Next == null) return null; //Если нет следующего узла, то и нет его значения, возвращаем null
            //Ищем в списке нужный узел с нашей точкой, затем получаем следующий узел и его значение.
            return point.Next.Value;
        }

        //Предыдущая точка после данной.
        //На вход подается точка.
        public static Point? GetPrevP(Point p)
        {
            LinkedListNode<Point> point = points.Find(p);
            if (point == null) return null; //Если у нас нет такого узла в списке, то и предыдущего после него нет, возвращаем null
            if (point.Previous == null) return null; //Если нет предыдущего узла, то и нет его значения, возвращаем null
            //Ищем в списке нужный узел с нашей точкой, затем получаем предыдущий узел и его значение.
            return point.Previous.Value;
        }

        //Индекс интересующей нас точки
        //На вход подается точка.
        public static int GetIndexOfP(Point p)
        {
            //Пользуемся моей функцией, реализованной в MyList, получение порядкового номера точки (начиная с нуля)
            return points.IndexOf(p);
        }

        //Получаем точку по индексу
        //На вход подается индекс.
        public static Point? GetP(int index)
        {
            //Пользуемся моей функцией, реализованной в MyList, получение узла по индексу, а затем получение его значения.
            LinkedListNode<Point> node = points.Get(index);
            if (node == null) return null; //Если узел с таким списком не нашелся, то возвращаем null
            return node.Value;
        }

        //Вставляем точку перед точкой
        //На вход подается точка, новая точка.
        public static void InsertPBeforeP(Point point, Point newpoint)
        {
            LinkedListNode<Point> node = points.Find(point);
            if (node == null) return; //Если у нас нет такого узла в списке, останавливаем выполнение функции.
            //Находим узел в списке по точке, добавляем после него новую точку.
            points.AddBefore(node, newpoint);
        }

        //Вставляем точку после точки
        //На вход подается точка, новая точка.
        public static void InsertPAfterP(Point point, Point newpoint)
        {
            LinkedListNode<Point> node = points.Find(point);
            if (node == null) return; //Если у нас нет такого узла в списке, останавливаем выполнение функции.
            //Находим узел в списке по точке, добавляем перед ним новую точку.
            points.AddAfter(node, newpoint);
        }

        //Добавляем точку в самое начало
        //На вход подается точка.
        public static void AddPToBegin(Point point)
        {
            //Вставляем точку в самое начало
            points.AddFirst(point);
        }

        //Добавляем точку в конец
        //На вход подается точка.
        public static void AddP(Point point)
        {
            //Вставляем точку в самый конец
            points.AddLast(point);
        }

        //Удалить нужную точку
        //На вход подается точка.
        public static void RemoveP(Point point)
        {
            points.Remove(point);
        }

        //Очистить список точек.
        public static void ClearAll()
        {
            points.Clear();
        }
    }
}
