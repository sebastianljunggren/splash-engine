using System;
using UnityEngine;
using System.Collections.Generic;


public class Octree<T> where T : class, OctreeContent
{
    private OctreeNode<T> Top;

    private int MaxDepth;

    public Octree(Bounds bounds, int maxDepth)
    {
        MaxDepth = maxDepth;
        Top = new OctreeNode<T>(bounds, 0, maxDepth);
    }

    public void Add(T content)
    {
        Top.Add(content);
    }

    public void UpdatePosition(T content)
    {
        Top.Remove(content);
        Top.Add(content);
    }

    public void Remove(T content)
    {
        Top.Remove(content);
    }

    public void Clear()
    {
        Top = new OctreeNode<T>(Top.Bounds, 0, MaxDepth);
    }

    public List<T> GetList()
    {
        return Top.GetList(new List<T>());
    }

    public bool Contains(Vector3 point)
    {
        return Top.Contains(point);
    }
}

