using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2048;
public static class Utils
{
    public static T RandomEnumValue<T>()
    {
        Random rnd = new Random();
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(rnd.Next(v.Length));
    }

    public static List<List<T>> Swap<T>(this List<List<T>> matrix, Cairo.Point p1, Cairo.Point p2)
    {
        (matrix[p1.X][p1.Y], matrix[p2.X][p2.Y]) = (matrix[p2.X][p2.Y], matrix[p1.X][p1.Y]);

        return matrix;
    }

    public static bool HasDuplicates<T>(this IEnumerable<T> list, int allowedDistance)
    {
        HashSet<T> set = new HashSet<T>();
        for (int i = 0; i < list.Count(); i++)
        {
            if (set.Contains(list.ElementAt(i)))
                return true;
            set.Add(list.ElementAt(i));
            if (i >= allowedDistance)
                set.Remove(list.ElementAt(i - allowedDistance));
        }
        return false;
    }

    public static void ForEach2D<T>(this IEnumerable<IEnumerable<T>> lists, Action<T> action)
    {
        foreach (IEnumerable<T> list in lists)
            foreach (T item in list)
                action(item);
    }

    public static List<T> Column<T>(this IEnumerable<IEnumerable<T>> matrix, int columnNumber)
    {
        return matrix.ElementAt(columnNumber).ToList();
    }

    public static List<T> Row<T>(this IEnumerable<IEnumerable<T>> matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.First().Count())
                .Select(x => matrix.ElementAt(x).ElementAt(rowNumber))
                .ToList();
    }

    public static (int X, int Y) CoordinatesOf<T>(this IEnumerable<IEnumerable<T>> matrix, T value)
    {
        for (int row = 0; row < matrix.First().Count(); row++)
            for (int column = 0; column < matrix.Count(); ++column)
                if (matrix.ElementAt(row).ElementAt(column).Equals(value))
                    return (row, column);
        return (-1, -1);
    }

}