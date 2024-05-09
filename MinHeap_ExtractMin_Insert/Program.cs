using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

var heap = new MinHeap<int>();

Debug.Assert(heap.TryExtractMin(out var value) is false);
heap.Insert(20);
Debug.Assert(heap.Items[0] is 20);
heap.Insert(5);
Debug.Assert(heap.Items[0] is 5);
Debug.Assert(heap.Items[1] is 20);
heap.Insert(15);
Debug.Assert(heap.Items[0] is 5);
Debug.Assert(heap.Items[1] is 20);
Debug.Assert(heap.Items[2] is 15);
heap.Insert(22);
Debug.Assert(heap.Items[0] is 5);
Debug.Assert(heap.Items[1] is 20);
Debug.Assert(heap.Items[2] is 15);
Debug.Assert(heap.Items[3] is 22);
heap.Insert(40);
Debug.Assert(heap.Items[0] is 5);
Debug.Assert(heap.Items[1] is 20);
Debug.Assert(heap.Items[2] is 15);
Debug.Assert(heap.Items[3] is 22);
Debug.Assert(heap.Items[4] is 40);
heap.Insert(3);
Debug.Assert(heap.Items[0] is 3);
Debug.Assert(heap.Items[1] is 20);
Debug.Assert(heap.Items[2] is 5);
Debug.Assert(heap.Items[3] is 22);
Debug.Assert(heap.Items[4] is 40);
Debug.Assert(heap.Items[5] is 15);

var mins = new List<int>(6);
while (heap.Count > 0)
{
    Debug.Assert(heap.TryExtractMin(out value));
    mins.Add(value);
}

var expectedMins = new List<int> { 3, 5, 15, 20, 22, 40 };
for (int i = 0; i < mins.Count; i++)
    Debug.Assert(mins[i] == expectedMins[i]);

sealed class MinHeap<T> where T : INumber<T>
{
    private readonly List<T> _list = [];

    public IReadOnlyList<T> Items => _list;

    public int Count => _list.Count;

    public void Insert(T item)
    {
        _list.Add(item);
        BubbleUp(Count - 1);
    }

    public bool TryExtractMin([NotNullWhen(true)] out T? value)
    {
        if (Count == 0)
        {
            value = default;
            return false;
        }

        value = _list[0];
        if (Count == 1)
        {
            _list.RemoveAt(0);
        }
        else
        {
            _list[0] = _list[Count - 1];
            _list.RemoveAt(Count - 1);
            BubbleDown(0);
        }

        return true;
    }

    private void BubbleUp(int index)
    {
        if (index is 0)
            return;

        var parentIndex = (index - 1) / 2;
        if (_list[index] < _list[parentIndex])
        {
            (_list[index], _list[parentIndex]) = (_list[parentIndex], _list[index]);
            BubbleUp(parentIndex);
        }
    }

    private void BubbleDown(int index)
    {
        var smallerChildIndex = FindSmallerChildIndex(index);
        if (smallerChildIndex > -1 && _list[index] > _list[smallerChildIndex])
        {
            (_list[index], _list[smallerChildIndex]) = (_list[smallerChildIndex], _list[index]);
            BubbleDown(smallerChildIndex);
        }
    }

    private int FindSmallerChildIndex(int index)
    {
        var leftChildIndex = index * 2 + 1;
        var rightChildIndex = index * 2 + 2;
        if (rightChildIndex >= Count)
            return leftChildIndex >= Count
                ? -1
                : leftChildIndex;
        else
            return _list[leftChildIndex] < _list[rightChildIndex]
                ? leftChildIndex
                : rightChildIndex;
    }
}