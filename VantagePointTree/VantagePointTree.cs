using System;
using System.Collections.Generic;

namespace LaXiS.VantagePointTree
{
    public delegate double DistanceFunction<T>(T a, T b);

    public class VantagePointTree<T>
    {
        private readonly Random _random;
        private readonly DistanceFunction<T> _distanceFunction;
        private TreeNode<T> _rootNode;

        public VantagePointTree(
            DistanceFunction<T> distanceFunction)
        {
            _random = new Random();
            _distanceFunction = distanceFunction;
        }

        public VantagePointTree(
            DistanceFunction<T> distanceFunction,
            List<T> items)
            : this(distanceFunction)
        {
            Build(items);
        }

        public void Build(List<T> items)
        {
            // Build on a shallow copy of the list, so we do not modify the original list
            _rootNode = BuildRecursive(items.GetRange(0, items.Count));
            // _rootNode.Dump();
        }

        private TreeNode<T> BuildRecursive(List<T> items)
        {
            if (items.Count == 0)
                return null;

            TreeNode<T> node = new TreeNode<T>();

            if (items.Count == 1)
            {
                node.Item = items[0];
            }
            else
            {
                // Choose a random Vantage Point (swap first element with a random other element)
                int randomIndex = _random.Next(1, items.Count);
                items.Swap(0, randomIndex);

                // Save current item and shallow copy list excluding this item
                node.Item = items[0];
                items = items.GetRange(1, items.Count - 1);

                // TODO optimizable (see selection algorithm)
                // Sort this item's children by their distance from it and take the item in the middle:
                // the current item's radius is its distance from this median child
                items.Sort(new TreeItemComparer<T>(node.Item, _distanceFunction));
                int medianIndex = items.Count / 2 - 1;
                if (medianIndex < 0) medianIndex = 0;
                node.Radius = _distanceFunction(node.Item, items[medianIndex]);

                // Recursively build left and right subtrees, splitting the list in half at the median item
                // to keep the tree balanced.
                // The median point is part of the inside subtree
                node.LeftNode = BuildRecursive(items.GetRange(0, medianIndex + 1));
                node.RightNode = BuildRecursive(items.GetRange(medianIndex + 1, items.Count - medianIndex - 1));
            }

            return node;
        }

        /// <summary>
        /// Search tree for k nearest neighbors to target
        /// </summary>
        public List<TreeSearchResult<T>> Search(T target, int k)
        {
            var results = new List<TreeSearchResult<T>>();
            double tau = double.MaxValue;

            SearchRecursive(target, k, _rootNode, ref tau, results);

            return results;
        }

        private void SearchRecursive(T target, int k, TreeNode<T> node, ref double tau, List<TreeSearchResult<T>> results)
        {
            // k = number of nearest neighbors to search for
            // tau = longest distance in current results, must be initialized to double.MaxValue and passed as a reference

            if (node == null)
                return;

            // Calculate target's distance from the current examined node
            double distance = _distanceFunction(target, node.Item);
            //Console.WriteLine("VPItem={{{0}}} VPRadius={1} Distance={2} Tau={3} Results={4}", node.Item, node.Radius, distance, tau, results.Count);

            // Add result if distance to current node is shorter than longest distance currently in results.
            // Tau is updated only once we have at least k results, otherwise we risk returning too early
            // (consider the case when the first vantage point is very close to the target)
            if (distance < tau)
            {
                // Remove result with longest distance if results is full
                if (results.Count == k)
                    results.RemoveAt(results.Count - 1);

                // Add current node to results
                results.Add(new TreeSearchResult<T>(node.Item, distance));

                if (results.Count == k)
                {
                    // Sort results by ascending distance and update longest distance in results (tau)
                    results.Sort((a, b) => Comparer<double>.Default.Compare(a.Distance, b.Distance));
                    tau = results[^1].Distance;
                }
            }

            // Note: by searching the appropriate subtree first, we cut the searched nodes to a minimum since
            // there is a higher probability for neighbors to be on the same side of the radius than on the other.
            // Also, tau shrinks every time we find a closer node, therefore we can skip searching an entire subtree
            // if all found results are on the same side
            if (distance < node.Radius)
            {
                // Always search left (inside) subtree if target is within radius
                //Console.WriteLine("Searching INSIDE INSIDE");
                SearchRecursive(target, k, node.LeftNode, ref tau, results);

                // Search right (outside) subtree only if there are results outside radius
                if (distance + tau > node.Radius)
                {
                    //Console.WriteLine("Searching INSIDE OUTSIDE");
                    SearchRecursive(target, k, node.RightNode, ref tau, results);
                }
            }
            else
            {
                // Always search right (outside) subtree if target is outside radius
                //Console.WriteLine("Searching OUTSIDE OUTSIDE");
                SearchRecursive(target, k, node.RightNode, ref tau, results);

                // then search left (inside) subtree if there are results within radius
                if (distance - tau <= node.Radius)
                {
                    //Console.WriteLine("Searching OUTSIDE INSIDE");
                    SearchRecursive(target, k, node.LeftNode, ref tau, results);
                }
            }
        }
    }
}