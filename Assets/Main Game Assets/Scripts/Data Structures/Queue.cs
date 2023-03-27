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

public class CircQ <T>
{
    private int[] queue = new int[10];
    private int fPointer, rPointer = -1;

    public int First()
    {
        int first = queue[fPointer];
        return first;
    }

    public bool IsFull()
    {
        int l = queue.Length;
        if (rPointer + 1 % l == fPointer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Enqueue(int item)
    {
        if (IsFull() != true)
        {

        }
    }
}
