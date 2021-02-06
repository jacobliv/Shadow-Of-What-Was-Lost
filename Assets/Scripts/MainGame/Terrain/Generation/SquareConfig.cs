using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareConfig {
    public List<ControlNode> activeNodes { get; set; }
    public List<int> externalEdges { get; set; }
    public List<TriangleType> triangleTypes { get; set; }
    public List<MeshTriangle> triangles { get; set; }
    public Square square { get; set; }
}
