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
        Point MousePrevPoint; //Сохраняем точку при нажатии мышкой
        Point MouseCurrentPoint; //Сохраняем точку при отжатии/перемещении мышки
        bool IsLeftMousePressed = false; //Зажата ли клавиша мышки
        List<GraphObject> SelectedFigures = new List<GraphObject>(); //Список созданных фигур
        List<Point> PolygonPoints = new List<Point>(); //Точки для создания фигуры
        Color fill = Color.Empty; //Цвет заливки по умолчанию
        Color thickness = Color.Black; //Цвет обводки(контура) по умолчанию

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //При загрузке формы
        {
            if (fill.IsEmpty) //Если цвет заливки пустой
            {
                colorButton1.Image = Properties.Resources.resource__9_; //Красная палочка
            } else //иначе
            {
                colorButton1.BackColor = fill;
            }

            if (thickness.IsEmpty) //Если цвет обводки пустой
            {
                colorButton2.Image = Properties.Resources.resource__9_; //Красная палочка
            }
            else //иначе
            {
                colorButton2.BackColor = thickness;
            }
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
                            MousePrevPoint = new Point(e.X, e.Y);
                            break;
                        }
                    case SettingsAndModes.EditorMode.Polygon: //Построение любого многоугольника
                        {
                            PolygonPoints.Add(new Point(e.X, e.Y)); //Добавляем точку в массив точек для построения многоугольника
                            pictureBox1.Invalidate(); //Очищаем PictureBox
                            break;
                        }
                }
                IsLeftMousePressed = true; //Мы зажали левую клавишу мышки
            }
            if (e.Button.Equals(MouseButtons.Right)) //Проверяем клавишу мышки
            {
                switch (SettingsAndModes.Mode)
                {
                    case SettingsAndModes.EditorMode.Polygon: //Создание многоугольника
                        {
                            if (PolygonPoints.Count < 3) return; //Если у нас меньше 3 точек, это не многоугольник
                            if (PolygonPoints.Count == 3) //Если у нас 3 точки, то это треугольник
                            {
                                VTriangle triangle = new VTriangle(thicknessBar.Value / 100f, thickness, fill, PolygonPoints.ToArray()); //Создаем экземпляр класса VTriangle, передаем точки
                                Vector.AddNewFigure(triangle);
                                PolygonPoints.Clear(); //Очищаем точки создания многоугольника
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                            } else //многоугольник
                            {
                                VPolygone polygone = new VPolygone(thicknessBar.Value / 100f, thickness, fill, PolygonPoints.ToArray()); //Создаем экземпляр класса VPolygone, передаем точки
                                Vector.AddNewFigure(polygone);
                                PolygonPoints.Clear(); //Очищаем точки создания многоугольника
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                            }
                            break;
                        }
                }
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
                            pictureBox1.Invalidate(); //Очищаем PictureBox
                        }
                        break;
                    }
                case SettingsAndModes.EditorMode.Polygon:
                    {
                        if (PolygonPoints.Count != 0) //Если мы поставили уже хотя бы одну точку создаваемого многоугольника
                        {
                            pictureBox1.Invalidate(); //Очищаем PictureBox

                        }
                        break;
                    }
            }
            MouseCurrentPoint = new Point(e.X, e.Y);
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
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                Line line = new Line(thicknessBar.Value / 100f, thickness, new Point[] { MousePrevPoint, MouseCurrentPoint }); //Создаем экземпляр класса Line, передаем точки
                                Vector.AddNewFigure(line);
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Circle:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                VEllipse ellipse = new VEllipse(thicknessBar.Value / 100f, thickness, fill, new Point[] { MousePrevPoint, MouseCurrentPoint }); //Создаем экземпляр класса VRectangle, передаем точки
                                Vector.AddNewFigure(ellipse);
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                VRectangle rect = new VRectangle(thicknessBar.Value / 100f, thickness, fill, new Point[] { MousePrevPoint, MouseCurrentPoint }); //Создаем экземпляр класса VRectangle, передаем точки
                                Vector.AddNewFigure(rect);
                            }
                            break;
                        }
                }
                IsLeftMousePressed = false; //Мы отжали левую клавишу мышки
            }
        }

        //Данное событие вызывается, когда PictureBox перерисовывается.
        // Т.е. когда мы вызываем Invalidate(), форма очищается и вызывается данное событие. Мы отрисовываем объект заново, тем самым мы сделали "резиновые" объекты.
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
                    case SettingsAndModes.EditorMode.Line: //Мод построения эллипса
                        {
                            g.DrawLine(new Pen(SettingsAndModes.SelectedThicknessColor, thicknessBar.Value / 100f), MousePrevPoint, MouseCurrentPoint);
                            break;
                        }
                    case SettingsAndModes.EditorMode.Circle: //Мод построения эллипса
                        {
                            g.DrawEllipse(new Pen(SettingsAndModes.SelectedThicknessColor, thicknessBar.Value / 100f), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle: //Мод построения прямоугольника
                        {
                            g.DrawRectangle(new Pen(SettingsAndModes.SelectedThicknessColor, thicknessBar.Value / 100f), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                            break;
                        }
                }
            }
            if (SettingsAndModes.Mode == SettingsAndModes.EditorMode.Polygon) //Мод построения любого многоугольника
            {
                if (PolygonPoints.Count == 0) return; //У нас еще не записаны точки для создания многоугольника
                for (int i = 0; i < PolygonPoints.Count - 1; i++) //Перебираем все точки
                {
                    g.DrawLine(new Pen(SettingsAndModes.SelectedThicknessColor, thicknessBar.Value / 100f), PolygonPoints[i], PolygonPoints[i + 1]);
                    g.FillEllipse(new SolidBrush(Color.Red), PolygonPoints[i].X - 5, PolygonPoints[i].Y - 5, 10, 10); //Строим точку диаметром 10
                }
                g.FillEllipse(new SolidBrush(Color.Red), PolygonPoints.Last().X - 5, PolygonPoints.Last().Y - 5, 10, 10); //Строим точку диаметром 10
                g.DrawLine(new Pen(SettingsAndModes.SelectedThicknessColor, thicknessBar.Value / 100f), PolygonPoints.Last(), MouseCurrentPoint);
            }
        }

        private void colorButton1_Click(object sender, EventArgs e) //Нажимаем на кнопку основного цвета
        {
            ColorDialog MyDialog = new ColorDialog(); //Диалоговое окно выбора цвета
            MyDialog.AllowFullOpen = true; //Разрешаем определять любой цвет
            MyDialog.ShowHelp = false;
            MyDialog.Color = fill; //Какой цвет записан в переменную fill, такой назначаем в диалоговом окне

            if (MyDialog.ShowDialog() == DialogResult.OK) //Выбрали цвет и нажали ок
            {
                //Назначаем цвет
                fill = MyDialog.Color;
                colorButton1.BackColor = fill;
                colorButton1.Image = null; //Убираем рисунок, означающий что цвет не выбран
                if (SelectedFigures.Count != 0) //Если у нас выбраны объекты
                {
                    foreach (Figure figure in SelectedFigures)
                    {
                        figure.Color = fill; //Меняем цвет заливки у выбранных объектов
                    }
                    pictureBox1.Invalidate(); //Очищаем PictureBox
                }
            }
        }

        private void colorButton2_Click(object sender, EventArgs e) //Нажимаем на кнопку побочного цвета
        {
            ColorDialog MyDialog = new ColorDialog(); //Диалоговое окно выбора цвета
            MyDialog.AllowFullOpen = true; //Разрешаем определять любой цвет
            MyDialog.ShowHelp = false;
            MyDialog.Color = thickness; //Какой цвет записан в переменную thickness, такой назначаем в диалоговом окне

            if (MyDialog.ShowDialog() == DialogResult.OK) //Выбрали цвет и нажали ок
            {
                //Назначаем цвет
                thickness = MyDialog.Color;
                colorButton2.BackColor = thickness; //Назначаем цвет
                colorButton2.Image = null; //Убираем рисунок, означающий что цвет не выбран
                if (SelectedFigures.Count != 0) //Если у нас выбраны объекты
                {
                    foreach (Figure figure in SelectedFigures)
                    {
                        figure.Color = thickness; //Меняем цвет контура у выбранных объектов
                    }
                    pictureBox1.Invalidate(); //Очищаем PictureBox
                }
            }
        }

        private void buttonCursor_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Cursor; //Включаем Курсор мод
            thicknessBar.Visible = false; //Выключаем отображение толщины
            labelThickness.Visible = false;
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Line; //Включаем Лайн мод
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                           //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void buttonRectangle_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Rectangle; //Включаем мод прямоугольников
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина обводки: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                   //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Circle; //Включаем мод окружности
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина обводки: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                   //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void buttonPolygon_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Polygon; //Включаем мод построения многоугольника
            labelThickness.Visible = true;
            labelThickness.Text = "Толщина обводки: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                          //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void thicknessBar_Scroll(object sender, EventArgs e) //Ловим изменение значения в TrackBar
        {
            switch (SettingsAndModes.Mode)
            {
                case SettingsAndModes.EditorMode.Line:
                    {
                        labelThickness.Text = "Толщина:" + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                      //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        break;
                    }
                case SettingsAndModes.EditorMode.Circle:
                    {
                        labelThickness.Text = "Толщина обводки:" + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                              //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        break;
                    }
                case SettingsAndModes.EditorMode.Rectangle:
                    {
                        labelThickness.Text = "Толщина обводки:" + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                              //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        break;
                    }
                case SettingsAndModes.EditorMode.Polygon:
                    {
                        labelThickness.Text = "Толщина обводки:" + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                                     //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                        pictureBox1.Invalidate(); //Очищаем PictureBox
                        break;
                    }
            }
        }

        private void colorButton1_MouseDown(object sender, MouseEventArgs e) //Ловим пкм нажатие по кнопке выбора цвета заливки
        {
            if (e.Button.Equals(MouseButtons.Right)) //пкм, очищаем выбранный цвет
            {
                colorButton1.BackColor = Color.White;
                colorButton1.Image = Properties.Resources.resource__9_;
                fill = Color.Empty;
                if (SelectedFigures.Count != 0) //Если у нас выбраны объекты
                {
                    foreach (Figure figure in SelectedFigures)
                    {
                        figure.Color = fill; //Меняем цвет заливки у выбранных объектов
                    }
                    pictureBox1.Invalidate(); //Очищаем PictureBox
                }
            }
        }

        private void colorButton2_MouseDown(object sender, MouseEventArgs e) //Ловим пкм нажатие по кнопке выбора цвета обводки
        {
            if (e.Button.Equals(MouseButtons.Right)) //пкм, очищаем выбранный цвет
            {
                colorButton2.BackColor = Color.White;
                colorButton2.Image = Properties.Resources.resource__9_;
                thickness = Color.Empty;

                if (SelectedFigures.Count != 0) //Если у нас выбраны объекты
                {
                    foreach (Figure figure in SelectedFigures)
                    {
                        figure.Color = thickness; //Меняем цвет контура у выбранных объектов
                    }
                    pictureBox1.Invalidate(); //Очищаем PictureBox
                }
            }
        }
    }
}
