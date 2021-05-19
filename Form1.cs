using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //ТЕСТ
            Vector.AddP(new MyPoint(1, 1, Vector.IDS++));
            Vector.AddP(new MyPoint(1, 1, Vector.IDS++));
            Vector.AddP(new MyPoint(1, 1, Vector.IDS++));
            Vector.InsertPAfterP(Vector.FindPbyID(1), new MyPoint(1, 1, Vector.IDS++));
            label1.Text = Vector.PointsToString();
            Vector.RemoveP(Vector.FindPbyID(2));
            label2.Text = Vector.PointsToString();
            Vector.RemoveP(Vector.GetPrevP(Vector.FindPbyID(1)));
            label3.Text = Vector.PointsToString();
            Vector.RemoveP(Vector.GetNextP(Vector.FindPbyID(1)));
            label4.Text = Vector.PointsToString();
            Vector.RemoveP(Vector.GetNextP(Vector.FindPbyID(1))); //В этом месте в списке останется только 1 точка, поэтому, данная функция должна не выполниться
        }

    }
}
