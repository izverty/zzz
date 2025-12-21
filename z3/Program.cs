using System;
using System.Collections.Generic;

public class Heap<T>
{
    private readonly List<T> _data;
    private readonly IComparer<T> _comparer;
    public Heap(IEnumerable<T> items, IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
        _data = new List<T>(items);
        BuildHeap();
    }
    public Heap(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
        _data = new List<T>();
    }

    public int Count => _data.Count;

    public T Peek()
    {
        if (_data.Count == 0)
            throw new InvalidOperationException("Куча пуста");

        return _data[0];
    }
    public T ExtractRoot()
    {
        if (_data.Count == 0)
            throw new InvalidOperationException("Куча пуста");

        T root = _data[0];
        int lastIndex = _data.Count - 1;

        _data[0] = _data[lastIndex];
        _data.RemoveAt(lastIndex);

        if (_data.Count > 0)
            HeapifyDown(0);

        return root;
    }
    public void ChangeKey(int index, T newValue)
    {
        if (index < 0 || index >= _data.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        T oldValue = _data[index];
        _data[index] = newValue;
        if (_comparer.Compare(newValue, oldValue) > 0)
            HeapifyUp(index);
        else
            HeapifyDown(index);
    }
    public void Add(T item)
    {
        _data.Add(item);
        HeapifyUp(_data.Count - 1);
    }
    public Heap<T> Merge(Heap<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        List<T> merged = new List<T>(_data.Count + other._data.Count);
        merged.AddRange(_data);
        merged.AddRange(other._data);

        return new Heap<T>(merged, _comparer);
    }
    private void BuildHeap()
    {
        for (int i = Parent(_data.Count - 1); i >= 0; i--)
            HeapifyDown(i);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = Parent(index);
            if (_comparer.Compare(_data[index], _data[parent]) <= 0)
                break;

            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int left = Left(index);
            int right = Right(index);
            int best = index;

            if (left < _data.Count &&
                _comparer.Compare(_data[left], _data[best]) > 0)
                best = left;

            if (right < _data.Count &&
                _comparer.Compare(_data[right], _data[best]) > 0)
                best = right;

            if (best == index)
                break;

            Swap(index, best);
            index = best;
        }
    }

    private static int Parent(int i) => (i - 1) / 2;
    private static int Left(int i) => 2 * i + 1;
    private static int Right(int i) => 2 * i + 2;

    private void Swap(int i, int j)
    {
        (_data[i], _data[j]) = (_data[j], _data[i]);
    }
}
