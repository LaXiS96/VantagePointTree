using System;

namespace LaXiS.VantagePointTree
{
    public class TreeNode<T>
    {
        /// <summary>
        /// Vantage Point for this tree level
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Distance radius of the bounding sphere/circle which splits the left (inside) and right (outside) subtrees
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Inner points subtree
        /// </summary>
        public TreeNode<T> LeftNode { get; set; }

        /// <summary>
        /// Outer points subtree
        /// </summary>
        public TreeNode<T> RightNode { get; set; }

        public void Dump(int step = 0)
        {
            Console.WriteLine($"{new string(' ', step * 2)}{Item} Radius:{Radius}");
            if (LeftNode != null)
                LeftNode.Dump(step + 1);
            if (RightNode != null)
                RightNode.Dump(step + 1);
        }
    }
}