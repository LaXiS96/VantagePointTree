using System;
using System.Collections.Generic;

namespace LaXiS.VantagePointTree
{
    public class VantagePointTree<T> where T : ITreeItem<T>
    {
        private readonly Random _random;
        private TreeNode<T> _rootNode;

        public VantagePointTree()
        {
            _random = new Random();
        }

        public VantagePointTree(List<T> items) : this()
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
                // Swap the first element with a random other element
                int randomIndex = _random.Next(1, items.Count);
                items.Swap(0, randomIndex);

                // Save current item and shallow copy list excluding this item
                node.Item = items[0];
                items = items.GetRange(1, items.Count - 1);

                // TODO optimizable (see selection algorithm)
                // Sort this item's children by their distance from it and take the item in the middle:
                // the current item's radius is its distance from this median child
                items.Sort(new TreeItemComparer<T>(node.Item));
                int medianIndex = items.Count / 2 - 1;
                if (medianIndex < 0) medianIndex = 0;
                node.Radius = node.Item.DistanceFrom(items[medianIndex]);

                // Recursively build left and right subtrees, splitting the list in half at the median item
                // Children end up 50/50 on either subtree (this also keeps the tree balanced)
                node.LeftNode = BuildRecursive(items.GetRange(0, medianIndex + 1));
                node.RightNode = BuildRecursive(items.GetRange(medianIndex + 1, items.Count - medianIndex - 1));
            }

            return node;
        }

        // Search tree for k nearest neighbors to target
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
            double distance = target.DistanceFrom(node.Item);

            // If distance to current node is shorter than longest distance currently in results
            // (this is always true for the first k passes, since tau only gets updated once we find
            // all k supposed nearest neighbors)
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

            // Note: by searching the appropriate subtree first (left if target is inside, right if target is outside)
            // we cut the searched nodes to a minimum, since there is a higher probability for neighbors to be
            // on the same side of the radius than on the other. Also, tau shrinks every time we find a closer node,
            // therefore we can skip searching an entire subtree if all found results are on the same side
            if (distance < node.Radius)
            {
                // If target is within radius, build left (inside) subtree first if there are results within radius,
                // then build right (outside) subtree if there are results outside radius
                if (distance - tau < node.Radius)
                    SearchRecursive(target, k, node.LeftNode, ref tau, results);
                if (distance + tau >= node.Radius)
                    SearchRecursive(target, k, node.RightNode, ref tau, results);
            }
            else
            {
                // If target is outside radius, build right (outside) subtree first if there are results outside radius,
                // then build left (inside) subtree if there are results within radius
                if (distance + tau >= node.Radius)
                    SearchRecursive(target, k, node.RightNode, ref tau, results);
                if (distance - tau < node.Radius)
                    SearchRecursive(target, k, node.LeftNode, ref tau, results);
            }
        }
    }
}