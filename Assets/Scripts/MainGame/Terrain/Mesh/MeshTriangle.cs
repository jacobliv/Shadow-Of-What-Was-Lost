using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangle : Triangle {
    public MeshTriangleUVCoord meshTriangleUVCoord { get; }
    public int i1 { get; set; }
    public int i2 { get; set; }
    public int i3 { get; set; }
    public int indicesI1 { get; set; }
    public int indicesI2 { get; set; }
    public int indicesI3 { get; set; }

    public MeshTriangle(Vector2 point1, Vector2 point2, Vector2 point3, MeshTriangleUVCoord meshTriangleUVCoord) : base(point1, point2, point3) {
        meshTriangleUVCoord = meshTriangleUVCoord;
    }
}
