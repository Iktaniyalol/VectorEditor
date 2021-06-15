using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    //Абстрактный класс фигуры.
    public abstract class Figure : GraphObject
    {
        protected Color color = Color.Empty; //Цвет фигуры

        //геттер и сеттер переменной color
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
    }
}
