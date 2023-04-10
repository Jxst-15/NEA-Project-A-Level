using System.Collections.Generic;
using UnityEngine;

public class MyStack <T>
{
    #region Fields
    private List<T> stack;
    #endregion

    public MyStack(int size)
    {
        this.stack = new List<T>();
    }

    public int Size()
    {
        return stack.Count;
    }

    public bool IsEmpty()
    {
        bool empty = true;
        if (stack.Count == 0)
        {
            empty = true;
        }
        else
        {
            empty = false;
        }
        return empty;
    }

    public T Top()
    {
        int top = Size() - 1;
        if (Size() <= 0)
        {
            Debug.Log("Stack is empty!");
        }
        return stack[top];
    }

    public void Push(T item)
    {
        stack.Add(item);
    }

    public void Pop()
    {
        int top = Size() - 1;
        stack.RemoveAt(top);
    }
}
