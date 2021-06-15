using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    class VPolygone : Figure
    {
        public VPolygone(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIndexes = new long[Points.Length];
            for (int i = 0; i < PointsIndexes.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIndexes[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }
        }
        public override void Draw(Graphics g) //Отрисовывание многоугольника
        {
            Point[] points = new Point[PointsIndexes.Length];
            for (int i = 0; i < PointsIndexes.Length; i++) //Перебираем список точек
            {
                MyPoint? mypoint = Vector.FindPbyID(PointsIndexes[i]); //Ищем данную точку
                if (mypoint == null) return; //если точка не найдена, не рисуем
                points[i] = Vector.FindPbyID(PointsIndexes[i]).Value.ConvertToPoint(); //Конвертируем
            }

            //Рисуем многоугольник, пользуемся drawPolygon, передаем все точки
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillPolygon(new SolidBrush(color), points); //Заливка

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawPolygon(new Pen(thicknessColor, thickness), points); //Рисуем контур
            }
        }
    }
}
