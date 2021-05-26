using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    public class Line : GraphObject
    {
        protected long FirstPointIndex; //Индекс первой точки
        protected long SecondPointIndex; //Индекс второй точки
        public Line(Point one, Point two, int thickness, Color color)
        {
            this.thickness = thickness; //Толщина линии
            MyPoint pone = new MyPoint(one.X, one.Y, Vector.IDS++);
            MyPoint ptwo = new MyPoint(two.X, two.Y, Vector.IDS++);
            this.FirstPointIndex = pone.ID; //записываем ID первой точки
            this.SecondPointIndex = ptwo.ID; //записываем ID второй точки
            Vector.AddP(pone);
            Vector.AddP(ptwo);
        }

        public override void Draw(Graphics g) //Отрисовывание линии
        {
            g.DrawLine(new Pen(Color.Black, thickness), GetFirstPoint().ConvertToPoint(), GetSecondPoint().ConvertToPoint());
        }

        protected MyPoint GetFirstPoint() //Получить первую точку фигуры
        {
            MyPoint? point = Vector.FindPbyID(FirstPointIndex);
            if (point == null)
            {
                Vector.RemoveFigure(this);
            }
            return point.Value;
        }

        protected MyPoint GetSecondPoint() //Получить вторую точку фигуры
        {
            MyPoint? point = Vector.FindPbyID(SecondPointIndex);
            if (point == null)
            {
                Vector.RemoveFigure(this);
            }
            return point.Value;
        }
    }
}
