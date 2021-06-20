using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VectorEditor.settings;

namespace VectorEditor.objects
{
    class VTriangle : CustomFigure
    {
        public VTriangle(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIDs = new int[8]; //У треугольника будет храниться 3 точки + центр и точки выделения

            Point max = Points[0], min = Points[0];
            for (int i = 0; i < PointsIDs.Length - 5; i++) //Перебираем список точек, от 0 до 3, т.к. центр и точки выделения нужно вычислить
            {
                //Ищем максимальную позицию и минимальную
                if (Points[i].X < min.X) min.X = Points[i].X; //Если у данной точки X меньше, назначаем в переменную
                if (Points[i].Y < min.Y) min.Y = Points[i].Y; //Если у данной точки Y меньше, назначаем в переменную
                if (Points[i].X > max.X) max.X = Points[i].X; //Если у данной точки X больше, назначаем в переменную
                if (Points[i].Y > max.Y) max.Y = Points[i].Y; //Если у данной точки Y больше, назначаем в переменную
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIDs[i] = mypoint.ID; //Записываем ID точки в массив точек
            }

            //Первая точка выделения
            MyPoint select1 = new MyPoint(min.X, min.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select1); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 5] = select1.ID; //Записываем ID

            //Вторая точка выделения
            MyPoint select2 = new MyPoint(max.X, max.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select2); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 4] = select2.ID; //Записываем ID

            //Третья точка выделения
            MyPoint select3 = new MyPoint(min.X, max.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select3); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 3] = select3.ID; //Записываем ID

            //Четвертая точка выделения
            MyPoint select4 = new MyPoint(max.X, min.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select4); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 2] = select4.ID; //Записываем ID

            //Центр
            MyPoint center = new MyPoint(((min.X + max.X) / 2), ((min.Y + max.Y) / 2), Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(center); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 1] = center.ID; //Записываем ID центра последним в массив
        }
        public override void Draw(Graphics g) //Отрисовывание треугольника
        {
            MyPoint? one = Vector.FindPbyID(PointsIDs[0]); //первая точка
            MyPoint? two = Vector.FindPbyID(PointsIDs[1]); //вторая точка
            MyPoint? three = Vector.FindPbyID(PointsIDs[2]); //третья точка
            if (one == null || two == null || three == null) return; //если точек нет, не рисуем
            //Рисуем треугольник, пользуемся drawPolygon, передаем все точки
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillPolygon(new SolidBrush(color), new Point[] { one.Value.ConvertToPoint(), two.Value.ConvertToPoint(), three.Value.ConvertToPoint() }); //Заливка

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawPolygon(new Pen(thicknessColor, thickness), new Point[] { one.Value.ConvertToPoint(), two.Value.ConvertToPoint(), three.Value.ConvertToPoint() }); //Рисуем контур
            }
        }

        public override void Select(Graphics g) //Выбор треугольника
        {
            Point[] points = new Point[PointsIDs.Length - 5];
            for (int i = 0; i < PointsIDs.Length - 5; i++)
            { // Берем длину на 5 меньше, т.к. мы отрисовываем точки и выделяем сам треугольник
                MyPoint? point = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (point == null) return; //если точки нет, не рисуем
                points[i] = point.Value.ConvertToPoint(); //Конвертируем в Point, записываем чтоб потом отрисовать выделение треугольника
                //Показываем точку треугольника
                g.FillRectangle(new SolidBrush(SettingsAndModes.EditPointColor), point.Value.X - 2, point.Value.Y - 2, 5, 5);
            }

            g.DrawPolygon(new Pen(SettingsAndModes.EditLineColor, 1), points); //Выделяем треугольник
            DrawCenter(g); //Рисуем центр
        }

        public override void DrawSelectArea(Graphics g) //Функция рисования выделителя
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 3; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 4 элемента, т.к. нас интересует отрисовка прямоугольника-выделителя
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), min.X, min.Y, max.X - min.X, max.Y - min.Y); //рисуем контур выделения треугольника прямоугольником
            for (int i = 3; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 4 элемента, т.к. нас интересует отрисовка квадратиков-точек у прямоугольника-выделителя
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //рисуем точки-выделители у прямоугольника
                g.FillRectangle(new SolidBrush(Color.White), p.Value.X - 2, p.Value.Y - 2, 5, 5);
                g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), p.Value.X - 2, p.Value.Y - 2, 5, 5);
            }
        }

        private void DrawCenter(Graphics g) //Прорисовываем центр
        {
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length - 1]); //центр треугольника
            if (center == null) return; //если нет точки, не рисуем
            //Показываем центр треугольника
            g.FillRectangle(new SolidBrush(SettingsAndModes.CenterPointColor), center.Value.X - 2, center.Value.Y - 2, 5, 5);
        }

        public override void RecalculateCenter() //Пересчет центра
        {
            MyPoint? begin = Vector.FindPbyID(PointsIDs[3]); //первая точка прямоугольника выделителя
            MyPoint? end = Vector.FindPbyID(PointsIDs[4]); //последняя точка прямоугольника выделителя
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length - 1]); //центр
            if (begin == null || end == null || center == null) return; //если нет точек, не пересчитываем
            //Теперь нужно перезаписать центру координаты
            Vector.SetCoordsP((begin.Value.X + end.Value.X) / 2, (begin.Value.Y + end.Value.Y) / 2, center);
        }

        public override void RecalculateSelectArea() //Пересчет точек выделения
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 0; i < PointsIDs.Length - 5; i++) //Перебираем список точек, от 0 до 3, т.к. нам нужно вычислить новые точки выделения
            {
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            //Первая точка выделения
            MyPoint? select1 = Vector.FindPbyID(GetPointsIDs()[3]);
            Vector.SetCoordsP(min.X, min.Y, select1);
            //Вторая точка выделения
            MyPoint? select2 = Vector.FindPbyID(GetPointsIDs()[4]);
            Vector.SetCoordsP(max.X, max.Y, select2);
            //Третья точка выделения
            MyPoint? select3 = Vector.FindPbyID(GetPointsIDs()[5]);
            Vector.SetCoordsP(min.X, max.Y, select3);
            //Четвертая точка выделения
            MyPoint? select4 = Vector.FindPbyID(GetPointsIDs()[6]);
            Vector.SetCoordsP(max.X, min.Y, select4);
        }

        public override GraphObject Clone(int dx, int dy) //Клонирование данного объекта, передается смещение по x и y
        {
            Point[] points = new Point[PointsIDs.Length - 5]; //Создаем массив с таким же размером, что и у текущего объекта, не считая точки выделения и центр
            for (int i = 0; i < PointsIDs.Length - 5; i++)
            {
                MyPoint? curpoint = Vector.FindPbyID(PointsIDs[i]); //Ищем текущую точку
                if (curpoint == null) return null; //Если точки нет, не клонируем
                Point p = new Point(curpoint.Value.X + dx, curpoint.Value.Y + dy); //Вычисляем новую позицию точки
                points[i] = p;
            }
            return new VTriangle(thickness, thicknessColor, color, points); //Создаем объект и возвращаем
        }
    }
}
