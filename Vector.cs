using System;
using System.Drawing;
using System.Collections.Generic;
using VectorEditor.objects;

namespace VectorEditor 
{
    //Моя собственная структура для точки, состоит из переменных x,y и ID.
    public struct MyPoint
    {
        private int x; //координата x
        private int y; //координата y
        private long id; //Айди данной точки, статичен

        //Конструктор создания
        public MyPoint(int x, int y, long id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }
        //setter getter
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        //setter getter
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        //getter
        public long ID
        {
            get
            {
                return id;
            }
        }

        //оператор не равно
        public static bool operator !=(MyPoint left, MyPoint right)
        {
            return !(left == right);
        }

        //оператор равно
        public static bool operator ==(MyPoint left, MyPoint right)
        {
            return left.X == right.X && left.Y == right.Y && left.ID == right.ID;
        }

        //эквивалентность
        public override bool Equals(object obj)
        {
            if (!(obj is MyPoint)) return false;
            MyPoint comp = (MyPoint)obj;
            return comp.X == this.X && comp.Y == this.Y && comp.ID == this.ID;
        }

        //Конвертация в Point структуру, встроенную в C#
        public Point ConvertToPoint()
        {
            return new Point(X, Y);
        }

        //HashCode структуры
        public override int GetHashCode()
        {
            return x ^ y;
        }

        //Конвертация структуры в String, для вывода в консоль
        public override string ToString()
        {
            return "{X=" + X.ToString() + ",Y=" + Y.ToString() + ",ID=" + ID.ToString() + "}";
        }
    }

    //Статичный класс работы с узлами точек векторных фигур или обычных линий.
    public static class Vector
    {
        //Статичный список узлов с точками
        private static LinkedList<MyPoint> points = new LinkedList<MyPoint>();
        private static List<Figure> figures = new List<Figure>();
        public static long IDS = 0;

        //Первая точка в списке
        public static MyPoint? GetFirstP()
        {
            //Переменная, хранящая первый узел
            if (points.First == null) return null; //Если у нас нет первого узла, то и нет значения, возвращаем null
            return points.First.Value; //В узле получаем значение
        }

        //Последняя точка в списке
        public static MyPoint? GetLastP()
        {
            //Переменная, хранящая последний узел.
            if (points.Last == null) return null; //Если у нас нет последнего узла, то и нет значения, возвращаем null
            return points.Last.Value; //В узле получаем значение
        }

        //Следующая точка после данной.
        //На вход подается точка.
        public static MyPoint? GetNextP(MyPoint? p)
        {
            if (p != null)
            {
                LinkedListNode<MyPoint> point = points.Find(p.Value);
                if (point == null) return null; //Если у нас нет такого узла в списке, то и следующего после него нет, возвращаем null
                if (point.Next == null) return null; //Если нет следующего узла, то и нет его значения, возвращаем null
                 //Ищем в списке нужный узел с нашей точкой, затем получаем следующий узел и его значение.
                return point.Next.Value;
            }
            return null;
        }

        //Предыдущая точка после данной.
        //На вход подается точка.
        public static MyPoint? GetPrevP(MyPoint? p)
        {
            if (p != null)
            {
                LinkedListNode<MyPoint> point = points.Find(p.Value);
                if (point == null) return null; //Если у нас нет такого узла в списке, то и предыдущего после него нет, возвращаем null
                if (point.Previous == null) return null; //Если нет предыдущего узла, то и нет его значения, возвращаем null
                //Ищем в списке нужный узел с нашей точкой, затем получаем предыдущий узел и его значение.
                return point.Previous.Value;
            }
            return null;
        }

        //Получаем точку по ID
        //На вход подается id.
        public static MyPoint? FindPbyID(int id)
        {
            //Получаем самый первый узел, его индекс будет 0
            LinkedListNode<MyPoint> node = points.First;
            for (int i = 0; i <= points.Count; i++)
            {
                if (node == null) return null; //Если узел null, возвращаем null, список пустой
                if (node.Value == null) return null; //Если значение в узле null, возвращаем null
                if (node.Value.ID == id) return node.Value;
                node = node.Next;
            }
            return null;
        }

        //Вставляем точку перед точкой
        //На вход подается точка, новая точка.
        public static void InsertPBeforeP(MyPoint? point, MyPoint? newpoint)
        {
            if (point != null && newpoint != null)
            {
                LinkedListNode<MyPoint> node = points.Find(point.Value);
                if (node == null) return; //Если у нас нет такого узла в списке, останавливаем выполнение функции.
                //Находим узел в списке по точке, добавляем после него новую точку.
                points.AddBefore(node, newpoint.Value);
            }
        }

        //Вставляем точку после точки
        //На вход подается точка, новая точка.
        public static void InsertPAfterP(MyPoint? point, MyPoint? newpoint)
        {
            if (point != null && newpoint != null)
            {
                LinkedListNode<MyPoint> node = points.Find(point.Value);
                if (node == null) return; //Если у нас нет такого узла в списке, останавливаем выполнение функции.
                //Находим узел в списке по точке, добавляем перед ним новую точку.
                points.AddAfter(node, newpoint.Value);
            }
        }

        //Добавляем точку в самое начало
        //На вход подается точка.
        public static void AddPToBegin(MyPoint? point)
        {
            if (point != null)
            {
                //Вставляем точку в самое начало
                points.AddFirst(point.Value);
            }
        }

        //Добавляем точку в конец
        //На вход подается точка.
        public static void AddP(MyPoint? point)
        {
            if (point != null)
            {
                //Вставляем точку в самый конец
                points.AddLast(point.Value);
            }
        }

        //Удалить нужную точку
        //На вход подается точка.
        public static void RemoveP(MyPoint? point)
        {
            if (point != null)
            {
                LinkedListNode<MyPoint> node = points.Find(point.Value);
                if (node == null) return; //Если у нас нет такого узла в списке, останавливаем выполнение функции.
                points.Remove(point.Value);
            }
        }

        //Очистить список точек.
        public static void ClearAll()
        {
            points.Clear();
        }

        //Вывести все точки как String строка.
        //Используется для дебага
        public static String PointsToString()
        {
            String str = "";
            foreach (MyPoint point in points) {
                str = str + "{X=" + point.X.ToString() + ",Y=" + point.Y.ToString() + ",ID=" + point.ID.ToString() + "}";
            }
            return str;
        }
    }
}
