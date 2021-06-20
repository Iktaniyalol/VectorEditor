using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VectorEditor.settings;

namespace VectorEditor.objects
{
    class VPolygone : CustomFigure
    {
        public VPolygone(float thickness, Color thicknessColor, Color Сolor, Point[] Points)
        {
            this.thickness = thickness; //Толщина линии
            this.thicknessColor = thicknessColor; //Цвет линии
            this.color = Сolor; //Цвет заливки
            PointsIDs = new int[Points.Length + 5]; //Точки, + точки выделения и центр
            pointCount = Points.Length;
            Point max = Points[0], min = Points[0];
            for (int i = 0; i < PointsIDs.Length - 5; i++) //Перебираем список точек, от 0 до n-1, где n - количество углов, т.к. центр и точки выделения нужно вычислить
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
        public override void Draw(Graphics g) //Отрисовывание многоугольника
        {
            Point[] points = new Point[PointsIDs.Length - 5];
            for (int i = 0; i < PointsIDs.Length - 5; i++) //Перебираем список точек от 0 до n-1, где n - количество углов
            {

                MyPoint? mypoint = Vector.FindPbyID(PointsIDs[i]); //Ищем данную точку
                if (mypoint == null) return; //если точка не найдена, не рисуем
                points[i] = Vector.FindPbyID(PointsIDs[i]).Value.ConvertToPoint(); //Конвертируем
            }

            //Рисуем многоугольник, пользуемся drawPolygon, передаем все точки
            if (!color.IsEmpty) //Если у нас есть цвет заливки
            {
                g.FillPolygon(new SolidBrush(color), points); //Заливка

            }
            if (!thicknessColor.IsEmpty) //Если у нас есть цвет контура
            {
                g.DrawPolygon(new Pen(thicknessColor, thickness), points); //Рисуем контур
            }
        }

        public override void Select(Graphics g) //Выбор многоугольника
        {
            Point[] points = new Point[PointsIDs.Length - 5];
            for (int i = 0; i < PointsIDs.Length - 5; i++)
            { // Берем длину на 5 меньше, т.к. мы отрисовываем точки и выделяем сам многоугольник
                MyPoint? point = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (point == null) return; //если точки нет, не рисуем
                points[i] = point.Value.ConvertToPoint(); //Конвертируем в Point, записываем чтоб потом отрисовать выделение многоугольника
                //Показываем точку многоугольника
                g.FillRectangle(new SolidBrush(SettingsAndModes.EditPointColor), point.Value.X - 2, point.Value.Y - 2, 5, 5);
            }

            g.DrawPolygon(new Pen(SettingsAndModes.EditLineColor, 1), points); //Выделяем многоугольник
            DrawCenter(g); //Рисуем центр
        }

        public override void DrawSelectArea(Graphics g) //Функция рисования выделителя
        {
            Point min = new Point(Int32.MaxValue, Int32.MaxValue), max = new Point();
            for (int i = PointCount; i < PointsIDs.Length - 1; i++)
            { // Берем длину на 1 меньше, т.к. последняя точка это центр и начинаем с n элемента, где n - количество углов, т.к. нас интересует отрисовка прямоугольника-выделителя
                MyPoint? p = Vector.FindPbyID(PointsIDs[i]); //текущая точка
                if (p == null) return; //если точки нет, не рисуем
                //Ищем максимальную позицию и минимальную
                if (p.Value.X < min.X) min.X = p.Value.X; //Если у данной точки X меньше, назначаем в переменную
                if (p.Value.Y < min.Y) min.Y = p.Value.Y; //Если у данной точки Y меньше, назначаем в переменную
                if (p.Value.X > max.X) max.X = p.Value.X; //Если у данной точки X больше, назначаем в переменную
                if (p.Value.Y > max.Y) max.Y = p.Value.Y; //Если у данной точки Y больше, назначаем в переменную
            }
            g.DrawRectangle(new Pen(SettingsAndModes.EditLineColor, 1), min.X, min.Y, max.X - min.X, max.Y - min.Y); //рисуем контур выделения многоугольника прямоугольником
            for (int i = PointCount; i < PointsIDs.Length - 1; i++)
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
            MyPoint? center = Vector.FindPbyID(PointsIDs[PointsIDs.Length - 1]); //центр многоугольника
            if (center == null) return; //если нет точки, не рисуем
            //Показываем центр многоугольника
            g.FillRectangle(new SolidBrush(SettingsAndModes.CenterPointColor), center.Value.X - 2, center.Value.Y - 2, 5, 5);
        }

        public override void RecalculateCenter() //Пересчет центра
        {
            MyPoint? begin = Vector.FindPbyID(PointsIDs[PointCount]); //первая точка прямоугольника выделителя
            MyPoint? end = Vector.FindPbyID(PointsIDs[PointCount + 1]); //последняя точка прямоугольника выделителя
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
            MyPoint? select1 = Vector.FindPbyID(GetPointsIDs()[pointCount]);
            Vector.SetCoordsP(min.X, min.Y, select1);
            //Вторая точка выделения
            MyPoint? select2 = Vector.FindPbyID(GetPointsIDs()[pointCount + 1]);
            Vector.SetCoordsP(max.X, max.Y, select2);
            //Третья точка выделения
            MyPoint? select3 = Vector.FindPbyID(GetPointsIDs()[pointCount + 2]);
            Vector.SetCoordsP(min.X, max.Y, select3);
            //Четвертая точка выделения
            MyPoint? select4 = Vector.FindPbyID(GetPointsIDs()[pointCount + 3]);
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
            return new VPolygone(thickness, thicknessColor, color, points); //Создаем объект и возвращаем
        }
    }
}
