using System;
using System.Collections.Generic;

namespace LaXiS.VantagePointTree.Examples
{
    class Program
    {
        static void Main()
        {
            var random = new Random();

            // Create 1million random points (with values from 0 to 10million)
            var items = new List<Point>();
            for (int i = 0; i < 1000000; i++)
            {
                Point point = new Point($"Point{i}", random.Next(10000001));
                items.Add(point);
                // Console.WriteLine($"{point.Key} {point.Value}");
            }

            // Build tree
            var vpTree = new VantagePointTree<Point>(items);

            // Choose a random search target point
            var targetPoint = new Point("PointSearch", random.Next(10000001));
            Console.WriteLine($"Search: {targetPoint}");

            // Search the tree for 100 points similar to targetPoint
            var results = vpTree.Search(targetPoint, 100);
            foreach (var result in results)
                Console.WriteLine($"{result}");
        }
    }

    public class Point : ITreeItem<Point>
    {
        public string Key;
        public int Value;

        public Point(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public double DistanceFrom(Point p)
        {
            return Math.Abs(Value - p.Value);
        }

        public override string ToString()
        {
            return $"Key={Key} Value={Value}";
        }
    }
}
