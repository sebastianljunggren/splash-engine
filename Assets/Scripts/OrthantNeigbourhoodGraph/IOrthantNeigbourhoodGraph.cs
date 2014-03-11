using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrthantNeigbourhoodGraph<T> where T : class, OrthantNeigbourhoodGraphNode<T>
{
    int Count { get; }
    void Insert(T newNode);
    void Insert(T newNode, T insertedNode);
    void Remove(T node);
    void Move(T node, Vector3 newPosition);
    T NearestNeighbour(Vector3 point, T insertedNode);
    T NearestNeighbour(Vector3 point);
    List<T> RangeSearch(T node, Vector3 newPosition);
    List<T> All();
}
