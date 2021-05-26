using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    public abstract class GraphObject
    {
        protected int thickness = 1; //Толщина линии/обводки
        protected Color thicknessColor = Color.Black; //Цвет обводки/линии

        public abstract void Draw(Graphics g);

        //setter getter
        public int Thickness
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


        //setter getter
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
    }
}
