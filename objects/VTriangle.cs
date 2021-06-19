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
                PointsIDs[i] = mypoint.ID; //Записываем ID точки в массив точек линии
            }

            //Первая точка выделения
            MyPoint select1 = new MyPoint(min.X, min.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select1); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 5] = select1.ID; //Записываем ID центра последним в массив

            //Вторая точка выделения
            MyPoint select2 = new MyPoint(max.X, max.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select2); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 4] = select2.ID; //Записываем ID центра последним в массив

            //Третья точка выделения
            MyPoint select3 = new MyPoint(min.X, max.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select3); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 3] = select3.ID; //Записываем ID центра последним в массив

            //Четвертая точка выделения
            MyPoint select4 = new MyPoint(max.X, min.Y, Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(select4); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length - 2] = select4.ID; //Записываем ID центра последним в массив

            //Центр
            MyPoint center = new MyPoint(((min.X + max.X) / 2), ((min.Y + max.Y) / 2), Vector.IDS++, this); //Конвертируем Point в мою структуру MyPoint
            Vector.AddP(center); //Добавляем точку в общий список
            PointsIDs[PointsIDs.Length-1] = center.ID; //Записываем ID центра последним в массив
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
                g.FillPolygon(new SolidBrush(color), new Point[] {one.Value.ConvertToPoint(), two.Value.ConvertToPoint(), three.Value.ConvertToPoint()}); //Заливка

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawPolygon(new Pen(thicknessColor, thickness), new Point[] { one.Value.ConvertToPoint(), two.Value.ConvertToPoint(), three.Value.ConvertToPoint() }); //Рисуем контур
            }                                                                                                                                                                             
        }

        public override void Select(Graphics g) //Выбор треугольника
        {
            Point[] points = new Point[PointsIDs.Length - 5];
            MyPoint? begin = null, end = null;
            for (int i = 0; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. мы работаем с текущей точкой и следующей после нее.
                begin = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                end = Vector.FindPbyID(PointsIDs[i + 1]); //следующая
                if (begin == null || end == null) return; //если точек нет, не рисуем
                //Сначала отрисовываем точки треугольника и записываем его точки, чтобы потом отрисовать выделение 
                if (i < PointsIDs.Length - 5) //от 0 до n-1, где n - количество углов
                {
                    points[i] = begin.Value.ConvertToPoint();//Конвертируем в Point, записываем чтоб потом отрисовать выделение
                    g.FillEllipse(new SolidBrush(SettingsAndModes.EditPointColor), begin.Value.X - 2, begin.Value.Y - 2, 5, 5); //Показываем точку треугольника
                    continue;
                }
                if (i == PointsIDs.Length - 5) //Мы дошли до точки прямоугольника-выделителя
                {
                    g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), begin.Value.X, begin.Value.Y, Math.Abs(begin.Value.X - end.Value.X), Math.Abs(begin.Value.Y - end.Value.Y)); //рисуем контур выделения треугольника прямоугольником
                }

                //рисуем квадратики-точки у прямоугольника-выделителя
                g.FillRectangle(new SolidBrush(Color.White), begin.Value.X - 2, begin.Value.Y - 2, 5, 5);
                g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), begin.Value.X - 2, begin.Value.Y - 2, 5, 5);
            }

            g.DrawPolygon(new Pen(SettingsAndModes.EditLineColor, 1), points); //Выделяем треугольник
            //Показываем центр треугольника
            g.FillEllipse(new SolidBrush(SettingsAndModes.CenterPointColor), end.Value.X - 2, end.Value.Y - 2, 5, 5);
            g.DrawEllipse(new Pen(SettingsAndModes.EditLineColor, 1), end.Value.X - 2, end.Value.Y - 2, 5, 5);
        }

        public override GraphObject Clone(int dx, int dy) //Клонирование данного объекта, передается смещение по x и y
        {
            Point[] points = new Point[PointsIDs.Length]; //Создаем массив с таким же размером, что и у текущего объекта
            for (int i = 0; i < PointsIDs.Length; i++)
            {
                MyPoint? curpoint = Vector.FindPbyID(PointsIDs[i]); //Ищем текущую точку
                if (curpoint == null) return null; //Если точки нет, не клонируем
                Point p = new Point(curpoint.Value.X + dx, curpoint.Value.Y + dx); //Вычисляем новую позицию точки;
                points[i] = p;
            }
            return new VTriangle(thickness, thicknessColor, color, points); //Создаем объект и возвращаем
        }

        public override void MoveTo(Point selected, Point newplace) //Перемещение треугольника по выбранной точке
        {

        }
    }
}
