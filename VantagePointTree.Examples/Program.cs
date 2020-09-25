using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LaXiS.VantagePointTree.Examples
{
    class Program
    {
        static void Main()
        {
            var stopwatch = new Stopwatch();
            var random = new Random();

            // Create 1million random points (with values from 0 to 10million)
            stopwatch.Restart();
            var items = new List<Point>();
            for (int i = 0; i < 1000000; i++)
            {
                Point point = new Point($"Point{i}", random.Next(10000001));
                items.Add(point);
                // Console.WriteLine($"{point.Key} {point.Value}");
            }
            stopwatch.Stop();
            Console.WriteLine("Creation took {0}", stopwatch.Elapsed);

            // Build tree
            stopwatch.Restart();
            var vpTree = new VantagePointTree<Point>(Point.DistanceFunction, items);
            stopwatch.Stop();
            Console.WriteLine("Tree build took {0}", stopwatch.Elapsed);

            // Choose a random search target point
            var targetPoint = new Point("PointSearch", random.Next(10000001));
            Console.WriteLine($"Search: {targetPoint}");

            // Search the tree for points similar to targetPoint
            stopwatch.Restart();
            var results = vpTree.Search(targetPoint, 5);
            stopwatch.Stop();
            Console.WriteLine("Search took {0}", stopwatch.Elapsed);
            foreach (var result in results)
                Console.WriteLine($"{result}");
        }
    }

    public class Point
    {
        public string Key;
        public int Value;

        public Point(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public static double DistanceFunction(Point a, Point b)
        {
            return Math.Abs(a.Value - b.Value);
        }

        public override string ToString()
        {
            return $"Key={Key} Value={Value}";
        }
    }
}
