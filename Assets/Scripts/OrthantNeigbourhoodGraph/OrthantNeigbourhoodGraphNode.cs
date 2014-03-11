using UnityEngine;
using System.Collections;

public interface OrthantNeigbourhoodGraphNode<T> where T : class, OrthantNeigbourhoodGraphNode<T>
{
    Vector3 Position { get; set; }
    T TopFrontRight { get; set; }
    T TopBackRight { get; set; }
    T TopBackLeft { get; set; }
    T TopFrontLeft { get; set; }
    T BottomFrontRight { get; set; }
    T BottomBackRight { get; set; }
    T BottomBackLeft { get; set; }
    T BottomFrontLeft { get; set; }
    bool Visited { get; set; }
}
