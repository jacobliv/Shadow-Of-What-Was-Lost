using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Bounds
{
    private Vector2 _min;
    private Vector2 _max;
    private Size size;
    private Vector2 center;

    public Vector2 Center {
        get => center;
    }

    public Size Size
    {
        get => size;
    }

    public Vector2 MIN
    {
        get => _min;
    }

    public Vector2 MAX
    {
        get => _max;
    }

    public Bounds(Vector2 min, Vector2 max)
    {
        _min = min;
        _max = max;
        size = new Size((int)(max.x-min.x),(int)(max.y-min.y));
        center = new Vector2(min.x+size.Width/2f,min.y+size.Height/2f);
    }
    
    public Bounds(Vector2 min, Size size)
    {
        _min = min;
        _max = new Vector2(min.x+size.Width,min.y + size.Height);
        this.size = size;
        center = new Vector2(min.x+size.Width/2f,min.y+size.Height/2f);
    }

    public bool inside(Vector2 nPos)
    {
        return nPos.x >= _min.x && nPos.y >= _min.y &&
               nPos.x <= _max.x && nPos.y <= _max.y;
    }
    
    public override string ToString() {
        return "Min: " + _min + " Max: " + _max;
    }
}
