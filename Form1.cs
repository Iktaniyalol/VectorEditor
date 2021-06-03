using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using VectorEditor.settings;
using VectorEditor.objects;

namespace VectorEditor
{
    public partial class Form1 : Form
    {
        Point MouseBeginPoint; //Сохраняем точку при нажатии мышкой
        Point MouseEndPoint; //Сохраняем точку при отжатии/перемещении мышки
        bool IsLeftMousePressed = false; //Зажата ли клавиша мышки

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //При загрузке формы
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) //Событие нажатия мышкой по PictureBox
        {
            if (e.Button.Equals(MouseButtons.Left)) //Проверяем клавишу мышки
            {
                switch (SettingsAndModes.Mode)
                {
                    case SettingsAndModes.EditorMode.Cursor:
                        {
                            //TODO
                            break;
                        }
                    case SettingsAndModes.EditorMode.Line:
                    case SettingsAndModes.EditorMode.Circle:
                    case SettingsAndModes.EditorMode.Rectangle:
                        {
                            MouseBeginPoint = new Point(e.X, e.Y);
                            break;
                        }
                }
                IsLeftMousePressed = true; //Мы зажали левую клавишу мышки
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) //Событие движения мышки по PictureBox
        {
            switch (SettingsAndModes.Mode)
            {
                case SettingsAndModes.EditorMode.Cursor:
                    {
                        //TODO
                        break;
                    }
                case SettingsAndModes.EditorMode.Line:
                case SettingsAndModes.EditorMode.Circle:
                case SettingsAndModes.EditorMode.Rectangle:
                    {
                        if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                        {
                            MouseEndPoint = new Point(e.X, e.Y);
                            pictureBox1.Invalidate(); //Очищаем PictureBox
                        }
                        break;
                    }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) //Событие отжатия мышки с PictureBox
        {
            if (e.Button.Equals(MouseButtons.Left))
            { //Проверяем клавишу мышки
                switch (SettingsAndModes.Mode)
                {
                    case SettingsAndModes.EditorMode.Cursor:
                        {
                            //TODO
                            break;
                        }
                    case SettingsAndModes.EditorMode.Line:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseEndPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                Line line = new Line(thicknessBar.Value / 100f, colorButton2.BackColor, new List<Point>(new Point[] { MouseBeginPoint, MouseEndPoint })); //Создаем экземпляр класса Line, передаем точки
                                Vector.AddNewFigure(line);
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Circle:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                //TODO
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                //TODO
                            }
                            break;
                        }
                }
                IsLeftMousePressed = false; //Мы отжали левую клавишу мышки
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
            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
            {
                switch (SettingsAndModes.Mode)
                {
                    case SettingsAndModes.EditorMode.Line:
                        {
                            g.DrawLine(new Pen(colorButton2.BackColor, thicknessBar.Value / 100f), MouseBeginPoint, MouseEndPoint);
                            break;
                        }
                    case SettingsAndModes.EditorMode.Circle:
                        {
                            //TODO
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle:
                        {
                            //TODO
                            break;
                        }
                }
            }
        }

        private void colorButton1_Click(object sender, EventArgs e) //Нажимаем на кнопку основного цвета
        {
            ColorDialog MyDialog = new ColorDialog(); //Диалоговое окно выбора цвета
            MyDialog.AllowFullOpen = true; //Разрешаем определять любой цвет
            MyDialog.ShowHelp = false;
            MyDialog.Color = colorButton1.BackColor; //Какой цвет выбран на кнопке, такой назначаем в диалоговом окне

            if (MyDialog.ShowDialog() == DialogResult.OK) //Выбрали цвет и нажали ок
                colorButton1.BackColor = MyDialog.Color; //Назначаем цвет
        }

        private void colorButton2_Click(object sender, EventArgs e) //Нажимаем на кнопку побочного цвета
        {
            ColorDialog MyDialog = new ColorDialog(); //Диалоговое окно выбора цвета
            MyDialog.AllowFullOpen = true; //Разрешаем определять любой цвет
            MyDialog.ShowHelp = false;
            MyDialog.Color = colorButton2.BackColor; //Какой цвет выбран на кнопке, такой назначаем в диалоговом окне

            if (MyDialog.ShowDialog() == DialogResult.OK) //Выбрали цвет и нажали ок
                colorButton2.BackColor = MyDialog.Color; //Назначаем цвет
        }

        private void buttonCursor_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Cursor; //Включаем Курсор мод
            colorButton1.Enabled = false;  //Выключаем кнопки выбора цвета
            colorButton2.Enabled = false;
            thicknessBar.Visible = false; //Выключаем отображение толщины
            labelThickness.Visible = false;
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Line; //Включаем Лайн мод
            colorButton1.Enabled = false; //У линии нет заливки
            colorButton2.Enabled = true; //Включаем выбор цвета обводки
            thicknessBar.Visible = true; //Включаем отображение толщины
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина: " + thicknessBar.Value / 100f; //Отображаем выбранную толщину,
                                                                           //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
        }

        private void buttonRectangle_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Rectangle; //Включаем мод прямоугольников
            colorButton1.Enabled = true; //Включаем цвета
            colorButton2.Enabled = true;
            thicknessBar.Visible = true; //Включаем отображение толщины
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина обводки: " + thicknessBar.Value / 100f; //Отображаем выбранную толщину,
                                                                                   //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Circle; //Включаем мод окружности
            colorButton1.Enabled = true; //Включаем цвета
            colorButton2.Enabled = true;
            thicknessBar.Visible = true; //Включаем отображение толщины
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина обводки: " + thicknessBar.Value / 100f; //Отображаем выбранную толщину,
                                                                                   //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
        }

        private void thicknessBar_Scroll(object sender, EventArgs e) //Ловим изменение значения в TrackBar
        {
            switch (SettingsAndModes.Mode)
            {
                case SettingsAndModes.EditorMode.Line:
                    {
                        labelThickness.Text = "Толщина:" + thicknessBar.Value / 100f; //Отображаем выбранную толщину,
                                                                                      //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        break;
                    }
                case SettingsAndModes.EditorMode.Circle:
                    {
                        labelThickness.Text = "Толщина обводки:" + thicknessBar.Value / 100f; //Отображаем выбранную толщину,
                                                                                              //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        break;
                    }
                case SettingsAndModes.EditorMode.Rectangle:
                    {
                        labelThickness.Text = "Толщина обводки:" + thicknessBar.Value / 100f; //Отображаем выбранную толщину,
                                                                                              //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        break;
                    }
            }
        }
    }
}
