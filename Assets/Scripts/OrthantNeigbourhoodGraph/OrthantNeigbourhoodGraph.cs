using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrthantNeigbourhoodGraph<T> : IOrthantNeigbourhoodGraph<T> where T : class, OrthantNeigbourhoodGraphNode<T>
{
    private T startNode;
    public int Count { get; private set; }

    public OrthantNeigbourhoodGraph()
    {
        Count = 0;
    }

    public void Insert(T newNode)
    {
        Insert(newNode, startNode);
    }

    public void Insert(T newNode, T insertedNode)
    {
        Count++;
        if (startNode == null)
        {
            // Graph is empty
            startNode = newNode;
            return;
        }
    }

    public void Remove(T node)
    {
        throw new System.NotImplementedException();
    }

    public void Move(T node, Vector3 newPosition)
    {
        throw new System.NotImplementedException();
    }

    public T NearestNeighbour(Vector3 point, T s)
    {
        T octant = Octant(s, point);
        float distance = Vector3.Distance(s.Position, point);
        do
        {
            Bounds searchBox = new Bounds(point, new Vector3(distance, distance, distance));
            s.Visited = true;
            List<T> otherOctants = OtherOctants(s, octant);
            foreach (T o in otherOctants)
            {
                if (NearestNeighbourInternal(o))
                {
                    break;
                }
            }
        } while (true);
    }

    public T NearestNeighbour(Vector3 point)
    {
        return NearestNeighbour(point, startNode);
    }

    private bool NearestNeighbourInternal(T n)
    {
        return false;
    }

    public List<T> RangeSearch(T node, Vector3 newPosition)
    {
        throw new System.NotImplementedException();
    }

    public List<T> All()
    {
        throw new System.NotImplementedException();
    }

    private T Octant(T node, Vector3 point)
    {
        if (point.x >= node.Position.x)
        {
            if (point.y >= node.Position.y)
            {
                if (point.z >= node.Position.z)
                {
                    return node.TopFrontRight;
                }
                else
                {
                    return node.TopBackRight;
                }
            }
            else
            {
                if (point.z >= node.Position.z)
                {
                    return node.BottomFrontRight;
                }
                else
                {
                    return node.BottomBackRight;
                }

            }
        }
        else
        {
            if (point.y >= node.Position.y)
            {
                if (point.z >= node.Position.z)
                {
                    return node.TopFrontLeft;
                }
                else
                {
                    return node.TopBackLeft;
                }
            }
            else
            {
                if (point.z >= node.Position.z)
                {
                    return node.BottomFrontLeft;
                }
                else
                {
                    return node.BottomBackLeft;
                }

            }

        }
    }

    private List<T> OtherOctants(T s, T octant)
    {
        List<T> octants = new List<T>(8);
        if (s.TopFrontRight != null && s.TopFrontRight != octant)
        {
            octants.Add(s.TopFrontRight);
        }
        if (s.TopBackRight != null && s.TopBackRight != octant)
        {
            octants.Add(s.TopBackRight);
        }
        if (s.TopBackLeft != null && s.TopBackLeft != octant)
        {
            octants.Add(s.TopBackLeft);
        }
        if (s.TopFrontLeft != null && s.TopFrontLeft != octant)
        {
            octants.Add(s.TopFrontLeft);
        }
        if (s.BottomFrontRight != null && s.BottomFrontRight != octant)
        {
            octants.Add(s.BottomFrontRight);
        }
        if (s.BottomBackRight != null && s.BottomBackRight != octant)
        {
            octants.Add(s.BottomBackRight);
        }
        if (s.BottomFrontLeft != null && s.BottomFrontLeft != octant)
        {
            octants.Add(s.BottomFrontLeft);
        }
        if (s.TopBackLeft != null && s.TopBackLeft != octant)
        {
            octants.Add(s.TopBackLeft);
        }
        return octants;
    }
}
