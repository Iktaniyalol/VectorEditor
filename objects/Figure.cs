using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    public abstract class Figure : GraphObject
    {
        protected Color color = Color.Black; //Цвет фигуры


        //setter getter
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
