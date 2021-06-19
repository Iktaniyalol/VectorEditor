using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VectorEditor.settings;

namespace VectorEditor.objects
{
    class VEllipse : Figure
    {
        public VEllipse(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIDs = new int[5]; //У эллипса будет храниться 4 точки + центр
            for (int i = 0; i < PointsIDs.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIDs[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }
        }
        public override void Draw(Graphics g) //Отрисовывание прямоугольника
        {
            MyPoint? one = Vector.FindPbyID(PointsIDs[0]); //первая точка
            MyPoint? two = Vector.FindPbyID(PointsIDs[1]); //вторая точка
            if (one == null || two == null) return; //если точек нет, не рисуем
            //Рисуем прямоугольник, берем угловые точки и вычисляем, какая из них самая левая, потом вычисляем длину и ширину
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillEllipse(new SolidBrush(color), one.Value.X, one.Value.Y, Math.Abs(one.Value.X - two.Value.X), Math.Abs(one.Value.Y - two.Value.Y)); //заливаем

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawEllipse(new Pen(thicknessColor, thickness), one.Value.X, one.Value.Y, Math.Abs(one.Value.X - two.Value.X), Math.Abs(one.Value.Y - two.Value.Y)); //рисуем контур эллипса
            }
        }

        public override void Select(Graphics g) //Выбор эллипса
        {
            MyPoint? begin = null, end = null;
            for (int i = 0; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. мы работаем с текущей точкой и следующей после нее.
                begin = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                end = Vector.FindPbyID(PointsIDs[i + 1]); //следующая
                if (begin == null || end == null) return; //если точек нет, не рисуем
                if (i == 0)
                {
                    g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, 1), begin.Value.X, begin.Value.Y, Math.Abs(begin.Value.X - end.Value.X), Math.Abs(begin.Value.Y - end.Value.Y)); //Рисуем контур выделения эллипса
                    g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), begin.Value.X, begin.Value.Y, Math.Abs(begin.Value.X - end.Value.X), Math.Abs(begin.Value.Y - end.Value.Y)); //рисуем контур выделения эллипса прямоугольником
                }

                //рисуем квадратики-точки у прямоугольника-выделителя
                g.FillRectangle(new SolidBrush(Color.White), begin.Value.X - 2, begin.Value.Y - 2, 5, 5);
                g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), begin.Value.X - 2, begin.Value.Y - 2, 5, 5); 
            }
            //Показываем центр эллипса
            g.FillEllipse(new SolidBrush(SettingsAndModes.CenterPointColor), end.Value.X - 2, end.Value.Y - 2, 5, 5);
            g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, 1), end.Value.X - 2, end.Value.Y - 2, 5, 5);
        }

        public override GraphObject Clone(int dx, int dy) //Клонирование данного объекта, передается смещение по x и y
        {
            Point[] points = new Point[PointsIDs.Length]; //Создаем массив с таким же размером, что и у текущего объекта
            for (int i = 0; i < PointsIDs.Length; i++)
            {
                MyPoint? curpoint = Vector.FindPbyID(PointsIDs[i]); //Ищем текущую точку
                if (curpoint == null) return null; //Если точки нет, не клонируем
                Point p = new Point(curpoint.Value.X + dx, curpoint.Value.Y + dx); //Вычисляем новую позицию точки;
                points[i] = p;
            }
            return new VEllipse(thickness, thicknessColor, color, points); //Создаем объект и возвращаем
        }

        public override void MoveTo(Point selected, Point newplace) //Перемещение эллипса по выбранной точке
        {

        }
    }
}
