using System.Collections;
using System.Runtime.InteropServices;
using HuffmanDecoder.Objects;

namespace HuffmanDecoder.Util;

public class MinHeap
{
    private List<Node> Nodes = new();

    private Node? Root = null;

    public void Push(Node node)
    {
        Nodes.Add(node);
        var child = Size() - 1;

        while (true)
        {
            var parent = (child - 1) / 2;

            if (parent < 0) parent = 0;

            if (Nodes[parent].IsRight(Nodes[child])) return;

            Swap(parent, child);

            child = parent;

            if (child <= 0) return;
        }
    }

    public Node Pop()
    {
        var node = Nodes.First();
        var last = Nodes.Last();

        Nodes.Remove(last);

        if (Size() == 0) return node;

        Nodes[0] = last;

        var parent = 0;
        var child = 1;

        while (child < Size())
        {
            if (child + 1 < Size() && Nodes[child + 1].IsLeft(Nodes[child])) child += 1;

            if (Nodes[parent].IsRight(Nodes[child]))
                return node;

            Swap(parent, child);

            parent = child;

            child = 2 * child + 1;
        }

        return node;
    }

    private void Swap(int parent, int child)
    {
        (Nodes[parent], Nodes[child]) = (Nodes[child], Nodes[parent]);
    }

    public int Size()
    {
        return Nodes.Count;
    }

    public Node RootNode()
    {
        return Root ??= Pop();
    }
}