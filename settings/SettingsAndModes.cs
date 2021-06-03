using System;

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
            Rectangle
        }

        static EditorMode mode = EditorMode.Cursor; //Текущий мод

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

    }
}