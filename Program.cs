﻿using System;
using System.Collections.Generic;

namespace LaXiS.VantagePointTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var items = new List<Point>();
            for (int i = 0; i < 1000000; i++)
            {
                Point point = new Point($"Point{i}", random.Next(10000001));
                items.Add(point);
                // Console.WriteLine($"{point.Key} {point.Value}");
            }

            var vpTree = new VantagePointTree<Point>(items);

            var targetPoint = new Point("PointSearch", random.Next(10000001));
            Console.WriteLine($"Search: {targetPoint}");

            var results = vpTree.Search(targetPoint, 100);
            foreach (var result in results)
            {
                Console.WriteLine($"{result}");
            }
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
