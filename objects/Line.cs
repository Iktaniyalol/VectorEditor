using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VectorEditor.objects
{
    class Line
    {
        private long FirstPointIndex;
        private long SecondPointIndex;
        private int thickness;
        
        public Line(MyPoint one, MyPoint two, int thickness)
        {
            this.thickness = thickness;
            this.FirstPointIndex = one.ID;
            this.SecondPointIndex = two.ID;
        }
    }
}
