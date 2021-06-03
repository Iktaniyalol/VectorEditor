using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    class Rectangle : Figure
    {
        public Rectangle(float thickness, Color thicknessColor, Color color, Point[] points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Color; //Цвет заливки

        }
        public override void Draw(Graphics g) //Отрисовывание прямоугольника
        {

        }
    }
}
