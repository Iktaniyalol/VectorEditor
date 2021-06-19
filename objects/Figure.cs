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

       /* public abstract void TransformLeftX(int dx); //Сжатие/растяжение по X слева

        public abstract void TransformRightX(int dx); //Сжатие/растяжение по X справа

        public abstract void TransformLeftY(int dx); //Сжатие/растяжение по Y слева

        public abstract void TransformRightY(int dx); //Сжатие/растяжение по Y справа

        public abstract void TransformLeftXY(int dx, int dy); //Масштабирование по X и Y слева

        public abstract void TransformRightXY(int dx, int dy); //Масштабирование по X и Y справа*/
    }
}
