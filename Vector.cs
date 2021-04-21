using System;
using System.Drawing;
using System.Collections.Generic; 

public static class Vector
{
    private static LinkedList<Point> points = new LinkedList<Point>();

    public static Point getFirstP()
    {
        Console.Write(points.First.Value);
        return points.First.Value;
    }
}
