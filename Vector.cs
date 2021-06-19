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
        private int id; //Айди данной точки, статичен
        private GraphObject vobject; //Ссылка на векторный объект, которому принадлежит данная точка

        //Конструктор создания
        public MyPoint(int x, int y, int id, GraphObject vobject)
        {
            this.x = x;
            this.y = y;
            this.id = id;
            this.vobject = vobject;
        }
        //геттер и сеттер переменной x
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
        //геттер и сеттер переменной y
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
        //геттер переменной id
        public int ID
        {
            get
            {
                return id;
            }
        }

        //геттер переменной vobject
        public GraphObject VObject
        {
            get
            {
                return vobject;
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
            return left.X == right.X && left.Y == right.Y && left.ID == right.ID && left.vobject == right.vobject;
        }

        //эквивалентность
        public override bool Equals(object obj)
        {
            if (!(obj is MyPoint)) return false;
            MyPoint comp = (MyPoint)obj;
            return comp.X == this.X && comp.Y == this.Y && comp.ID == this.ID && comp.vobject == this.vobject;
        }

        //Конвертация в Point структуру, встроенную в C#
        public Point ConvertToPoint()
        {
            return new Point(X, Y);
        }

        //HashCode структуры, грубо говоря, уникальный код созданного объекта
        public override int GetHashCode()
        {
            return x ^ y ^ ID; //Делаем исключающее или между x,y и ID
        }

        //Конвертация структуры в String, для вывода в консоль
        public override string ToString()
        {
            return "{X=" + X.ToString() + ",Y=" + Y.ToString() + ",ID=" + ID.ToString() + ", VObject=" + VObject.ToString() + "}";
        }
    }

    //Статичный класс работы с узлами точек векторных фигур или обычных линий.
    public static class Vector
    {
        //Статичный список узлов с точками
        private static List<MyPoint> points = new List<MyPoint>();
        private static List<GraphObject> figures = new List<GraphObject>();
        public static int IDS = 0;

        //Первая точка в списке
        public static MyPoint? GetFirstP()
        {
            //Переменная, хранящая первый узел
            if (points.Count == 0) return null; //Если у нас пустой список, возвращаем null
            return points[0]; //Возвращаем значение
        }

        //Последняя точка в списке
        public static MyPoint? GetLastP()
        {
            //Переменная, хранящая последний узел.
            if (points.Count == 0) return null; //Если у нас пустой список, возвращаем null
            return points[points.Count - 1]; //Возвращаем значение
        }

        //Следующая точка после данной.
        //На вход подается точка.
        public static MyPoint? GetNextP(MyPoint? p)
        {
            if (p.HasValue) //Проверяем, есть ли точка
            {
                int index = points.IndexOf(p.Value);
                if (index == -1) return null; //Если индекс -1, то такой точки в списке нет. Возвращаем null
                if (index + 1 > points.Count - 1) return null; //Следующей точки нет
                return points[index + 1];
            }
            return null;
        }

        //Предыдущая точка после данной.
        //На вход подается точка.
        public static MyPoint? GetPrevP(MyPoint? p)
        {
            if (p.HasValue) //Проверяем, есть ли точка
            {
                int index = points.IndexOf(p.Value);
                if (index == -1) return null; //Если индекс -1, то такой точки в списке нет. Возвращаем null
                if (index - 1 < 0) return null; //Предыдущей точки нет
                return points[index - 1];
            }
            return null;
        }

        //Получаем точку по ID
        //На вход подается id.
        //Используется алгоритм двоичного поиска
        public static MyPoint? FindPbyID(int id)
        {
            int p = 0, q = points.Count - 1, CenterIndex = points.Count / 2; //Края проверки
            MyPoint center = points[CenterIndex]; //Делим длину списка пополам
            if (center.ID == id) return center; //проверяем на соответствие
            if (center.ID > id) q = CenterIndex - 1; // Если айди больше чем искомый, сдвигаем правый край проверки
            else p = CenterIndex + 1; //иначе сдвигаем левый край проверки
            while (p <= q) //пока левый край меньше или равен правому
            {
                CenterIndex = (p + q) / 2; //меняем центр
                center = points[CenterIndex]; //берем новый центр
                if (center.ID == id) return center; //проверяем на соответствие
                if (center.ID > id) q = CenterIndex - 1; // Если айди больше чем искомый, сдвигаем правый край проверки
                else p = CenterIndex + 1; //иначе сдвигаем левый край проверки
            }
            return null;
        }

        //Добавляем точку в конец
        //На вход подается точка.
        public static void AddP(MyPoint? point)
        {
            if (point.HasValue) //Проверяем, есть ли точка
            {
                //Вставляем точку в самый конец
                points.Add(point.Value);
            }
        }

        //Удалить нужную точку
        //На вход подается точка.
        public static void RemoveP(MyPoint? point)
        {
            if (point.HasValue) //Проверяем, есть ли точка
            {
                if (!points.Contains(point.Value)) return; //Если такой точки нет в списке, удалять нечего
                points.Remove(point.Value); //Удаляем
            }
        }

        //Изменить координаты уже существующей точки
        //На вход подается новый x, y, сама точка
        public static void SetCoordsP(int x, int y, MyPoint? point)
        {
            if (point.HasValue) //Проверяем, есть ли точка
            {
                if (!points.Contains(point.Value)) return; //Если такой точки нет в списке, мы не можем поменять ей координаты
                MyPoint p = point.Value;
                int index = points.IndexOf(p); //Получаем индекс точки
                p.X = x; //Приравниваем x
                p.Y = y; //Приравниваем y
                points[index] = p; //Т.к. структуры при приравнивании делают полное копирование и не имеют ссылок, нам нужно назначить на этот индекс переменную с новыми точками
            }
        }

        //Очистить список точек.
        public static void ClearAll()
        {
            points.Clear();
        }

        //геттер списка точек
        public static List<MyPoint> VPoints
        {
            get
            {
                return points;
            }
        }

        //Добавление новой фигуры в список
        public static void AddNewFigure(GraphObject figure)
        {
            figures.Add(figure);
        }

        //Удаление фигуры из списка
        public static void RemoveFigure(GraphObject figure)
        {
            figures.Remove(figure);
        }

        public static List<GraphObject> GetAllFigures()
        {
            return figures;
        }

        //Вывести все точки как String строка.
        //Используется для дебага
        public static String PointsToString()
        {
            String str = "";
            foreach (MyPoint point in points) {
                str = str + "{X=" + point.X.ToString() + ",Y=" + point.Y.ToString() + ",ID=" + point.ID.ToString() + ", VObject=" + point.VObject.ToString() + "}";
            }
            return str;
        }
    }
}
