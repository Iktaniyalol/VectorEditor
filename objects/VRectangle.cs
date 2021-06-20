using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VectorEditor.settings;

namespace VectorEditor.objects
{
    class VRectangle : Figure
    {
        public VRectangle(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIDs = new int[5]; //У прямоугольника(квадрата) будет храниться 4 точки + центр
            for (int i = 0; i < PointsIDs.Length; i++) //Перебираем список точек
            {
                MyPoint mypoint = new MyPoint(Points[i].X, Points[i].Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
                Vector.AddP(mypoint); //Добавляем точку в общий список
                PointsIDs[i] = mypoint.ID; //Записываем ID точки в массив точек
            }
        }
        public override void Draw(Graphics g) //Отрисовывание прямоугольника
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 0; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            //Рисуем прямоугольник, берем угловые точки и вычисляем, какая из них самая левая, потом вычисляем длину и ширину
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillRectangle(new SolidBrush(color), min.X, min.Y, max.X - min.X, max.Y - min.Y); //заливаем

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawRectangle(new Pen(thicknessColor, thickness), min.X, min.Y, max.X - min.X, max.Y - min.Y); //рисуем контур прямоугольника
            }
        }

        public override void Select(Graphics g) //Выбор прямоугольника
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = 0; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //рисуем точки у прямоугольника
                g.FillRectangle(new SolidBrush(SettingsAndModes.EditPointColor), p.Value.X - 2, p.Value.Y - 2, 5, 5);
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), min.X, min.Y, max.X - min.X, max.Y - min.Y); //Рисуем контур выделения прямоугольника 
            DrawCenter(g); //Рисуем центр
        }

        public override void DrawSelectArea(Graphics g) //Функция рисования выделителя
        {
            for (int i = 0; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //рисуем точки-выделители у прямоугольника
                g.FillRectangle(new SolidBrush(Color.White), p.Value.X - 2, p.Value.Y - 2, 5, 5);
                g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), p.Value.X - 2, p.Value.Y - 2, 5, 5);
            }
        }

        private void DrawCenter(Graphics g) //Прорисовываем центр
        {
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length-1]); //центр прямоугольника
            if (center == null) return; //если нет точки, не рисуем
            //Показываем центр прямоугольника
            g.FillRectangle(new SolidBrush(SettingsAndModes.CenterPointColor), center.Value.X - 2, center.Value.Y - 2, 5, 5);
        }

        public override void RecalculateCenter() //Пересчет центра
        {
            MyPoint? begin = Vector.FindPbyID(PointsIDs[0]); //первая точка прямоугольника
            MyPoint? end = Vector.FindPbyID(PointsIDs[1]); //последняя точка прямоугольника
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length - 1]); //центр
            if (begin == null || end == null || center == null) return; //если нет точек, не пересчитываем
            //Теперь нужно перезаписать центру координаты
            Vector.SetCoordsP((begin.Value.X + end.Value.X) / 2, (begin.Value.Y + end.Value.Y) / 2, center);
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
            return new VRectangle(thickness, thicknessColor, color, points); //Создаем объект и возвращаем
        }
    }
}
