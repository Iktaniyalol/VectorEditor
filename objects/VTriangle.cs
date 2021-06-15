using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    class VTriangle : Figure
    {
        public VTriangle(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIndexes = new long[3]; //У треугольника будет храниться 3 точки
            for (int i = 0; i < PointsIndexes.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIndexes[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }
        }
        public override void Draw(Graphics g) //Отрисовывание треугольника
        {
            MyPoint? one = Vector.FindPbyID(PointsIndexes[0]); //первая точка
            MyPoint? two = Vector.FindPbyID(PointsIndexes[1]); //вторая точка
            MyPoint? three = Vector.FindPbyID(PointsIndexes[2]); //третья точка
            if (one == null || two == null || three == null) return; //если точек нет, не рисуем
            //Рисуем треугольник, пользуемся drawPolygon, передаем все точки
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillPolygon(new SolidBrush(color), new Point[] {one.Value.ConvertToPoint(), two.Value.ConvertToPoint(), three.Value.ConvertToPoint()}); //Заливка

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawPolygon(new Pen(thicknessColor, thickness), new Point[] { one.Value.ConvertToPoint(), two.Value.ConvertToPoint(), three.Value.ConvertToPoint() }); //Рисуем контур
            }                                                                                                                                                                             
        }
    }
}
