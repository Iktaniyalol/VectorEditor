using System;
using System.Collections.Generic;
using System.Text;

namespace VectorEditor.objects
{
    public abstract class CustomFigure : Figure
    {
        protected int pointCount = 3; //Количество точек

        //геттер переменной PointCount
        public int PointCount
        {
            get
            {
                return pointCount;
            }
        }

        public abstract void RecalculateSelectArea(); //Пересчет точек выделения
    }
}
