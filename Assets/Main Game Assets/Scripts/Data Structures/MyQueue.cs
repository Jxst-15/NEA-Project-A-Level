using System.Collections.Generic;
using UnityEngine;

// A generic class which defines the Queue object
public class MyQueue <T>
{
    #region Fields
    private List<T> queue;
    private int head = 0;
    private int tail = -1;
    #endregion

    public MyQueue()
    {
        queue = new List<T>();
    }

    // Returns the first item in the queue
    public T First()
    {
        T first = queue[head];
        return first;
    }

    // Returns the size of the queue
    public int Size()
    {
        int size = 0;
        if (tail >= head)
        {
            size = tail - head + 1;
        }
        return size;
    }

    // Queues the given parameter
    public void Enqueue(T item)
    {
        queue.Add(item);
        tail += 1;
    }

    // Removes the first item in the queue and adjusts the head
    public T Dequeue()
    {
        if (tail < head)
        {
            // Debug.Log("Error! Empty queue");
            return default;
        }
        else
        {
            head += 1;
            Debug.Log("Dequed");
            return queue[head - 1];
        }
    }
}
