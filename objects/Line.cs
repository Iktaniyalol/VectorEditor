using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

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
            PointsIndexes = new long[Points.Length]; //Сколько передали точек, такой и будет размер массива
            for (int i = 0; i < PointsIndexes.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIndexes[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }
        }

        public override void Draw(Graphics g) //Отрисовывание линии
        {
            //Перебираем все точки, составляющие ограниченную линию
            for (int i = 0; i < PointsIndexes.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. мы работаем с текущей точкой и следующей после нее.
                MyPoint? one = Vector.FindPbyID(PointsIndexes[i]); //текущая точка
                MyPoint? two = Vector.FindPbyID(PointsIndexes[i + 1]); //следующая
                if (one == null || two == null) break; //если точек нет, не рисуем
                if (!thicknessColor.IsEmpty) // Если у нас есть цвет контура
                {
                    g.DrawLine(new Pen(thicknessColor, thickness), one.Value.ConvertToPoint(), two.Value.ConvertToPoint()); //рисуем
                }
            
            }
        }
    }
}
