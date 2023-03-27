using System.Collections.Generic;
using UnityEngine;

// Need to turn into array/other
// Maybe use for PlayerAction
public class Queue <T>
{
    #region Fields
    private List<T> queue;
    private int front = 0;
    private int rear = -1;
    #endregion

    public Queue()
    {
        queue = new List<T>();
    }

    public T First()
    {
        T first = queue[front];
        return first;
    }

    public int Size()
    {
        int size = 0;
        if (rear > front)
        {
            size = rear - front + 1;
        }
        return size;
    }

    public bool IsFull()
    {
        int length = queue.Count;

        if (rear < length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public T Dequeue()
    {
        if (rear < front)
        {
            Debug.Log("Error! Empty queue");
            return default(T);
        }
        else
        {
            front += 1;
            return queue[front - 1];
        }
    }

    public void Enqueue(T item)
    {
        queue.Add(item);
        rear += 1;
    }
}
