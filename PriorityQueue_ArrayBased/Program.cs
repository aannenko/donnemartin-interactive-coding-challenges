using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

var queue = new PriorityQueue<char>();
queue.Insert(new('a', 20));
queue.Insert(new('b', 5));
queue.Insert(new('c', 15));
queue.Insert(new('d', 22));
queue.Insert(new('e', 40));
queue.Insert(new('f', 3));
Debug.Assert(queue.TrySetKey('f', 2));
Debug.Assert(queue.TrySetKey('a', 19));

var minKeys = new List<int>(6);
while (queue.TryExtractMin(out var node))
    minKeys.Add(node.Key);

var expectedMinKeys = new int[] { 2, 5, 15, 19, 22, 40 };
for (int i = 0; i < expectedMinKeys.Length; i++)
    Debug.Assert(expectedMinKeys[i] == minKeys[i]);

sealed class PriorityQueueNode<T>(T item, int key) where T : IEquatable<T>
{
    public T Item => item;

    public int Key { get; set; } = key;
}

sealed class PriorityQueue<T> where T : IEquatable<T>
{
    private readonly List<PriorityQueueNode<T>> _list = [];

    public void Insert(PriorityQueueNode<T> node)
    {
        _list.Add(node);
    }

    public bool TryExtractMin([NotNullWhen(true)] out PriorityQueueNode<T>? node)
    {
        node = null;
        if (_list.Count == 0)
            return false;

        var minKey = _list[0].Key;
        var minIndex = 0;
        for (int i = 1; i < _list.Count; i++)
        {
            if (_list[i].Key < minKey)
            {
                minIndex = i;
                minKey = _list[i].Key;
            }
        }

        node = _list[minIndex];
        _list.RemoveAt(minIndex);
        return true;
    }

    public bool TrySetKey(T item, int key)
    {
        foreach (var node in _list)
        {
            if (node.Item.Equals(item))
            {
                node.Key = key;
                return true;
            }
        }

        return false;
    }
}