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

        //Режим курсора в виде Enum
        public enum CursorMode
        {
            Select,
            MovePoint,
            MoveFigure,
            Transform
        }


        public static EditorMode Mode = EditorMode.Cursor; //Текущий мод
        public static CursorMode CMode = CursorMode.Select; //Мод курсора
        public static Color EditLineColor = Color.BlueViolet; //Цвет обводки выделенной фигуры/линии
        public static Color EditPointColor = Color.BlueViolet; //Цвет точки для выделения
        public static Color CenterPointColor = Color.BlueViolet; //Цвет центральной точки любого объекта
        public const int Eps = 5; //Радиус, в котором мы будем искать, на какую точку нажал пользователь
    }
}