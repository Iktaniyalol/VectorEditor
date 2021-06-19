using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    //Абстрактный класс графического объекта.
    public abstract class GraphObject
    {
        protected float thickness = 1f; //Толщина линии/обводки
        protected Color thicknessColor = Color.Black; //Цвет обводки/линии
        protected int[] PointsIDs; //Массив, в котором будут храниться все точки графического объекта

        public abstract void Draw(Graphics g);

        public abstract void Select(Graphics g);

        public void Remove() //Удаление векторного объекта
        {
            for (int i = 0; i < PointsIDs.Length; i++)
            {
                Vector.RemoveP(Vector.FindPbyID(PointsIDs[i])); //Удаляем точки
            }
            Vector.GetAllFigures().Remove(this); //Удаляем объект из списка
        }

        public abstract void MoveTo(Point selected, Point newplace);

        public abstract GraphObject Clone(int dx, int dy); //Клонирование данного объекта, передается смещение по x и y

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

        public int[] GetPointsIDs() // Получить ID всех точек в виде массива
        {
            return PointsIDs;
        }
    }
}
