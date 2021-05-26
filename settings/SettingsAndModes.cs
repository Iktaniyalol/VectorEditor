using System;

namespace VectorEditor.settings
{
    public static class SettingsAndModes
    {
        static bool LineMode;
        static bool CircleMode;
        static bool RectangleMode;

        public static bool IsLineMode
        {
            get
            {
                return LineMode;
            }
            set
            {
                LineMode = value;
            }
        }

        public static bool IsCircleMode
        {
            get
            {
                return CircleMode;
            }
            set
            {
                CircleMode = value;
            }
        }

        public static bool IsRectangleMode
        {
            get
            {
                return RectangleMode;
            }
            set
            {
                RectangleMode = value;
            }
        }
    }
}