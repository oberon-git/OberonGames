using System.Collections.Generic;
using System.Linq;

namespace War
{
    public class Point
    {
        #region Public Properties

        public int X { get; set; }

        public int Y { get; set; }

        #endregion

        #region Constructor

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"x = {X}, y = {Y}";
        }

        #endregion

        #region Static Methods

        public static List<Point> GetPerpendiculars()
        {
            return new List<Point>
        {
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1),
            new Point(1, 0),
        };
        }

        public static List<Point> GetDiagonals()
        {
            return new List<Point>
        {
            new Point(-1, -1),
            new Point(-1, 1),
            new Point(1, -1),
            new Point(1, 1),
        };
        }

        public static List<Point> GetAllDirections()
        {
            return GetPerpendiculars().Concat(GetDiagonals()).ToList();
        }

        #endregion

        #region Operator Overloads

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, int b)
        {
            return new Point(a.X * b, a.Y * b);
        }

        public static Point operator /(Point a, int b)
        {
            return new Point(a.X / b, a.Y / b);
        }

        #endregion
    }
}