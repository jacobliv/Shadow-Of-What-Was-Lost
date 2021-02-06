using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle {
    private Vector2[] points;
    public Triangle(Vector2 point1, Vector2 point2, Vector2 point3) {
        points = new Vector2[3];
        this.points[0] = point1;
        this.points[1] = point2;
        this.points[2] = point3;
    }
    
    public Vector2 this[int key] {
        get => GetValue(key);
    }

    private Vector2 GetValue(int key) {
        return points[key];
    }
}
