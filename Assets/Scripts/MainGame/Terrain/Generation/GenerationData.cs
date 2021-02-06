using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationData {
    public GenerationData() {}
    private Dictionary<Vector2, int> cellularAutomata;
    private Bounds bounds;
    public List<Color> colors;
    public Dictionary<ControlNode, SquareConfig> squareConfigs { get; set; }
    public Dictionary<Vector2,int> AdjacentData { get; set; }
    public Bounds Bounds { get ; set ; }
    public Dictionary<Vector2, int> CellularAutomata {get; set;}
    public Dictionary<Vector2, ControlNode> ControlNodes { get; set; }
    public Dictionary<ControlNode, List<int>> controlNodeToIndicies { get; set; }
    public Dictionary<Vector2, int> posToIndex { get; set; }
    public List<Vector3> vertices { get; set; }
    public Dictionary<Vector2, Square> squares { get; set; }
    public List<int> indices { get; set; }
    public Dictionary<ExternalEdgeType, List<ExternalEdge>> edges { get; set; }
}
