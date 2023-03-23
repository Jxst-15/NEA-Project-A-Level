using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    #region Fields
    private List<GameObject> queue;
    private int front = 0;
    private int rear = -1;
    #endregion

    public Queue()
    {
        queue = new List<GameObject>();
    }

    public GameObject first()
    {
        GameObject first = queue[front];
        return first;
    }

    public int size()
    {
        int size = 0;
        if (rear > front)
        {
            size = rear - front + 1;
        }
        return size;
    }

    public bool isFull()
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

    public GameObject dequeue()
    {
        if (rear < front)
        {
            Debug.Log("Error! Empty queue");
            return null;
        }
        else
        {
            front += 1;
            return queue[front - 1];
        }
    }

    public void enqueue(GameObject item)
    {
        queue.Add(item);
        rear += 1;
    }
}
