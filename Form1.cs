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
        List<GraphObject> CopiedFigures = new List<GraphObject>(); //Список скопированных объектов
        List<GraphObject> SelectedFigures = new List<GraphObject>(); //Список выбранных объектов
        List<Point> PolygonPoints = new List<Point>(); //Точки для создания фигуры
        Color fill = Color.Empty; //Цвет заливки по умолчанию
        Color thickness = Color.Black; //Цвет обводки(контура) по умолчанию
        MyPoint? SelectPoint = null; //Точка, которую мы сейчас выбрали


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
                            //Алгоритм поиска ближайшей точки в месте клика
                            MyPoint? min = null; //По умолчанию точка не найдена
                            double minR = SettingsAndModes.Eps; //Константа
                            foreach (MyPoint point in Vector.VPoints) //Перебираем все точки
                            {
                                if (Math.Sqrt((e.X - point.X) * (e.X - point.X) + (e.Y - point.Y) * (e.Y - point.Y)) < minR) //Сравниваем, близко ли мы нажали к точке
                                {
                                    min = point; //Назначаем точку
                                    minR = Math.Sqrt((e.X - point.X) * (e.X - point.X) + (e.Y - point.Y) * (e.Y - point.Y)); //Пересчитываем минимальное расстояние
                                }
                            }
                            if (min != null) //Если мы нашли точку, на которую нажал пользователь
                            {
                                SelectPoint = min.Value; //Сохраняем ее в переменную
                                GraphObject vobject = SelectPoint.Value.VObject; //Берем фигуру, которой принадлежит точка

                                if (!SelectedFigures.Contains(vobject)) //Если фигура еще не была выбрана
                                {
                                    if (vobject is CustomFigure) //Если фигура, которой принадлежит точка кастомная (многоугольник, треугольник), нужно проверить чтоб нажатая точка не была точкой выделения
                                    {
                                        for (int i = 1; i < 4; i++) //Перебираем точки прямоугольника-выделителя. Если хоть у одной совпадает ID с выбранной точкой - мы не выделяем фигуру
                                        {
                                            //Мы в кастомных фигурах записывали, сколько у них точек. Поэтому, вычитаем 1 и прибавляем i, т.к. точки выделения записываются после точек фигуры, а центр в конце
                                            if (vobject.GetPointsIDs()[((CustomFigure)vobject).PointCount + i] == SelectPoint.Value.ID)
                                            {
                                                SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                                                SelectPoint = null; //очищаем выбранную точку
                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Назначаем обычный режим на курсор
                                                MousePrevPoint = new Point(e.X, e.Y); //Сохраняем точку, в которой мы сделали клик
                                                pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки
                                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                                                          //Убираем толщину и ставим ее значение на стандартное
                                                labelThickness.Visible = false;
                                                thicknessBar.Value = 100;
                                                thicknessBar.Visible = false;
                                                return;
                                            }
                                        }
                                    }
                                    SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                                    SelectedFigures.Add(vobject); //Мы выделили фигуру, нажав на ее точку
                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Назначаем обычный режим на курсор
                                    pictureBox1.Invalidate(); //Очищаем PictureBox
                                    //Отображаем редактирование толщины
                                    thicknessBar.Visible = true;
                                    thicknessBar.Value = (int) (vobject.Thickness * 100); //Ставим такую же толщину как и у выбранной фигуры
                                    labelThickness.Visible = true;
                                    labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                                                  //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                                    //Ставим на кнопки выбора цветов цвета
                                    fill = SelectedFigures[0] is Figure ? ((Figure)SelectedFigures[0]).Color : Color.Empty;  //Записываем цвет заливки, если это не Figure, то цвет заливки будет пустой 
                                    thickness = SelectedFigures[0].ThicknessColor; //Записываем цвет обводки
                                    if (fill.IsEmpty) //Если цвет заливки пустой
                                    {
                                        colorButton1.Image = Properties.Resources.resource__9_; //Красная палочка
                                        colorButton1.BackColor = Color.White;
                                    }
                                    else //иначе
                                    {
                                        colorButton1.BackColor = fill;
                                        colorButton1.Image = null;
                                    }

                                    if (thickness.IsEmpty) //Если цвет обводки пустой
                                    {
                                        colorButton2.Image = Properties.Resources.resource__9_; //Красная палочка
                                        colorButton2.BackColor = Color.White;
                                    }
                                    else //иначе
                                    {
                                        colorButton2.BackColor = thickness;
                                        colorButton2.Image = null;
                                    }
                                }
                                else
                                {
                                    //Ищем, к какому типу относится объект
                                    if (vobject is Line) //Линия
                                    {
                                        for (int i = 0; i < vobject.GetPointsIDs().Length; i++)
                                        {
                                            if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                            {
                                                switch (i)
                                                {
                                                    case 0:
                                                    case 1: //Точки линии, их можно двигать
                                                        {
                                                            SettingsAndModes.CMode = SettingsAndModes.CursorMode.MovePoint; //Назначаем режим движения точек на курсор
                                                            SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты, на них не получится повлиять когда мы двигаем точки у линии
                                                            SelectedFigures.Add(vobject); //Мы выделили фигуру, нажав на ее точку
                                                            break;
                                                        }
                                                    case 2: //Центр линии, можем двигать линию
                                                        {
                                                            SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                            //TODO
                                                            break;
                                                        }
                                                }
                                            }
                                        }
                                    }
                                }
                                pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                            }
                            else
                            {
                                SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                                SelectPoint = null; //очищаем выбранную точку
                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Назначаем обычный режим на курсор
                                MousePrevPoint = new Point(e.X, e.Y); //Сохраняем точку, в которой мы сделали клик
                                pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                //Убираем толщину и ставим ее значение на стандартное
                                labelThickness.Visible = false;
                                thicknessBar.Value = 100;
                                thicknessBar.Visible = false;
                            }

                            break;
                        }
                    case SettingsAndModes.EditorMode.Line:
                    case SettingsAndModes.EditorMode.Circle:
                    case SettingsAndModes.EditorMode.Rectangle:
                        {
                            SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                            MousePrevPoint = new Point(e.X, e.Y); //Сохраняем точку, в которой мы сделали клик
                            pictureBox1.Invalidate(); //Очищаем PictureBox
                            break;
                        }
                    case SettingsAndModes.EditorMode.Polygon: //Построение любого многоугольника
                        {
                            SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                            PolygonPoints.Add(new Point(e.X, e.Y)); //Добавляем точку в массив точек для построения многоугольника
                            pictureBox1.Invalidate(); //Очищаем PictureBox
                            pictureBox1.ContextMenuStrip = null; //Убираем контекст меню при создании многоугольника
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
                        if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                        {
                            pictureBox1.Invalidate(); //Очищаем PictureBox
                        }
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
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                switch (SettingsAndModes.CMode)
                                {
                                    case SettingsAndModes.CursorMode.Select:
                                        {
                                            if (SelectPoint == null) //Если мы не выбирали никакой точки
                                            {
                                                foreach (GraphObject vobject in Vector.GetAllFigures()) //Перебираем все фигуры
                                                {
                                                    int i = 0; //Счетчик точек, мы должны не учитывать точки выделения у кастомных фигур, т.к. они работают только когда фигура уже выделена
                                                    foreach (int ID in vobject.GetPointsIDs()) //Перебираем все точки фигуры
                                                    {
                                                        if (vobject is CustomFigure)
                                                        {
                                                            if (i != vobject.GetPointsIDs().Length - 1 && i >= ((CustomFigure)vobject).PointCount) //Если точка не центр и не составляет фигуру, значит это точка выделения, пропускаем
                                                            {
                                                                i++;
                                                                continue;
                                                            }
                                                        }
                                                        MyPoint? point = Vector.FindPbyID(ID);
                                                        if (!point.HasValue) continue;
                                                        //Условие вхождения точки фигуры в область
                                                        if (point.Value.X <= Math.Max(MousePrevPoint.X, MouseCurrentPoint.X) &&
                                                            point.Value.X >= Math.Min(MousePrevPoint.X, MouseCurrentPoint.X) &&
                                                            point.Value.Y <= Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y) &&
                                                                 point.Value.Y >= Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y))
                                                        {
                                                            SelectedFigures.Add(vobject); //Выделяем объект
                                                            pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                                                            break;
                                                        }
                                                        i++;
                                                    }
                                                }
                                                if (SelectedFigures.Count > 0) {
                                                    bool IsSameColor = true, IsSameThickColor = true, IsSameThick = true; //Сверяем, одинаковые ли цвета и толщина у фигур
                                                    Color fill = SelectedFigures[0] is Figure ? ((Figure)SelectedFigures[0]).Color : Color.Empty, thickColor = SelectedFigures[0].ThicknessColor; //Записываем цвет заливки и цвет обводки, если это не Figure, то цвет заливки будет пустой 
                                                    float thick = SelectedFigures[0].Thickness;
                                                    foreach (GraphObject vobject in SelectedFigures) //Перебираем выбранные фигуры
                                                    {
                                                        IsSameColor = fill == (vobject is Figure ? ((Figure)SelectedFigures[0]).Color : Color.Empty); //Цвета заливки, если объект не Figure, то цвет заливки будет пустой 
                                                        IsSameThick = thick == vobject.Thickness; //Толщина обводки
                                                        IsSameThickColor = thickColor == vobject.ThicknessColor; //Цвет обводки
                                                    }
                                                    if (IsSameColor) this.fill = fill; //Если у всех фигур один и тот же цвет заливки, назначаем его в переменную текущего выбранного первого цвета
                                                    else this.fill = Color.Empty; //Цвет заливки не выбран
                                                    if (IsSameThickColor) this.thickness = thickColor; //Если у всех фигур один и тот же цвет обводки, назначаем его в переменную текущего выбранного второго цвета
                                                    else this.thickness = Color.Empty; //Цвет заливки не выбран
                                                    if (fill.IsEmpty) //Если цвет заливки пустой
                                                    {
                                                        colorButton1.Image = Properties.Resources.resource__9_; //Красная палочка
                                                        colorButton1.BackColor = Color.White;
                                                    }
                                                    else //иначе
                                                    {
                                                        colorButton1.BackColor = fill;
                                                        colorButton1.Image = null;
                                                    }

                                                    if (thickness.IsEmpty) //Если цвет обводки пустой
                                                    {
                                                        colorButton2.Image = Properties.Resources.resource__9_; //Красная палочка
                                                        colorButton2.BackColor = Color.White;
                                                    }
                                                    else //иначе
                                                    {
                                                        colorButton2.BackColor = thickness;
                                                        colorButton2.Image = null;
                                                    }
                                                    //Отображаем редактирование толщины
                                                    thicknessBar.Visible = true;
                                                    if (IsSameThick)
                                                    {
                                                        thicknessBar.Value = (int)(thick * 100); //Ставим такую же толщину как и у выбранной фигуры
                                                        labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                                                                      //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
                                                    }
                                                    else
                                                    {
                                                        thicknessBar.Value = 100; //Стандартная толщина
                                                        labelThickness.Text = "Обводка: ?"; //Отображаем вопрос, т.к. общей толщины у фигур мы не нашли
                                                    }
                                                    labelThickness.Visible = true;
                                                } 
                                            }
                                            break;
                                        }
                                }
                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Возвращаем стандартный мод назад
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Line:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                Line line = new Line(thicknessBar.Value / 100f, thickness, new Point[] { MousePrevPoint, MouseCurrentPoint, new Point(((MousePrevPoint.X + MouseCurrentPoint.X) / 2), ((MousePrevPoint.Y + MouseCurrentPoint.Y) / 2)) }); //Создаем экземпляр класса Line, передаем точки
                                Vector.AddNewFigure(line);
                                SelectedFigures.Add(line); //Когда мы создаем векторный объект, он сразу становится "выбранным"
                                pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Circle:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                VEllipse ellipse = new VEllipse(thicknessBar.Value / 100f, thickness, fill, new Point[] { new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(((MousePrevPoint.X + MouseCurrentPoint.X) / 2), ((MousePrevPoint.Y + MouseCurrentPoint.Y) / 2)) }); //Создаем экземпляр класса VEllipse, передаем точки
                                Vector.AddNewFigure(ellipse);
                                SelectedFigures.Add(ellipse); //Когда мы создаем векторный объект, он сразу становится "выбранным"
                                pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                            }
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle:
                        {
                            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
                            {
                                MouseCurrentPoint = new Point(e.X, e.Y);
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                                VRectangle rect = new VRectangle(thicknessBar.Value / 100f, thickness, fill, new Point[] { new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)), new Point(((MousePrevPoint.X + MouseCurrentPoint.X) / 2), ((MousePrevPoint.Y + MouseCurrentPoint.Y) / 2)) }); //Создаем экземпляр класса VRectangle, передаем точки
                                Vector.AddNewFigure(rect);
                                SelectedFigures.Add(rect); //Когда мы создаем векторный объект, он сразу становится "выбранным"
                                pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                            }
                            break;
                        }
                }
                IsLeftMousePressed = false; //Мы отжали левую клавишу мышки
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
                                SelectedFigures.Add(triangle); //Когда мы создаем векторный объект, он сразу становится "выбранным"
                                PolygonPoints.Clear(); //Очищаем точки создания многоугольника
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                            }
                            else //многоугольник
                            {
                                VPolygone polygone = new VPolygone(thicknessBar.Value / 100f, thickness, fill, PolygonPoints.ToArray()); //Создаем экземпляр класса VPolygone, передаем точки
                                Vector.AddNewFigure(polygone);
                                SelectedFigures.Add(polygone); //Когда мы создаем векторный объект, он сразу становится "выбранным"
                                PolygonPoints.Clear(); //Очищаем точки создания многоугольника
                                pictureBox1.Invalidate(); //Очищаем PictureBox
                            }
                            pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                            break;
                        }
                }
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
                if (SelectedFigures.Contains(figure)) figure.Select(g);
            }
            if (IsLeftMousePressed) //Зажата ли была левая клавиша мышки
            {
                switch (SettingsAndModes.Mode)
                {
                    case SettingsAndModes.EditorMode.Line: //Мод построения линии
                        {
                            g.DrawLine(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), MousePrevPoint, MouseCurrentPoint);
                            break;
                        }
                    case SettingsAndModes.EditorMode.Circle: //Мод построения эллипса
                        {
                            g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle: //Мод построения прямоугольника
                        {
                            g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                            break;
                        }
                    case SettingsAndModes.EditorMode.Cursor: //Мод курсора
                        {
                            switch (SettingsAndModes.CMode)
                            {
                                case SettingsAndModes.CursorMode.Select:
                                    {
                                        if (SelectPoint != null) return; //Если мы выбирали точку, не рисуем
                                        g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                                        break;
                                    }
                                case SettingsAndModes.CursorMode.Transform:
                                    {
                                        GraphObject figure = SelectPoint.Value.VObject; //Берем из точки объект, которому она принадлежит
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            if (SettingsAndModes.Mode == SettingsAndModes.EditorMode.Polygon) //Мод построения любого многоугольника
            {
                if (PolygonPoints.Count == 0) return; //У нас еще не записаны точки для создания многоугольника
                for (int i = 0; i < PolygonPoints.Count - 1; i++) //Перебираем все точки
                {
                    g.DrawLine(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), PolygonPoints[i], PolygonPoints[i + 1]);
                    g.FillEllipse(new SolidBrush(SettingsAndModes.EditPointColor), PolygonPoints[i].X - 2, PolygonPoints[i].Y - 2, 5, 5); //Строим точку диаметром 10
                }
                g.DrawLine(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), PolygonPoints.Last(), MouseCurrentPoint); //Резиновая линия для демонстрации, как будет выглядеть линия, если поставить точку здесь
                g.FillEllipse(new SolidBrush(SettingsAndModes.EditPointColor), PolygonPoints.Last().X - 2, PolygonPoints.Last().Y - 2, 5, 5); //Показываем последнюю точку, причем поверх "резиновой" линии для красоты                                                                                                                                           }
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
                    foreach (GraphObject figure in SelectedFigures)
                    {
                        if (!(figure is Figure)) continue; //Наш векторный объект не является фигурой, значит ему нельзя назначить заливку
                        ((Figure)figure).Color = fill; //Меняем цвет заливки у выбранных объектов, приводим тип GraphObject к Figure
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
                    foreach (GraphObject figure in SelectedFigures)
                    {
                        figure.ThicknessColor = thickness; //Меняем цвет контура у выбранных объектов
                    }
                    pictureBox1.Invalidate(); //Очищаем PictureBox
                }
            }
        }

        private void buttonCursor_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Cursor; //Включаем Курсор мод
            if (SelectedFigures.Count == 0) //Если у нас не выбраны объекты, выключаем отображение толщины
            {
                thicknessBar.Visible = false;
                labelThickness.Visible = false;
            }
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
            if (pictureBox1.ContextMenuStrip == null) pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки если оно было убрано
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Line; //Включаем Лайн мод
            labelThickness.Visible = true;
            labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                  //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
            if (pictureBox1.ContextMenuStrip == null) pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки если оно было убрано
        }

        private void buttonRectangle_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Rectangle; //Включаем мод прямоугольников
            labelThickness.Visible = true;
            labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                          //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
            if (pictureBox1.ContextMenuStrip == null) pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки если оно было убрано
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Circle; //Включаем мод окружности
            labelThickness.Visible = true;
            labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                          //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
            if (pictureBox1.ContextMenuStrip == null) pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки если оно было убрано
        }

        private void buttonPolygon_Click(object sender, EventArgs e)
        {
            SettingsAndModes.Mode = SettingsAndModes.EditorMode.Polygon; //Включаем мод построения многоугольника
            labelThickness.Visible = true;
            labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                          //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            thicknessBar.Visible = true; //Включаем отображение толщины
            PolygonPoints.Clear(); //Очищаем точки создания многоугольника
            pictureBox1.Invalidate(); //Очищаем PictureBox
            if (pictureBox1.ContextMenuStrip == null) pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки если оно было убрано
        }

        private void thicknessBar_Scroll(object sender, EventArgs e) //Ловим изменение значения в TrackBar
        {
            labelThickness.Text = "Обводка: " + thicknessBar.Value / 100f + "px"; //Отображаем выбранную толщину,
                                                                                  //обязательно делим на 100, т.к. в TrackBar могут храниться только целые числа
            if (SelectedFigures.Count != 0) //Если у нас выбраны объекты
            {
                foreach (GraphObject figure in SelectedFigures)
                {
                    figure.Thickness = thicknessBar.Value / 100f; //Меняем толщину обводки у выбранных объектов
                }
                pictureBox1.Invalidate(); //Очищаем PictureBox
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
                    foreach (GraphObject figure in SelectedFigures)
                    {
                        if (!(figure is Figure)) return; //Наш векторный объект не является фигурой, значит ему нельзя назначить заливку
                        ((Figure)figure).Color = fill; //Меняем цвет заливки у выбранных объектов, приводим тип GraphObject к Figure
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
                    foreach (GraphObject figure in SelectedFigures)
                    {
                        figure.ThicknessColor = thickness; //Меняем цвет контура у выбранных объектов
                    }
                    pictureBox1.Invalidate(); //Очищаем PictureBox
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e) //Удаление выделенных фигур
        {
            if (SelectedFigures.Count == 0) return; //У нас не выбраны объекты
            foreach (GraphObject vobject in SelectedFigures)
            {
                vobject.Remove(); //Удаление фигуры вместе с точками
                CopiedFigures.Remove(vobject); //Удаление из скопированных объектов
            }
            SelectedFigures.Clear(); //Удаление выбранных объектов
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void Copy_Click(object sender, EventArgs e) //Копирование выделенных фигур
        {
            if (SelectedFigures.Count == 0) return; //У нас не выбраны объекты
            CopiedFigures.Clear(); //Очищаем список скопированных объектов
            CopiedFigures.AddRange(SelectedFigures); //Добавляем выбранные объекты в скопированные
        }

        private void Paste_Click(object sender, EventArgs e) //Вставка скопированных фигур
        {
            if (CopiedFigures.Count == 0) return; //У нас нет скопированных объектов
            SelectedFigures.Clear(); //Удаление выбранных объектов
            MyPoint? center = Vector.FindPbyID(CopiedFigures[0].GetPointsIDs()[CopiedFigures[0].GetPointsIDs().Length - 1]); //Ищем центр 1 объекта 
            int dx = MouseCurrentPoint.X - center.Value.X; //смещение по x
            int dy = MouseCurrentPoint.Y - center.Value.Y; //смещение по y
            foreach (GraphObject vobject in CopiedFigures) //Перебираем скопированные объекты
            {
                GraphObject clone = vobject.Clone(dx, dy); //Клонируем, передается смещение по x и y
                if (clone == null) return;
                Vector.AddNewFigure(clone); //Добавляем объект в список объектов
                SelectedFigures.Add(clone); //Автоматически выбираем скопированный объект

            }
            pictureBox1.Invalidate(); //Очищаем PictureBox
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete: //Удаление выделенных фигур
                    {
                        if (SelectedFigures.Count == 0) return; //У нас не выбраны объекты
                        foreach (GraphObject vobject in SelectedFigures)
                        {
                            vobject.Remove(); //Удаление фигуры вместе с точками
                            CopiedFigures.Remove(vobject); //Удаление из скопированных объектов
                        }
                        SelectedFigures.Clear(); //Удаление выбранных объектов
                        pictureBox1.Invalidate(); //Очищаем PictureBox
                        break;
                    }
                case Keys.V: //Вставка скопированных фигур
                    {
                        if (CopiedFigures.Count == 0) return; //У нас нет скопированных объектов
                        SelectedFigures.Clear(); //Удаление выбранных объектов
                        MyPoint? center = Vector.FindPbyID(CopiedFigures[0].GetPointsIDs()[CopiedFigures[0].GetPointsIDs().Length - 1]); //Ищем центр 1 объекта 
                        int dx = MouseCurrentPoint.X - center.Value.X; //смещение по x
                        int dy = MouseCurrentPoint.Y - center.Value.Y; //смещение по y
                        foreach (GraphObject vobject in CopiedFigures) //Перебираем скопированные объекты
                        {
                            GraphObject clone = vobject.Clone(dx, dy); //Клонируем, передается смещение по x и y
                            if (clone == null) return;
                            Vector.AddNewFigure(clone); //Добавляем объект в список объектов
                            SelectedFigures.Add(clone); //Автоматически выбираем скопированный объект

                        }
                        pictureBox1.Invalidate(); //Очищаем PictureBox
                        break;
                    }
                case Keys.C: //Копирование выделенных фигур
                    {
                        if (!e.Control) return;
                        if (SelectedFigures.Count == 0) return; //У нас не выбраны объекты
                        CopiedFigures.AddRange(SelectedFigures); //Добавляем выбранные объекты в скопированные
                        break;
                    }
            }
        }
    }
}
