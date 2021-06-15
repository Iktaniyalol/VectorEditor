using System;
using System.Drawing;

namespace VectorEditor.settings
{
    public static class SettingsAndModes
    {
        //Чтобы было проще, сделаем режим в виде Enum-ов
        public enum EditorMode
        {
            Cursor,
            Line,
            Circle,
            Rectangle,
            Polygon
        }

        static EditorMode mode = EditorMode.Cursor; //Текущий мод
        static Color selectedThicknessColor = Color.BlueViolet; //Цвет обводки выделенной фигуры/линии

        //геттер и сеттер переменной mode
        public static EditorMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        //геттер и сеттер переменной selectThicknessColor
        public static Color SelectedThicknessColor
        {
            get
            {
                return selectedThicknessColor;
            }
            set
            {
                selectedThicknessColor = value;
            }
        }
    }
}