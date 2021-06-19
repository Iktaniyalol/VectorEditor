using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VectorEditor.settings;

namespace VectorEditor.objects
{
    //Класс линии, наследуется от класса графический объект (GraphObject)
    public class Line : GraphObject
    {
        //Конструктор линии, передаем толщину, цвет и список точек, составляющие данную ограниченную линию
        public Line(float thickness, Color color, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = color; //Цвет линии
            PointsIDs = new int[3]; //Мы храним 3 точки, первую, центр и последнюю
            for (int i = 0; i < PointsIDs.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIDs[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }
        }

        public override void Draw(Graphics g) //Отрисовывание линии
        {
            MyPoint? begin = Vector.FindPbyID(PointsIDs[0]); //первая точка
            MyPoint? end = Vector.FindPbyID(PointsIDs[1]); //последняя точка
            if (begin == null || end == null) return; //если точек нет, не рисуем
            g.DrawLine(new Pen(thicknessColor, thickness), begin.Value.ConvertToPoint(), end.Value.ConvertToPoint()); //рисуем
        }

        public override void Select(Graphics g) //Выбор линии 
        {
            MyPoint? begin = Vector.FindPbyID(PointsIDs[0]); //первая точка
            MyPoint? end = Vector.FindPbyID(PointsIDs[1]); //последняя точка
            MyPoint? center = Vector.FindPbyID(PointsIDs[2]); //центр линии
            if (begin == null || end == null || center == null) return; //если точек нет, не рисуем
            g.DrawLine(new Pen(SettingsAndModes.EditLineColor, 1), begin.Value.ConvertToPoint(), end.Value.ConvertToPoint()); //рисуем
            g.FillEllipse(new SolidBrush(SettingsAndModes.EditPointColor), begin.Value.X - 2, begin.Value.Y - 2, 5, 5); //Показываем первую точку
            g.FillEllipse(new SolidBrush(SettingsAndModes.EditPointColor), end.Value.X - 2, end.Value.Y - 2, 5, 5); //Показываем первую точку
            g.FillEllipse(new SolidBrush(SettingsAndModes.CenterPointColor), ((begin.Value.X + end.Value.X) / 2) - 2, ((begin.Value.Y + end.Value.Y) / 2) - 2, 5, 5); //Показываем центр линии
            g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, 1), ((begin.Value.X + end.Value.X) / 2) - 2, ((begin.Value.Y + end.Value.Y) / 2) - 2, 5, 5);
        }

        public override GraphObject Clone(int dx, int dy) //Клонирование данного объекта, передается смещение по x и y
        {
            Point[] points = new Point[PointsIDs.Length]; //Создаем массив с таким же размером, что и у текущего объекта
            for (int i = 0; i < PointsIDs.Length; i++)
            {
                MyPoint? curpoint = Vector.FindPbyID(PointsIDs[i]); //Ищем текущую точку
                if (curpoint == null) return null; //Если точки нет, не клонируем
                Point p = new Point(curpoint.Value.X + dx, curpoint.Value.Y + dy); //Вычисляем новую позицию точки;
                points[i] = p;
            }
            return new Line(thickness, thicknessColor, points); //Создаем объект и возвращаем
        }

        public override void MoveTo(Point selected, Point newplace) //Перемещение линии по выбранной точке
        {

        }
    }
}
