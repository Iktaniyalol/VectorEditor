using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectorEditor.settings;
using VectorEditor.objects;

namespace VectorEditor
{
    public partial class Form1 : Form
    {
        Point MouseBeginPoint; //Сохраняем точку при нажатии мышкой
        Point MouseEndPoint; //Сохраняем точку при отжатии/перемещении мышки
        bool IsMousePressed = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) //Событие нажатия мышкой по PictureBox
        {
            MouseBeginPoint = new Point(e.X, e.Y);
            IsMousePressed = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) //Событие движения мышки по PictureBox
        {
            if (IsMousePressed) //Зажата ли была мышка
            {
                MouseEndPoint = new Point(e.X, e.Y);
                pictureBox1.Invalidate(); //Очищаем PictureBox
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) //Событие отжатия мышки с PictureBox
        {
            if (IsMousePressed) //Зажата ли была мышка
            {
                MouseEndPoint = new Point(e.X, e.Y);
                pictureBox1.Invalidate(); //Очищаем PictureBox
                Line line = new Line(MouseBeginPoint, MouseEndPoint, 1, Color.Black);
                Vector.AddNewFigure(line);
                IsMousePressed = false; //Мы отжали мышку
            }
        }

        //Данное событие вызывается, когда PictureBox перерисовывается.
        // Т.е. когда мы вызываем Invalidate(), форма очищается и вызывается данное событие. Мы отрисовываем фигуру заново, тем самым мы сделали резиновые фигуры.
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (GraphObject figure in Vector.GetAllFigures())
            {
                figure.Draw(g);
            }
            if (IsMousePressed) //Зажата ли была мышка
            {
                g.DrawLine(new Pen(Color.Black, 1), MouseBeginPoint, MouseEndPoint);
            }
        }
    }
}
