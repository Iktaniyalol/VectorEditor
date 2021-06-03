using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    //Абстрактный класс графического объекта.
    public abstract class GraphObject
    {
        protected float thickness = 0.5f; //Толщина линии/обводки
        protected Color thicknessColor = Color.Black; //Цвет обводки/линии
        protected long[] PointsIndexes; //Массив, в котором будут храниться все точки графического объекта

        public abstract void Draw(Graphics g);

        //геттер и сеттер переменной thickness
        public float Thickness
        {
            get
            {
                return thickness;
            }
            set
            {
                thickness = value;
            }
        }


        //геттер и сеттер переменной thicknessColor
        public Color ThicknessColor
        {
            get
            {
                return thicknessColor;
            }
            set
            {
                thicknessColor = value;
            }
        }

        public long[] GetPointsIndexes() // Получить ID всех точек в виде массива
        {
            return PointsIndexes;
        }
    }
}
