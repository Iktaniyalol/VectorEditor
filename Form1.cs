using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VectorEditor.objects;
using VectorEditor.settings;

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
        Point? SelectedAreaPoint = null; //Выбранная точка отдельного прямоугольника выделителя
        Point[] SelectAreaPoints = new Point[] { }; //Если мы выделяем несколько фигур, создается отдельный прямоугольник выделитель. Мы должны сохранить его точки
        List<PointF> RelativelyCoords = new List<PointF>(); //Относительные координаты от минимального угла прямоугольника-выделителя

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
                            double minR = SettingsAndModes.Eps; //Константа
                            foreach (Point point in SelectAreaPoints) //Перебираем точки выделения если они есть
                            {
                                if (Math.Sqrt((e.X - point.X) * (e.X - point.X) + (e.Y - point.Y) * (e.Y - point.Y)) < minR) //Сравниваем, близко ли мы нажали к точке
                                {
                                    SelectedAreaPoint = point; //Назначаем точку
                                    minR = Math.Sqrt((e.X - point.X) * (e.X - point.X) + (e.Y - point.Y) * (e.Y - point.Y)); //Пересчитываем минимальное расстояние
                                }
                            }
                            if (SelectedAreaPoint != null)
                            {
                                RelativelyCoords.Clear(); //Очищаем список
                                Point p0 = SelectAreaPoints[0]; //Минимальный угол прямоугольника-выделителя
                                Point p1 = SelectAreaPoints[1]; //Максимальный угол прямоугольника-выделителя
                                float width = p1.X - p0.X, height = p1.Y - p0.Y;
                                foreach (GraphObject vobject in SelectedFigures)
                                {
                                    if (vobject is Line)
                                    {
                                        for (int k = 0; k < vobject.GetPointsIDs().Length - 1; k++)
                                        { // Берем длину на 1 меньше, т.к. мы работаем с точками линии
                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка линии
                                            if (p == null) return; //Если нет точки, ничего не делаем
                                            RelativelyCoords.Add(new PointF((p.Value.X - p0.X) / width, (p.Value.Y - p0.Y) / height));
                                        }
                                    }
                                    else if (vobject is VRectangle)
                                    {
                                        for (int k = 0; k < vobject.GetPointsIDs().Length - 1; k++)
                                        { // Берем длину на 1 меньше, т.к. мы работаем с точками прямоугольника
                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка прямоугольника
                                            if (p == null) return; //Если нет точки, ничего не делаем
                                            RelativelyCoords.Add(new PointF((p.Value.X - p0.X) / width, (p.Value.Y - p0.Y) / height));
                                        }
                                    }
                                    else if (vobject is VEllipse)
                                    {
                                        for (int k = 4; k < vobject.GetPointsIDs().Length - 1; k++)
                                        { // Берем длину на 1 меньше, т.к. мы работаем с точками выделителями эллипса
                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка выделителя эллипса
                                            if (p == null) return; //Если нет точки, ничего не делаем
                                            RelativelyCoords.Add(new PointF((p.Value.X - p0.X) / width, (p.Value.Y - p0.Y) / height));
                                        }
                                    }
                                    else if (vobject is VTriangle)
                                    {
                                        for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                        { // Берем длину на 5 меньше, т.к. мы работаем с точками треугольника
                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка треугольника
                                            if (p == null) return; //Если нет точки, ничего не делаем
                                            RelativelyCoords.Add(new PointF((p.Value.X - p0.X) / width, (p.Value.Y - p0.Y) / height));
                                        }
                                    }
                                    else if (vobject is VPolygone)
                                    {
                                        for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                        { // Берем длину на 5 меньше, т.к. мы работаем с точками многоугольника
                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка многоугольника
                                            if (p == null) return; //Если нет точки, ничего не делаем
                                            RelativelyCoords.Add(new PointF((p.Value.X - p0.X) / width, (p.Value.Y - p0.Y) / height));
                                        }
                                    }
                                }
                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Transform; //Назначаем режим масштабирования на курсор
                            }
                            else
                            {
                                //Алгоритм поиска ближайшей точки в месте клика
                                MyPoint? min = null; //По умолчанию точка не найдена
                                minR = SettingsAndModes.Eps; //Константа
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
                                            for (int i = ((CustomFigure)vobject).PointCount; i < ((CustomFigure)vobject).PointCount + 4; i++) //Перебираем точки прямоугольника-выделителя. Если хоть у одной совпадает ID с выбранной точкой - мы не выделяем фигуру
                                            {
                                                //Мы в кастомных фигурах записывали, сколько у них точек. Поэтому, вычитаем 1 и прибавляем i, т.к. точки выделения записываются после точек фигуры, а центр в конце
                                                if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        else if (vobject is VEllipse) //Если фигура, которой принадлежит точка эллипс, нужно проверить чтоб нажатая точка не была точкой выделения
                                        {
                                            for (int i = 4; i < 8; i++) //Перебираем точки прямоугольника-выделителя. Если хоть у одной совпадает ID с выбранной точкой - мы не выделяем фигуру
                                            {
                                                //Мы в кастомных фигурах записывали, сколько у них точек. Поэтому, вычитаем 1 и прибавляем i, т.к. точки выделения записываются после точек фигуры, а центр в конце
                                                if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                                        SelectedFigures.Add(vobject); //Мы выделили фигуру, нажав на ее точку
                                        SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Назначаем обычный режим на курсор
                                        pictureBox1.Invalidate(); //Очищаем PictureBox
                                                                  //Отображаем редактирование толщины
                                        thicknessBar.Visible = true;
                                        thicknessBar.Value = (int)(vobject.Thickness * 100); //Ставим такую же толщину как и у выбранной фигуры
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
                                                                if (SelectedFigures.Count > 1) //Мы не можем редактировать точки одной фигуры когда выбрано несколько, мы можем только двигать
                                                                {
                                                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                                }
                                                                else
                                                                {
                                                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.MovePoint; //Назначаем режим движения точек на курсор
                                                                }

                                                                break;
                                                            }
                                                        case 2: //Центр линии, можем двигать линию
                                                            {
                                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                                break;
                                                            }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (vobject is VRectangle) //Прямоугольник
                                        {
                                            for (int i = 0; i < vobject.GetPointsIDs().Length; i++)
                                            {
                                                if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                        case 1:
                                                        case 2:
                                                        case 3: //Точки прямоугольника, можно двигать прямоугольник, либо если выбран он один, то масштабировать
                                                            {
                                                                if (SelectedFigures.Count > 1) //Мы не можем масштабировать прямоугольник когда выбрано несколько фигур, мы можем только двигать
                                                                {
                                                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                                }
                                                                else
                                                                {
                                                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.Transform; //Назначаем режим масштабирования на курсор
                                                                }

                                                                break;
                                                            }
                                                        case 4: //Центр прямоугольника, можно двигать прямоугольник

                                                            {
                                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                                break;

                                                            }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (vobject is VEllipse) //Эллипс
                                        {
                                            for (int i = 0; i < vobject.GetPointsIDs().Length; i++)
                                            {
                                                if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                        case 1:
                                                        case 2:
                                                        case 3: //Точки эллипса, можно двигать эллипс
                                                        case 8: //Центр эллипса, можно двигать
                                                            {
                                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                                break;
                                                            }
                                                        case 4:
                                                        case 5:
                                                        case 6:
                                                        case 7: //Точки прямоугольника выделителя. Благодаря ним можем масштабировать эллипс, если не выбраны другие фигуры
                                                            {
                                                                if (SelectedFigures.Count == 1) //Мы не можем масштабировать эллипс когда выбрано несколько фигур, мы можем только двигать
                                                                {
                                                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.Transform; //Назначаем режим масштабирования на курсор
                                                                }
                                                                break;
                                                            }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (vobject is VTriangle) //Треугольник
                                        {
                                            for (int i = 0; i < vobject.GetPointsIDs().Length; i++)
                                            {
                                                if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                        case 1:
                                                        case 2: //Точки треугольника, можно двигать треугольник
                                                        case 7: //Центр
                                                            {
                                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                                break;
                                                            }
                                                        case 3:
                                                        case 4:
                                                        case 5:
                                                        case 6: //Точки прямоугольника выделителя. Благодаря ним можем масштабировать треугольник, если не выбраны другие фигуры

                                                            if (SelectedFigures.Count == 1) //Мы не можем масштабировать треугольник когда выбрано несколько фигур, мы можем только двигать
                                                            {
                                                                MyPoint? p1 = Vector.FindPbyID(vobject.GetPointsIDs()[3]); //Минимальный угол прямоугольника-выделителя
                                                                MyPoint? p2 = Vector.FindPbyID(vobject.GetPointsIDs()[4]); //Максимальный угол прямоугольника-выделителя
                                                                if (p1 == null || p2 == null) return; //Если нет точек, ничего не делаем
                                                                float width = p2.Value.X - p1.Value.X, height = p2.Value.Y - p1.Value.Y;
                                                                RelativelyCoords.Clear();
                                                                for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                                                { // Берем длину на 5 меньше, т.к. мы работаем с точками треугольника
                                                                    MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка треугольника
                                                                    if (p == null) return; //Если нет точки, ничего не делаем
                                                                    RelativelyCoords.Add(new PointF((p.Value.X - p1.Value.X) / width, (p.Value.Y - p1.Value.Y) / height));
                                                                }
                                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Transform; //Назначаем режим масштабирования на курсор
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (vobject is VPolygone) //Многоугольник
                                        {
                                            for (int i = 0; i < vobject.GetPointsIDs().Length; i++)
                                            {
                                                if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                {
                                                    int count = ((CustomFigure)vobject).PointCount;
                                                    if (i < count || i == vobject.GetPointsIDs().Length - 1)  //Точки многоугольника или центр, можно двигать многоугольник
                                                    {
                                                        SettingsAndModes.CMode = SettingsAndModes.CursorMode.MoveFigure; //Назначаем режим движения объекта на курсор
                                                    }
                                                    else //Точки прямоугольника выделителя. Благодаря ним можем масштабировать многоугольник, если не выбраны другие фигуры
                                                    {
                                                        if (SelectedFigures.Count == 1) //Мы не можем масштабировать многоугольник когда выбрано несколько фигур, мы можем только двигать
                                                        {
                                                            MyPoint? p1 = Vector.FindPbyID(vobject.GetPointsIDs()[count]); //Минимальный угол прямоугольника-выделителя
                                                            MyPoint? p2 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 1]); //Максимальный угол прямоугольника-выделителя
                                                            if (p1 == null || p2 == null) return; //Если нет точек, ничего не делаем
                                                            float width = p2.Value.X - p1.Value.X, height = p2.Value.Y - p1.Value.Y;
                                                            RelativelyCoords.Clear();
                                                            for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                                            { // Берем длину на 5 меньше, т.к. мы работаем с точками многоугольника
                                                                MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка многоугольника
                                                                if (p == null) return; //Если нет точки, ничего не делаем
                                                                RelativelyCoords.Add(new PointF((p.Value.X - p1.Value.X) / width, (p.Value.Y - p1.Value.Y) / height));
                                                            }
                                                            SettingsAndModes.CMode = SettingsAndModes.CursorMode.Transform; //Назначаем режим масштабирования на курсор
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    pictureBox1.ContextMenuStrip = CopyDeleteMenu; //Назначаем меню копирования/удаления
                                } else
                                {
                                    SelectedFigures.Clear(); //Очищаем выбранные фигуры/объекты
                                    SelectPoint = null; //очищаем выбранную точку
                                    SelectedAreaPoint = null;
                                    SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Назначаем обычный режим на курсор
                                    MousePrevPoint = new Point(e.X, e.Y); //Сохраняем точку, в которой мы сделали клик
                                    pictureBox1.ContextMenuStrip = PasteMenu; //Назначаем меню вставки
                                    pictureBox1.Invalidate(); //Очищаем PictureBox
                                                              //Убираем толщину и ставим ее значение на стандартное
                                    labelThickness.Visible = false;
                                    thicknessBar.Value = 100;
                                    thicknessBar.Visible = false;
                                }

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
                            if (SelectPoint.HasValue)
                            {
                                GraphObject vobject = SelectPoint.Value.VObject; //Берем фигуру, которой принадлежит точка
                                switch (SettingsAndModes.CMode)
                                {
                                    case SettingsAndModes.CursorMode.MovePoint: //Если установлен режим движения точки 
                                        {
                                            if (!(vobject is Line)) //Движение точки доступно только линии
                                            {
                                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Назначаем стандартный мод
                                                return;
                                            }
                                            int dx = e.X - SelectPoint.Value.X, dy = e.Y - SelectPoint.Value.Y; //Вычисляем смещение
                                            Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint); //Устанавливаем точке новые координаты
                                            MyPoint newp = SelectPoint.Value;
                                            newp.X = SelectPoint.Value.X + dx;
                                            newp.Y = SelectPoint.Value.Y + dy;
                                            SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                            vobject.RecalculateCenter(); //Пересчитываем центр
                                            break;
                                        }
                                    case SettingsAndModes.CursorMode.MoveFigure: //Если установлен режим движения фигуры
                                        {
                                            int dx = e.X - SelectPoint.Value.X, dy = e.Y - SelectPoint.Value.Y;
                                            if (SelectedFigures.Count > 1)
                                            {
                                                //Создаем пустую максимальную точку (0,0) и минимальную в точке (Int32.MaxValue, Int32.MaxValue) для пересчета прямоугольника выделения (если выделено больше 1 фигуры)
                                                Point min = new Point(Int32.MaxValue, Int32.MaxValue);
                                                Point max = new Point();
                                                foreach (GraphObject curobj in SelectedFigures) //Перебираем выбранные фигуры
                                                {
                                                    foreach (int ID in curobj.GetPointsIDs()) //Перебираем их точки
                                                    {
                                                        MyPoint? p = Vector.FindPbyID(ID); //Ищем точку
                                                        if (p == null) return; //если точки нет
                                                        //Ищем максимальную позицию и минимальную
                                                        if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                                                        if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                                                        if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                                                        if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
                                                        Vector.SetCoordsP(p.Value.X + dx, p.Value.Y + dy, p);
                                                    }
                                                }
                                                SelectAreaPoints = new Point[] { min, max, new Point(min.X, max.Y), new Point(max.X, min.Y) }; //Точки прямоугольника-выделителя

                                            }
                                            else
                                            {
                                                foreach (int ID in vobject.GetPointsIDs()) //Перебираем точки фигуры
                                                {
                                                    MyPoint? p = Vector.FindPbyID(ID); //Ищем точку
                                                    if (p == null) continue; //если точки нет
                                                    Vector.SetCoordsP(p.Value.X + dx, p.Value.Y + dy, p);
                                                }
                                            }
                                            MyPoint newp = SelectPoint.Value;
                                            newp.X = SelectPoint.Value.X + dx;
                                            newp.Y = SelectPoint.Value.Y + dy;
                                            SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                            break;
                                        }
                                    case SettingsAndModes.CursorMode.Transform:
                                        {
                                            int dx = e.X - SelectPoint.Value.X, dy = e.Y - SelectPoint.Value.Y; //Вычисляем смещение
                                            if (vobject is VRectangle)
                                            {
                                                for (int i = 0; i < vobject.GetPointsIDs().Length - 1; i++) //Проверяем, какая это точка масштабирования
                                                {
                                                    if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                    {
                                                        switch (i)
                                                        {
                                                            case 0:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p2 = Vector.FindPbyID(vobject.GetPointsIDs()[2]);
                                                                    MyPoint? p3 = Vector.FindPbyID(vobject.GetPointsIDs()[3]);
                                                                    if (p2 == null || p3 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p2.Value.X + dx, p2.Value.Y, p2);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p3.Value.X, p3.Value.Y + dy, p3);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    break;
                                                                }
                                                            case 1:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p2 = Vector.FindPbyID(vobject.GetPointsIDs()[2]);
                                                                    MyPoint? p3 = Vector.FindPbyID(vobject.GetPointsIDs()[3]);
                                                                    if (p2 == null || p3 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p2.Value.X, p2.Value.Y + dy, p2);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p3.Value.X + dx, p3.Value.Y, p3);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p0 = Vector.FindPbyID(vobject.GetPointsIDs()[0]);
                                                                    MyPoint? p1 = Vector.FindPbyID(vobject.GetPointsIDs()[1]);
                                                                    if (p0 == null || p1 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p0.Value.X + dx, p0.Value.Y, p0);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p1.Value.X, p1.Value.Y + dy, p1);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p0 = Vector.FindPbyID(vobject.GetPointsIDs()[0]);
                                                                    MyPoint? p1 = Vector.FindPbyID(vobject.GetPointsIDs()[1]);
                                                                    if (p0 == null || p1 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p0.Value.X, p0.Value.Y + dy, p0);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p1.Value.X + dx, p1.Value.Y, p1);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    break;
                                                                }
                                                        }
                                                        break; //Нам не нужно дальше проверять точки
                                                    }

                                                }
                                            }
                                            else if (vobject is VEllipse)
                                            {
                                                for (int i = 4; i < vobject.GetPointsIDs().Length - 1; i++) //Проверяем, какая это точка масштабирования, начинаем с 4
                                                {
                                                    if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                    {
                                                        switch (i)
                                                        {
                                                            case 4:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p6 = Vector.FindPbyID(vobject.GetPointsIDs()[6]);
                                                                    MyPoint? p7 = Vector.FindPbyID(vobject.GetPointsIDs()[7]);
                                                                    if (p6 == null || p7 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p6.Value.X + dx, p6.Value.Y, p6);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p7.Value.X, p7.Value.Y + dy, p7);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    ((VEllipse)vobject).RecalculatePoints(); //Пересчитываем точки эллипса
                                                                    break;
                                                                }
                                                            case 5:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p6 = Vector.FindPbyID(vobject.GetPointsIDs()[6]);
                                                                    MyPoint? p7 = Vector.FindPbyID(vobject.GetPointsIDs()[7]);
                                                                    if (p6 == null || p7 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p6.Value.X, p6.Value.Y + dy, p6);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p7.Value.X + dx, p7.Value.Y, p7);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    ((VEllipse)vobject).RecalculatePoints(); //Пересчитываем точки эллипса
                                                                    break;
                                                                }
                                                            case 6:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p4 = Vector.FindPbyID(vobject.GetPointsIDs()[4]);
                                                                    MyPoint? p5 = Vector.FindPbyID(vobject.GetPointsIDs()[5]);
                                                                    if (p4 == null || p5 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p4.Value.X + dx, p4.Value.Y, p4);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p5.Value.X, p5.Value.Y + dy, p5);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    ((VEllipse)vobject).RecalculatePoints(); //Пересчитываем точки эллипса
                                                                    break;
                                                                }
                                                            case 7:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p4 = Vector.FindPbyID(vobject.GetPointsIDs()[4]);
                                                                    MyPoint? p5 = Vector.FindPbyID(vobject.GetPointsIDs()[5]);
                                                                    if (p4 == null || p5 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p4.Value.X, p4.Value.Y + dy, p4);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p5.Value.X + dx, p5.Value.Y, p5);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    vobject.RecalculateCenter(); //Пересчитываем центр
                                                                    ((VEllipse)vobject).RecalculatePoints(); //Пересчитываем точки эллипса
                                                                    break;
                                                                }
                                                        }
                                                        break; //Нам не нужно дальше проверять точки
                                                    }

                                                }
                                            }
                                            else if (vobject is VTriangle)
                                            {
                                                for (int i = 3; i < vobject.GetPointsIDs().Length - 1; i++) //Проверяем, какая это точка масштабирования, начинаем с 3
                                                {
                                                    if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                    {
                                                        switch (i)
                                                        {
                                                            case 3:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p5 = Vector.FindPbyID(vobject.GetPointsIDs()[5]);
                                                                    MyPoint? p6 = Vector.FindPbyID(vobject.GetPointsIDs()[6]);
                                                                    if (p5 == null || p6 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p5.Value.X + dx, p5.Value.Y, p5);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p6.Value.X, p6.Value.Y + dy, p6);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p5 = Vector.FindPbyID(vobject.GetPointsIDs()[5]);
                                                                    MyPoint? p6 = Vector.FindPbyID(vobject.GetPointsIDs()[6]);
                                                                    if (p5 == null || p6 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p5.Value.X, p5.Value.Y + dy, p5);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p6.Value.X + dx, p6.Value.Y, p6);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                            case 5:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p3 = Vector.FindPbyID(vobject.GetPointsIDs()[3]);
                                                                    MyPoint? p4 = Vector.FindPbyID(vobject.GetPointsIDs()[4]);
                                                                    if (p3 == null || p4 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p3.Value.X + dx, p3.Value.Y, p3);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p4.Value.X, p4.Value.Y + dy, p4);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                            case 6:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p3 = Vector.FindPbyID(vobject.GetPointsIDs()[3]);
                                                                    MyPoint? p4 = Vector.FindPbyID(vobject.GetPointsIDs()[4]);
                                                                    if (p3 == null || p4 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p3.Value.X, p3.Value.Y + dy, p3);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p4.Value.X + dx, p4.Value.Y, p4);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                        }

                                                        MyPoint? pmin = Vector.FindPbyID(vobject.GetPointsIDs()[3]); //Минимальный угол прямоугольника-выделителя
                                                        MyPoint? pmax = Vector.FindPbyID(vobject.GetPointsIDs()[4]); //Максимальный угол прямоугольника-выделителя
                                                        if (pmin == null || pmax == null) return; //Если нет точек, ничего не делаем
                                                        float width = pmax.Value.X - pmin.Value.X, height = pmax.Value.Y - pmin.Value.Y;

                                                        for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                                        { // Берем длину на 5 меньше, т.к. мы работаем с точками треугольника
                                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка треугольника
                                                            if (p == null) return; //Если нет точки, ничего не делаем
                                                            Vector.SetCoordsP((int)(RelativelyCoords[k].X * width) + pmin.Value.X, (int)(RelativelyCoords[k].Y * height) + pmin.Value.Y, p);
                                                        }
                                                        vobject.RecalculateCenter(); //Пересчитываем центр
                                                        break; //Нам не нужно дальше проверять точки
                                                    }

                                                }
                                            }
                                            else if (vobject is VPolygone)
                                            {
                                                int count = ((CustomFigure)vobject).PointCount;
                                                for (int i = count; i < vobject.GetPointsIDs().Length - 1; i++) //Проверяем, какая это точка масштабирования, начинаем с count
                                                {
                                                    if (vobject.GetPointsIDs()[i] == SelectPoint.Value.ID)
                                                    {
                                                        switch (i - count)
                                                        {
                                                            case 0:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p2 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 2]);
                                                                    MyPoint? p3 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 3]);
                                                                    if (p2 == null || p3 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p2.Value.X + dx, p2.Value.Y, p2);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p3.Value.X, p3.Value.Y + dy, p3);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                            case 1:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p2 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 2]);
                                                                    MyPoint? p3 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 3]);
                                                                    if (p2 == null || p3 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p2.Value.X, p2.Value.Y + dy, p2);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p3.Value.X + dx, p3.Value.Y, p3);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p0 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 0]);
                                                                    MyPoint? p1 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 1]);
                                                                    if (p0 == null || p1 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p0.Value.X + dx, p0.Value.Y, p0);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p1.Value.X, p1.Value.Y + dy, p1);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    //Получаем точки которые имеют связь с данной точкой
                                                                    MyPoint? p0 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 0]);
                                                                    MyPoint? p1 = Vector.FindPbyID(vobject.GetPointsIDs()[count + 1]);
                                                                    if (p0 == null || p1 == null) return;
                                                                    //Прибавляем к выбранному месту dx, dy
                                                                    Vector.SetCoordsP(SelectPoint.Value.X + dx, SelectPoint.Value.Y + dy, SelectPoint);
                                                                    //Прибавляем dy
                                                                    Vector.SetCoordsP(p0.Value.X, p0.Value.Y + dy, p0);
                                                                    //Прибавляем dx
                                                                    Vector.SetCoordsP(p1.Value.X + dx, p1.Value.Y, p1);
                                                                    MyPoint newp = SelectPoint.Value;
                                                                    newp.X = SelectPoint.Value.X + dx;
                                                                    newp.Y = SelectPoint.Value.Y + dy;
                                                                    SelectPoint = newp; //Перезаписываем выбранной точке координаты
                                                                    break;
                                                                }
                                                        }

                                                        MyPoint? pmin = Vector.FindPbyID(vobject.GetPointsIDs()[count]); //Минимальный угол прямоугольника-выделителя
                                                        MyPoint? pmax = Vector.FindPbyID(vobject.GetPointsIDs()[count + 1]); //Максимальный угол прямоугольника-выделителя
                                                        if (pmin == null || pmax == null) return; //Если нет точек, ничего не делаем
                                                        float width = pmax.Value.X - pmin.Value.X, height = pmax.Value.Y - pmin.Value.Y;

                                                        for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                                        { // Берем длину на 5 меньше, т.к. мы работаем с точками треугольника
                                                            MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка треугольника
                                                            if (p == null) return; //Если нет точки, ничего не делаем
                                                            Vector.SetCoordsP((int)(RelativelyCoords[k].X * width) + pmin.Value.X, (int)(RelativelyCoords[k].Y * height) + pmin.Value.Y, p);
                                                        }
                                                        vobject.RecalculateCenter(); //Пересчитываем центр
                                                        break; //Нам не нужно дальше проверять точки
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            else if (SelectedAreaPoint.HasValue && SettingsAndModes.CMode == SettingsAndModes.CursorMode.Transform)
                            { //Если мы нажали на точку масштабирования у прямоугольника-выделителя
                                int dx = e.X - SelectedAreaPoint.Value.X, dy = e.Y - SelectedAreaPoint.Value.Y;
                                for (int i = 0; i < SelectAreaPoints.Length; i++) //Проверяем, какая это точка масштабирования
                                {
                                    if (SelectAreaPoints[i] == SelectedAreaPoint)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                {
                                                    //Получаем точки которые имеют связь с данной точкой
                                                    Point p2 = SelectAreaPoints[2];
                                                    Point p3 = SelectAreaPoints[3];
                                                    //Прибавляем к выбранному месту dx, dy
                                                    SelectAreaPoints[i] = new Point(SelectedAreaPoint.Value.X + dx, SelectedAreaPoint.Value.Y + dy);
                                                    SelectedAreaPoint = SelectAreaPoints[i];
                                                    //Прибавляем dx
                                                    p2.X += dx;
                                                    SelectAreaPoints[2] = p2;
                                                    //Прибавляем dy
                                                    p3.Y += dy;
                                                    SelectAreaPoints[3] = p3;
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    //Получаем точки которые имеют связь с данной точкой
                                                    Point p2 = SelectAreaPoints[2];
                                                    Point p3 = SelectAreaPoints[3];
                                                    //Прибавляем к выбранному месту dx, dy
                                                    SelectAreaPoints[i] = new Point(SelectedAreaPoint.Value.X + dx, SelectedAreaPoint.Value.Y + dy);
                                                    SelectedAreaPoint = SelectAreaPoints[i];
                                                    //Прибавляем dy
                                                    p2.Y += dy;
                                                    SelectAreaPoints[2] = p2;
                                                    //Прибавляем dx
                                                    p3.X += dx;
                                                    SelectAreaPoints[3] = p3;
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    //Получаем точки которые имеют связь с данной точкой
                                                    Point p0 = SelectAreaPoints[0];
                                                    Point p1 = SelectAreaPoints[1];
                                                    //Прибавляем к выбранному месту dx, dy
                                                    SelectAreaPoints[i] = new Point(SelectedAreaPoint.Value.X + dx, SelectedAreaPoint.Value.Y + dy);
                                                    SelectedAreaPoint = SelectAreaPoints[i];
                                                    //Прибавляем dx
                                                    p0.X += dx;
                                                    SelectAreaPoints[0] = p0;
                                                    //Прибавляем dy
                                                    p1.Y += dy;
                                                    SelectAreaPoints[1] = p1;
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    //Получаем точки которые имеют связь с данной точкой
                                                    Point p0 = SelectAreaPoints[0];
                                                    Point p1 = SelectAreaPoints[1];
                                                    //Прибавляем к выбранному месту dx, dy
                                                    SelectAreaPoints[i] = new Point(SelectedAreaPoint.Value.X + dx, SelectedAreaPoint.Value.Y + dy);
                                                    SelectedAreaPoint = SelectAreaPoints[i];
                                                    //Прибавляем dy
                                                    p0.Y += dy;
                                                    SelectAreaPoints[0] = p0;
                                                    //Прибавляем dx
                                                    p1.X += dx;
                                                    SelectAreaPoints[1] = p1;
                                                    break;
                                                }

                                        }
                                        Point min = SelectAreaPoints[0]; //Минимальный угол прямоугольника-выделителя
                                        Point max = SelectAreaPoints[1]; //Максимальный угол прямоугольника-выделителя
                                        float width = max.X - min.X, height = max.Y - min.Y;
                                        int j = 0;
                                        foreach (GraphObject vobject in SelectedFigures) //Перебираем выбранные фигуры
                                        {
                                            if (vobject is Line)
                                            {
                                                for (int k = 0; k < vobject.GetPointsIDs().Length - 1; k++)
                                                { // Берем длину на 1 меньше, т.к. мы работаем с точками линии
                                                    MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка линии
                                                    if (p == null) return; //Если нет точки, ничего не делаем
                                                    Vector.SetCoordsP((int)(min.X + RelativelyCoords[j].X * width), (int)(min.Y + RelativelyCoords[j].Y * height), p); //Назначаем точки координаты
                                                    j++; //Счетчик
                                                }

                                            }
                                            else if (vobject is VRectangle)
                                            {
                                                for (int k = 0; k < vobject.GetPointsIDs().Length - 1; k++)
                                                { // Берем длину на 1 меньше, т.к. мы работаем с точками прямоугольника
                                                    MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка прямоугольника
                                                    if (p == null) return; //Если нет точки, ничего не делаем
                                                    Vector.SetCoordsP((int)(min.X + RelativelyCoords[j].X * width), (int)(min.Y + RelativelyCoords[j].Y * height), p); //Назначаем точки координаты
                                                    j++; //Счетчик
                                                }
                                            }
                                            else if (vobject is VEllipse)
                                            {
                                                for (int k = 4; k < vobject.GetPointsIDs().Length - 1; k++)
                                                { // Берем длину на 1 меньше, т.к. мы работаем с точками эллипса
                                                    MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка выделителя эллипса
                                                    if (p == null) return; //Если нет точки, ничего не делаем
                                                    Vector.SetCoordsP((int)(min.X + RelativelyCoords[j].X * width), (int)(min.Y + RelativelyCoords[j].Y * height), p); //Назначаем точки координаты
                                                    j++; //Счетчик
                                                }
                                                ((VEllipse)vobject).RecalculatePoints();
                                            }
                                            else if (vobject is VTriangle)
                                            {
                                                for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                                { // Берем длину на 5 меньше, т.к. мы работаем с точками треугольника
                                                    MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка треугольника
                                                    if (p == null) return; //Если нет точки, ничего не делаем
                                                    Vector.SetCoordsP((int)(min.X + RelativelyCoords[j].X * width), (int)(min.Y + RelativelyCoords[j].Y * height), p); //Назначаем точки координаты
                                                    j++; //Счетчик
                                                }
                                                ((VTriangle)vobject).RecalculateSelectArea();
                                            }
                                            else if (vobject is VPolygone)
                                            {
                                                for (int k = 0; k < vobject.GetPointsIDs().Length - 5; k++)
                                                { // Берем длину на 5 меньше, т.к. мы работаем с точками многоугольника
                                                    MyPoint? p = Vector.FindPbyID(vobject.GetPointsIDs()[k]); //Точка многоугольника
                                                    if (p == null) return; //Если нет точки, ничего не делаем
                                                    Vector.SetCoordsP((int)(min.X + RelativelyCoords[j].X * width), (int)(min.Y + RelativelyCoords[j].Y * height), p); //Назначаем точки координаты
                                                    j++; //Счетчик
                                                }
                                                ((VPolygone)vobject).RecalculateSelectArea();
                                            }
                                            vobject.RecalculateCenter();
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        pictureBox1.Invalidate(); //Очищаем PictureBox
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
                                            if (SelectPoint == null && SelectedAreaPoint == null) //Если мы не выбирали никакой точки
                                            {
                                                foreach (GraphObject vobject in Vector.GetAllFigures()) //Перебираем все фигуры
                                                {
                                                    int i = 0; //Счетчик точек, мы должны не учитывать точки выделения у кастомных фигур, т.к. они работают только когда фигура уже выделена
                                                    foreach (int ID in vobject.GetPointsIDs()) //Перебираем все точки фигуры
                                                    {
                                                        if (vobject is CustomFigure) //Если фигура, которой принадлежит точка кастомная (многоугольник, треугольник), нужно проверить чтоб нажатая точка не была точкой выделения
                                                        {
                                                            if (i != vobject.GetPointsIDs().Length - 1 && i >= ((CustomFigure)vobject).PointCount) //Если точка не центр и не составляет фигуру, значит это точка выделения, пропускаем
                                                            {
                                                                i++;
                                                                continue;
                                                            }
                                                        }
                                                        else if (vobject is VEllipse) //Если фигура, которой принадлежит точка эллипс, нужно проверить чтоб нажатая точка не была точкой выделения
                                                        {
                                                            if (i != vobject.GetPointsIDs().Length - 1 && i >= 4) //Если точка не центр и не составляет фигуру, значит это точка выделения, пропускаем
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
                                                if (SelectedFigures.Count > 0)
                                                {
                                                    bool IsSameColor = true, IsSameThickColor = true, IsSameThick = true; //Сверяем, одинаковые ли цвета и толщина у фигур
                                                    Color fill = SelectedFigures[0] is Figure ? ((Figure)SelectedFigures[0]).Color : Color.Empty, thickColor = SelectedFigures[0].ThicknessColor; //Записываем цвет заливки и цвет обводки, если это не Figure, то цвет заливки будет пустой 
                                                    float thick = SelectedFigures[0].Thickness;
                                                    //Создаем пустую максимальную точку (0,0) и минимальную в точке (Int32.MaxValue, Int32.MaxValue)
                                                    Point min = new Point(Int32.MaxValue, Int32.MaxValue);
                                                    Point max = new Point();
                                                    foreach (GraphObject vobject in SelectedFigures) //Перебираем выбранные фигуры
                                                    {
                                                        IsSameColor = fill == (vobject is Figure ? ((Figure)vobject).Color : Color.Empty); //Цвета заливки, если объект не Figure, то цвет заливки будет пустой 
                                                        IsSameThick = thick == vobject.Thickness; //Толщина обводки
                                                        IsSameThickColor = thickColor == vobject.ThicknessColor; //Цвет обводки
                                                        foreach (int ID in vobject.GetPointsIDs()) //Перебираем все точки фигуры
                                                        {
                                                            MyPoint? point = Vector.FindPbyID(ID);
                                                            if (!point.HasValue) continue;
                                                            //Ищем максимальную позицию и минимальную
                                                            if (point.Value.X < min.X) min.X = point.Value.X; //Если у данной точки X меньше, назначаем в переменную
                                                            if (point.Value.Y < min.Y) min.Y = point.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                                                            if (point.Value.X > max.X) max.X = point.Value.X; //Если у данной точки X больше, назначаем в переменную
                                                            if (point.Value.Y > max.Y) max.Y = point.Value.Y; //Если у данной точки Y больше, назначаем в переменную
                                                        }

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
                                                    SelectAreaPoints = new Point[] { min, max, new Point(min.X, max.Y), new Point(max.X, min.Y) }; //Точки прямоугольника-выделителя
                                                }
                                            }
                                            break;
                                        }
                                }
                                SettingsAndModes.CMode = SettingsAndModes.CursorMode.Select; //Возвращаем стандартный мод назад
                                SelectedAreaPoint = null;
                                SelectPoint = null;
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
                                Line line = new Line(thicknessBar.Value / 100f, thickness, new Point[] { MousePrevPoint, MouseCurrentPoint, new Point(((MousePrevPoint.X + MouseCurrentPoint.X) / 2), 
                                    ((MousePrevPoint.Y + MouseCurrentPoint.Y) / 2)) }); //Создаем экземпляр класса Line, передаем точки
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
                                VEllipse ellipse = new VEllipse(thicknessBar.Value / 100f, thickness, fill, new Point[] { new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)),
                                    new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)),
                                    new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)), 
                                    new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)),
                                    new Point(((MousePrevPoint.X + MouseCurrentPoint.X) / 2), ((MousePrevPoint.Y + MouseCurrentPoint.Y) / 2)) }); //Создаем экземпляр класса VEllipse, передаем точки
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
                                VRectangle rect = new VRectangle(thicknessBar.Value / 100f, thickness, fill, new Point[] { new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)), 
                                    new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)),
                                    new Point(Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Max(MousePrevPoint.Y, MouseCurrentPoint.Y)), 
                                    new Point(Math.Max(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y)),
                                    new Point(((MousePrevPoint.X + MouseCurrentPoint.X) / 2), ((MousePrevPoint.Y + MouseCurrentPoint.Y) / 2)) }); //Создаем экземпляр класса VRectangle, передаем точки
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
            foreach (GraphObject figure in Vector.GetAllFigures()) //Перебираем все созданные фигуры
            {
                figure.Draw(g); //Отрисовка фигуры
            }
            if (SelectedFigures.Count == 1) //Если у нас одна фигура
            {
                SelectedFigures[0].Select(g);
                SelectedFigures[0].DrawSelectArea(g);
            }
            else
            {
                foreach (GraphObject figure in SelectedFigures) //Перебираем все выбранные фигуры
                {

                    figure.Select(g);
                    Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
                    for (int i = 0; i < SelectAreaPoints.Length; i++)
                    {
                        if (SelectAreaPoints[i].X < min.X) min.X = SelectAreaPoints[i].X; //Если у данной точки X меньше, назначаем в переменную
                        if (SelectAreaPoints[i].Y < min.Y) min.Y = SelectAreaPoints[i].Y; //Если у данной точки Y меньше, назначаем в переменную
                        if (SelectAreaPoints[i].X > max.X) max.X = SelectAreaPoints[i].X; //Если у данной точки X больше, назначаем в переменную
                        if (SelectAreaPoints[i].Y > max.Y) max.Y = SelectAreaPoints[i].Y; //Если у данной точки Y больше, назначаем в переменную
                    }
                    g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), min.X, min.Y, max.X - min.X, max.Y - min.Y); //рисуем контур прямоугольника-выделителя

                    for (int i = 0; i < SelectAreaPoints.Length; i++)
                    {
                        //рисуем квадратики-точки у прямоугольника-выделителя
                        g.FillRectangle(new SolidBrush(Color.White), SelectAreaPoints[i].X - 2, SelectAreaPoints[i].Y - 2, 5, 5);
                        g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), SelectAreaPoints[i].X - 2, SelectAreaPoints[i].Y - 2, 5, 5);
                    }
                }
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
                            g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), 
                                Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                            break;
                        }
                    case SettingsAndModes.EditorMode.Rectangle: //Мод построения прямоугольника
                        {
                            g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, thicknessBar.Value / 100f), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), 
                                Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
                            break;
                        }
                    case SettingsAndModes.EditorMode.Cursor: //Мод курсора
                        {
                            switch (SettingsAndModes.CMode)
                            {
                                case SettingsAndModes.CursorMode.Select:
                                    {
                                        if (SelectPoint != null || SelectedAreaPoint != null) return; //Если мы выбирали точку, не рисуем
                                        g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), Math.Min(MousePrevPoint.X, MouseCurrentPoint.X), Math.Min(MousePrevPoint.Y, MouseCurrentPoint.Y), 
                                            Math.Abs(MousePrevPoint.X - MouseCurrentPoint.X), Math.Abs(MousePrevPoint.Y - MouseCurrentPoint.Y));
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
                g.FillEllipse(new SolidBrush(SettingsAndModes.EditPointColor), PolygonPoints.Last().X - 2, PolygonPoints.Last().Y - 2, 5, 5); //Показываем последнюю точку, причем поверх "резиновой" линии для красоты 
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
            //Создаем пустую максимальную точку (0,0) и минимальную в точке (Int32.MaxValue, Int32.MaxValue)
            Point min = new Point(Int32.MaxValue, Int32.MaxValue);
            Point max = new Point();

            foreach (GraphObject vobject in CopiedFigures) //Перебираем скопированные объекты
            {
                GraphObject clone = vobject.Clone(dx, dy); //Клонируем, передается смещение по x и y
                if (clone == null) return;
                Vector.AddNewFigure(clone); //Добавляем объект в список объектов
                SelectedFigures.Add(clone); //Автоматически выбираем скопированный объект
                foreach (int ID in clone.GetPointsIDs()) //Перебираем все точки фигуры
                {
                    MyPoint? point = Vector.FindPbyID(ID);
                    if (!point.HasValue) continue;
                    //Ищем максимальную позицию и минимальную
                    if (point.Value.X < min.X) min.X = point.Value.X; //Если у данной точки X меньше, назначаем в переменную
                    if (point.Value.Y < min.Y) min.Y = point.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                    if (point.Value.X > max.X) max.X = point.Value.X; //Если у данной точки X больше, назначаем в переменную
                    if (point.Value.Y > max.Y) max.Y = point.Value.Y; //Если у данной точки Y больше, назначаем в переменную
                }


            }
            SelectAreaPoints = new Point[] { min, max, new Point(min.X, max.Y), new Point(max.X, min.Y) }; //Точки прямоугольника-выделителя
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
                                                                       //Создаем пустую максимальную точку (0,0) и минимальную в точке (Int32.MaxValue, Int32.MaxValue)
                        Point min = new Point(Int32.MaxValue, Int32.MaxValue);
                        Point max = new Point();

                        foreach (GraphObject vobject in CopiedFigures) //Перебираем скопированные объекты
                        {
                            GraphObject clone = vobject.Clone(dx, dy); //Клонируем, передается смещение по x и y
                            if (clone == null) return;
                            Vector.AddNewFigure(clone); //Добавляем объект в список объектов
                            SelectedFigures.Add(clone); //Автоматически выбираем скопированный объект
                            foreach (int ID in clone.GetPointsIDs()) //Перебираем все точки фигуры
                            {
                                MyPoint? point = Vector.FindPbyID(ID);
                                if (!point.HasValue) continue;
                                //Ищем максимальную позицию и минимальную
                                if (point.Value.X < min.X) min.X = point.Value.X; //Если у данной точки X меньше, назначаем в переменную
                                if (point.Value.Y < min.Y) min.Y = point.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                                if (point.Value.X > max.X) max.X = point.Value.X; //Если у данной точки X больше, назначаем в переменную
                                if (point.Value.Y > max.Y) max.Y = point.Value.Y; //Если у данной точки Y больше, назначаем в переменную
                            }


                        }
                        SelectAreaPoints = new Point[] { min, max, new Point(min.X, max.Y), new Point(max.X, min.Y) }; //Точки прямоугольника-выделителя
                        pictureBox1.Invalidate(); //Очищаем PictureBox
                        break;
                    }
                case Keys.C: //Копирование выделенных фигур
                    {
                        if (!e.Control) return;
                        if (SelectedFigures.Count == 0) return; //У нас не выбраны объекты
                        CopiedFigures.Clear(); //Очищаем список скопированных объектов
                        CopiedFigures.AddRange(SelectedFigures); //Добавляем выбранные объекты в скопированные
                        break;
                    }
            }
        }
    }
}
