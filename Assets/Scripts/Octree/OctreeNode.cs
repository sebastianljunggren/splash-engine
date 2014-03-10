using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class OctreeNode<T> where T : class, OctreeContent
{
    private bool _IsLeaf;
    private int Level;
    private int MaxLevel;
    private List<T> _Contents;
    private OctreeNode<T>[] Children;
    private Bounds ActualBounds;

    public ReadOnlyCollection<T> Contents { get; private set; }
    public Bounds Bounds { get; private set; }

    public OctreeNode(Bounds bounds, int level, int maxLevel)
    {
        Level = level;
        MaxLevel = maxLevel;
        _IsLeaf = true;
        Children = new OctreeNode<T>[8];
        _Contents = new List<T>(1);
        ActualBounds = bounds;
        this.Bounds = bounds;
        this.Contents = _Contents.AsReadOnly();
    }

    public void Add(T content)
    {
        if (!this.Bounds.Contains(content.Position))
        {
            throw new UnityException("Cannot add an object outside of octree");
        }

        // just add if empty
        if ((IsLeaf() && _Contents.Count == 0) || Level == MaxLevel)
        {
            _Contents.Add(content);
        }
        else
        {
            if (content.Position.Equals(this.Bounds.center) || (_Contents.Count > 0 && _Contents.TrueForAll(i => content.Position.Equals(i.Position))))
            {
                _Contents.Add(content);
            }
            else
            {
                if (IsLeaf())
                {
                    // build children
                    _IsLeaf = false;

                    var ex = this.Bounds.extents * 0.5f;

                    Children[0] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(-ex.x, -ex.y, -ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[1] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(-ex.x, -ex.y, ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[2] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(ex.x, -ex.y, -ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[3] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(ex.x, -ex.y, ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[4] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(-ex.x, ex.y, -ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[5] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(-ex.x, ex.y, ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[6] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(ex.x, ex.y, -ex.z), this.Bounds.extents), Level + 1, MaxLevel);
                    Children[7] = new OctreeNode<T>(new Bounds(this.Bounds.center + new Vector3(ex.x, ex.y, ex.z), this.Bounds.extents), Level + 1, MaxLevel);

                    // move any existing items
                    for (var i = 0; i < Contents.Count; i++)
                    {
                        if (!content.Position.Equals(this.Bounds.center))
                        {
                            for (var j = 0; j < 8; j++)
                            {
                                T c = Contents[i];
                                if (Children[j].Contains(c.Position))
                                    Children[j].Add(c);
                            }
                            _Contents.Clear();
                        }
                    }
                }

                // add new item into child
                for (var i = 0; i < 8; i++)
                {
                    if (Children[i].Contains(content.Position))
                        Children[i].Add(content);
                }
            }
        }
    }

    public void Remove(T content)
    {
        var match = _Contents.FirstOrDefault(c => c == content);
        if (match != null)
            _Contents.Remove(match);

        if (!IsLeaf())
        {
            var emptyChildren = 0;
            var hasFloater = true;
            List<T> floaters = null;

            for (var i = 0; i < 8; i++)
            {
                Children[i].Remove(content);

                if (Children[i].IsEmpty())
                {
                    emptyChildren++;
                }
                else if (Children[i].IsLeaf())
                {
                    if (floaters == null)
                    {
                        floaters = Children[i].Contents.ToList();
                    }
                    else if (!floaters.Equals(Children[i].Contents.ToList()))
                    {
                        hasFloater = false;
                    }
                }
                else
                {
                    hasFloater = false;
                }
            }

            if (emptyChildren == 8)
            {
                _IsLeaf = true;
                Children = new OctreeNode<T>[8];
            }
            else if (hasFloater && _Contents.Count == 0)
            {
                foreach (var p in floaters)
                    _Contents.Add(p);

                _IsLeaf = true;
                Children = new OctreeNode<T>[8];
            }
        }
    }

    public T GetContent(T content)
    {
        var match = _Contents.FirstOrDefault(c => c == content);

        if (match != null)
            return match;
        else if (!IsLeaf())
        {
            for (var i = 0; i < 8; i++)
            {
                match = Children[i].GetContent(content);

                if (match != null)
                    return match;
            }
        }

        return null;
    }

    public bool IsEmpty()
    {
        return _IsLeaf && _Contents.Count == 0;
    }

    public bool IsLeaf()
    {
        return _IsLeaf;
    }

    public bool Contains(Vector3 point)
    {
        return ActualBounds.Contains(point);
    }

    public List<T> GetList(List<T> result)
    {
        result.AddRange(Contents);
        if (!IsLeaf())
        {
            foreach (OctreeNode<T> node in Children)
            {
                node.GetList(result);
            }
        }
        return result;
    }
}

