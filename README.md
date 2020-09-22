# VantagePointTree

C# implementation of a vantage-point tree, a data structure which is well suited to similarity searching.

## Details

This implementation is mostly educational, as can be seen by all the explanatory comments spread around the code. No guarantees are given.

The solution includes:

- .NET Standard 2.1 class library implementation (`VantagePointTree`)
- xUnit.net testing project (`VantagePointTree.Tests`)
- .NET Core 3.1 example program (`VantagePointTree.Examples`)

The VantagePointTree class is generic and can accept any `T` that implements `ITreeItem<T>` (for the `DistanceFrom` distance calculation method).

## Features

- Build tree from a list of items
- Search tree for k nearest neighbors

## TODO (by priority)

1. Item insertion and removal (plus tree balancing if needed)
2. Search by max distance
3. Save and load to/from file

- Optimization (not a priority)

## References and inspirations

- https://en.wikipedia.org/wiki/Vantage-point_tree
- http://stevehanov.ca/blog/?id=130
- https://fribbels.github.io/vptree/writeup
- https://github.com/mcraiha/CSharp-vptree
