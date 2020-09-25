using System.Collections.Generic;

namespace LaXiS.VantagePointTree
{
    public class TreeItemComparer<T> : Comparer<T>
    {
        private readonly T _baseItem;
        private readonly DistanceFunction<T> _distanceFunction;

        /// <summary>
        /// Comparer for ordering points based on their distances from a base point
        /// </summary>
        public TreeItemComparer(
            T baseItem,
            DistanceFunction<T> distanceFunction)
        {
            _baseItem = baseItem;
            _distanceFunction = distanceFunction;
        }

        public override int Compare(T a, T b)
        {
            double aDist = _distanceFunction(a, _baseItem);
            double bDist = _distanceFunction(b, _baseItem);

            return aDist == bDist ? 0 : (aDist < bDist ? -1 : 1);
        }
    }
}