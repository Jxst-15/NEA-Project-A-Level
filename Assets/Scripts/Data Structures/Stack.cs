using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private List<GameObject> stack;

    public Stack()
    {
        this.stack = new List<GameObject>();
    }

    public int size()
    { 
        return stack.Count;
    }

    public bool isEmpty()
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

    public GameObject top()
    {
        int top = size() - 1;
        if (size() <= 0)
        {
            Debug.Log("Stack is empty!");
        }
        return stack[top];
    }

    public void push(GameObject item)
    {
        stack.Add(item);
    }

    public void pop()
    {
        int top = size() - 1;
        stack.RemoveAt(top);
    }
}
