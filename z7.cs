using System;
using System.IO;
public delegate int PriorityQueueComparer<T>(T x, T y);

public class MyPriorityQueue<T>
{
    private T[] queue;
    private int size;
    private readonly PriorityQueueComparer<T> comparator;

    private const int DEFAULT_CAPACITY = 11;

    public MyPriorityQueue()
        : this(DEFAULT_CAPACITY, null)
    {
    }
    public MyPriorityQueue(T[] a)
        : this(a?.Length ?? DEFAULT_CAPACITY, null)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            Add(item);
    }
    public MyPriorityQueue(int initialCapacity)
        : this(initialCapacity, null)
    {
    }
    public MyPriorityQueue(int initialCapacity, PriorityQueueComparer<T> comparator)
    {
        if (initialCapacity < 0)
            throw new ArgumentException("Начальная ёмкость < 0");

        queue = new T[initialCapacity];
        size = 0;
        this.comparator = comparator;
    }
    public MyPriorityQueue(MyPriorityQueue<T> c)
        : this(c?.size ?? DEFAULT_CAPACITY, c?.comparator)
    {
        if (c == null)
            throw new ArgumentNullException(nameof(c));

        for (int i = 0; i < c.size; i++)
            Add(c.queue[i]);
    }
    public bool Add(T e)
    {
        if (e == null)
            throw new ArgumentNullException(nameof(e));

        EnsureCapacity(size + 1);
        queue[size] = e;
        HeapifyUp(size);
        size++;
        return true;
    }
    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            Add(item);
    }
    public void Clear()
    {
        queue = new T[queue.Length];
        size = 0;
    }
    public bool Contains(object o)
    {
        for (int i = 0; i < size; i++)
            if (Equals(queue[i], o))
                return true;

        return false;
    }
    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            if (!Contains(item))
                return false;

        return true;
    }
    public bool IsEmpty() => size == 0;
    public bool Remove(object o)
    {
        for (int i = 0; i < size; i++)
        {
            if (Equals(queue[i], o))
            {
                RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public void RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            while (Remove(item)) { }
    }
    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        for (int i = 0; i < size; i++)
        {
            if (Array.IndexOf(a, queue[i]) < 0)
            {
                RemoveAt(i);
                i--;
            }
        }
    }
    public int Size() => size;
    public object[] ToArray()
    {
        object[] result = new object[size];
        Array.Copy(queue, result, size);
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < size)
            a = new T[size];

        Array.Copy(queue, a, size);
        return a;
    }
    public T Element()
    {
        if (size == 0)
            throw new InvalidOperationException("Очередь пуста");

        return queue[0];
    }
    public bool Offer(T obj)
    {
        if (obj == null)
            return false;

        return Add(obj);
    }
    public T Peek()
    {
        return size == 0 ? default : queue[0];
    }
    public T Poll()
    {
        if (size == 0)
            return default;

        T result = queue[0];
        RemoveAt(0);
        return result;
    }
    private void RemoveAt(int index)
    {
        size--;
        queue[index] = queue[size];
        queue[size] = default;
        HeapifyDown(index);
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (queue.Length >= minCapacity)
            return;

        int newCapacity;
        if (queue.Length < 64)
            newCapacity = queue.Length + 2;
        else
            newCapacity = queue.Length + queue.Length / 2;

        if (newCapacity < minCapacity)
            newCapacity = minCapacity;

        Array.Resize(ref queue, newCapacity);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (Compare(queue[index], queue[parent]) >= 0)
                break;

            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int smallest = index;

            if (left < size && Compare(queue[left], queue[smallest]) < 0)
                smallest = left;

            if (right < size && Compare(queue[right], queue[smallest]) < 0)
                smallest = right;

            if (smallest == index)
                break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private int Compare(T a, T b)
    {
        if (comparator != null)
            return comparator(a, b);

        return Comparer<T>.Default.Compare(a, b);
    }

    private void Swap(int i, int j)
    {
        T temp = queue[i];
        queue[i] = queue[j];
        queue[j] = temp;
    }
}
public class Request
{
    public int Id { get; }
    public int Priority { get; }
    public int StepIn { get; }

    public Request(int id, int priority, int stepIn)
    {
        Id = id;
        Priority = priority;
        StepIn = stepIn;
    }

    public override string ToString()
    {
        return $"Заявка #{Id}, приоритет={Priority}, шаг поступления={StepIn}";
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите количество шагов N: ");
        int N = int.Parse(Console.ReadLine());

        static int requestComparator(Request a, Request b)
        {
            int cmp = b.Priority.CompareTo(a.Priority);
            if (cmp != 0)
                return cmp;

            return a.Id.CompareTo(b.Id);
        }

        var queue = new MyPriorityQueue<Request>(11, requestComparator);
        var random = new Random();

        int currentStep = 0;
        int requestId = 1;

        int maxWaitTime = -1;
        Request maxWaitRequest = null;

        using StreamWriter log = new StreamWriter("log.txt");
        for (currentStep = 1; currentStep <= N; currentStep++)
        {
            int count = random.Next(1, 11);

            for (int i = 0; i < count; i++)
            {
                int priority = random.Next(1, 6);
                var req = new Request(requestId++, priority, currentStep);

                queue.Add(req);
                log.WriteLine($"ADD {req.Id} {req.Priority} {currentStep}");
            }

            RemoveAndProcess(queue, currentStep, log,
                ref maxWaitTime, ref maxWaitRequest);
        }
        while (!queue.IsEmpty())
        {
            currentStep++;
            RemoveAndProcess(queue, currentStep, log,
                ref maxWaitTime, ref maxWaitRequest);
        }
        Console.WriteLine("\nЗаявка с максимальным временем ожидания:");
        Console.WriteLine(maxWaitRequest);
        Console.WriteLine($"Время ожидания: {maxWaitTime}");
    }
    static void RemoveAndProcess(
        MyPriorityQueue<Request> queue,
        int currentStep,
        StreamWriter log,
        ref int maxWaitTime,
        ref Request maxWaitRequest)
    {
        if (queue.IsEmpty())
            return;

        Request req = queue.Poll();

        int waitTime = currentStep - req.StepIn;

        log.WriteLine($"REMOVE {req.Id} {req.Priority} {currentStep}");

        if (waitTime > maxWaitTime)
        {
            maxWaitTime = waitTime;
            maxWaitRequest = req;
        }
    }
}
