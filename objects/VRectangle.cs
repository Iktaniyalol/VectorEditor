using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    class VRectangle : Figure
    {
        public VRectangle(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIndexes = new long[2]; //У прямоугольника(квадрата) будет храниться 2 точки
            for (int i = 0; i < PointsIndexes.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIndexes[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }
        }
        public override void Draw(Graphics g) //Отрисовывание прямоугольника
        {
            MyPoint? one = Vector.FindPbyID(PointsIndexes[0]); //первая точка
            MyPoint? two = Vector.FindPbyID(PointsIndexes[1]); //вторая точка
            if (one == null || two == null) return; //если точек нет, не рисуем
            //Рисуем прямоугольник, берем угловые точки и вычисляем, какая из них самая левая, потом вычисляем длину и ширину
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillRectangle(new SolidBrush(color), Math.Min(one.Value.X, two.Value.X), Math.Min(one.Value.Y, two.Value.Y), Math.Abs(one.Value.X - two.Value.X), Math.Abs(one.Value.Y - two.Value.Y)); //заливаем

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawRectangle(new Pen(thicknessColor, thickness), Math.Min(one.Value.X, two.Value.X), Math.Min(one.Value.Y, two.Value.Y), Math.Abs(one.Value.X - two.Value.X), Math.Abs(one.Value.Y - two.Value.Y)); //рисуем контур прямоугольника
            }                                                                                                                                                                                           //
        }
    }
}
