using System.Collections.Generic;

namespace LaXiS.VantagePointTree
{
    public class TreeItemComparer<T> : Comparer<T> where T : ITreeItem<T>
    {
        private readonly T _baseItem;

        public TreeItemComparer(T baseItem)
        {
            _baseItem = baseItem;
        }

        public override int Compare(T a, T b)
        {
            double aDist = a.DistanceFrom(_baseItem);
            double bDist = b.DistanceFrom(_baseItem);

            return aDist == bDist ? 0 : (aDist < bDist ? -1 : 1);
        }
    }
}