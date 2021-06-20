using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VectorEditor.settings;

namespace VectorEditor.objects
{
    class VEllipse : Figure
    {
        public VEllipse(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIDs = new int[9]; //У эллипса будет храниться 4 точки + 4 точки выделения + центр
            for (int i = 0; i < PointsIDs.Length - 4; i++) //Перебираем список точек, -4 т.к. мы точки эллипса будем высчитывать сами
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIDs[i + 4] = mypoint.ID; //Записываем ID точки в массив точек, записываются точки выделения и точка центра
            }
            //Вычисляем точки самого эллипса
            //Верхние точки прямоугольника выделителя
            MyPoint point = new MyPoint((Points[0].X + Points[3].X) / 2, (Points[0].Y + Points[3].Y) / 2, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(point); //Добавляем точку в общий список
            PointsIDs[0] = point.ID; //Записываем ID точки в массив точек
            //Правые точки прямоугольника выделителя
            point = new MyPoint((Points[3].X + Points[1].X) / 2, (Points[3].Y + Points[1].Y) / 2, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(point); //Добавляем точку в общий список
            PointsIDs[1] = point.ID; //Записываем ID точки в массив точек
            //Нижние точки прямоугольника выделителя
            point = new MyPoint((Points[1].X + Points[2].X) / 2, (Points[1].Y + Points[2].Y) / 2, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(point); //Добавляем точку в общий список
            PointsIDs[2] = point.ID; //Записываем ID точки в массив точек
            //Левые точки прямоугольника выделителя
            point = new MyPoint((Points[2].X + Points[0].X) / 2, (Points[2].Y + Points[0].Y) / 2, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(point); //Добавляем точку в общий список
            PointsIDs[3] = point.ID; //Записываем ID точки в массив точек
        }
        public override void Draw(Graphics g) //Отрисовывание эллипса
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 4; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 5 элемента, т.к. нас интересует отрисовка эллипса
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillEllipse(new SolidBrush(color), min.X, min.Y, max.X - min.X, max.Y - min.Y); //заливаем

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawEllipse(new Pen(thicknessColor, thickness), min.X, min.Y, max.X - min.X, max.Y - min.Y); //рисуем контур эллипса
            }
        }

        public override void Select(Graphics g) //Выбор эллипса
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            //Рисуем точки эллипса
            for (int i = 0; i < PointsIDs.Length - 5; i++)
            { // Берем длину на 5 меньше, т.к. мы отрисовываем точки эллипса
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //точки эллипса
                g.FillRectangle(new SolidBrush(SettingsAndModes.EditPointColor), p.Value.X - 2, p.Value.Y - 2, 5, 5);
            }
            //Выделяем сам эллипс
            for (int i = 4; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 5 элемента, т.к. нас интересует отрисовка выделения эллипса
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, 1), min.X, min.Y, max.X - min.X, max.Y - min.Y);
            DrawCenter(g); //Рисуем центр
        }

        public override void DrawSelectArea(Graphics g) //Функция рисования выделителя
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 4; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 5 элемента, т.к. нас интересует отрисовка прямоугольника-выделителя
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), min.X, min.Y, max.X - min.X, max.Y - min.Y); //рисуем контур выделения эллипса прямоугольником
            for (int i = 4; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 5 элемента, т.к. нас интересует отрисовка квадратиков-точек у прямоугольника-выделителя
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //рисуем точки-выделители у прямоугольника
                g.FillRectangle(new SolidBrush(Color.White), p.Value.X - 2, p.Value.Y - 2, 5, 5);
                g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), p.Value.X - 2, p.Value.Y - 2, 5, 5);
            }
        }

        private void DrawCenter(Graphics g) //Прорисовываем центр
        {
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length - 1]); //центр эллипса
            if (center == null) return; //если нет точки, не рисуем
            //Показываем центр эллипса
            g.FillRectangle(new SolidBrush(SettingsAndModes.CenterPointColor), center.Value.X - 2, center.Value.Y - 2, 5, 5);
        }

        public override void RecalculateCenter() //Пересчет центра
        {
            MyPoint? begin = Vector.FindPbyID(PointsIDs[4]); //первая точка прямоугольника-выделителя
            MyPoint? end = Vector.FindPbyID(PointsIDs[5]); //последняя точка прямоугольника-выделителя
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length - 1]); //центр
            if (begin == null || end == null || center == null) return; //если нет точек, не пересчитываем
            //Теперь нужно перезаписать центру координаты
            Vector.SetCoordsP((begin.Value.X + end.Value.X) / 2, (begin.Value.Y + end.Value.Y) / 2, center);
        }

        public void RecalculatePoints() //Пересчет точек эллипса
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 4; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с 5 элемента, т.к. нас интересует вычисление точек эллипса заново по точкам прямоугольника-выделителя
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            //Вычисляем точки самого эллипса
            //Верхние точки прямоугольника выделителя
            Vector.SetCoordsP((min.X + max.X) / 2, min.Y, Vector.FindPbyID(GetPointsIDs()[0]));
            //Правые точки прямоугольника выделителя
            Vector.SetCoordsP(max.X, (min.Y + max.Y) / 2, Vector.FindPbyID(GetPointsIDs()[1]));
            //Нижние точки прямоугольника выделителя
            Vector.SetCoordsP((max.X + min.X) / 2, max.Y, Vector.FindPbyID(GetPointsIDs()[2]));
            //Левые точки прямоугольника выделителя
            Vector.SetCoordsP(min.X, (max.Y + min.Y) / 2, Vector.FindPbyID(GetPointsIDs()[3]));
        }

        public override GraphObject Clone(int dx, int dy) //Клонирование данного объекта, передается смещение по x и y
        {
            Point[] points = new Point[PointsIDs.Length]; //Создаем массив с таким же размером, что и у текущего объекта
            for (int i = 0; i < PointsIDs.Length; i++)
            {
                MyPoint? curpoint = Vector.FindPbyID(PointsIDs[i]); //Ищем текущую точку
                if (curpoint == null) return null; //Если точки нет, не клонируем
                Point p = new Point(curpoint.Value.X + dx, curpoint.Value.Y + dy); //Вычисляем новую позицию точки
                points[i] = p;
            }
            return new VEllipse(thickness, thicknessColor, color, points); //Создаем объект и возвращаем
        }
    }
}
