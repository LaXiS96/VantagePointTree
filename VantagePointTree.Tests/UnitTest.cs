using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace LaXiS.VantagePointTree.Tests
{
    public class UnitTest
    {
        private readonly ITestOutputHelper _output;

        public UnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var items = new List<Point>
            {
                new Point("Point0", 9),
                new Point("Point1", 48),
                new Point("Point2", 52),
                new Point("Point3", 75),
                new Point("Point4", 77),
                new Point("Point5", 35),
                new Point("Point6", 36),
                new Point("Point7", 61),
                new Point("Point8", 17),
                new Point("Point9", 57),
                new Point("Point10", 87),
                new Point("Point11", 8),
                new Point("Point12", 90),
                new Point("Point13", 45),
                new Point("Point14", 88),
                new Point("Point15", 64),
                new Point("Point16", 37),
                new Point("Point17", 12),
                new Point("Point18", 78),
                new Point("Point19", 51)
            };

            var vpTree = new VantagePointTree<Point>(Point.DistanceFunction, items);

            var targetPoint = new Point("PointSearch", 54);
            _output.WriteLine($"Search: {targetPoint}");

            var results = vpTree.Search(targetPoint, 5);
            foreach (var result in results)
                _output.WriteLine($"{result}");

            // TODO must implement IEquatable on Point for this to work as is
            // Assert.Equal(new TreeSearchResult<Point>(new Point("Point2", 52), 2), results[0]);
            // Assert.Equal(new TreeSearchResult<Point>(new Point("Point9", 57), 3), results[1]);
            // Assert.Equal(new TreeSearchResult<Point>(new Point("Point19", 51), 3), results[2]);
            // Assert.Equal(new TreeSearchResult<Point>(new Point("Point1", 48), 6), results[3]);
            // Assert.Equal(new TreeSearchResult<Point>(new Point("Point7", 61), 7), results[4]);
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
