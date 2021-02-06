using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {
    public SquareConfiguration configuration { get; set; }
    private ControlNode topLeft;
    private ControlNode topRight;
    private ControlNode bottomRight;
    private ControlNode bottomLeft;
    private Node centerTop;
    private Node centerRight;
    private Node centerBottom;
    private Node centerLeft;
    private Node center;

    public Square(ControlNode topLeft,
        ControlNode topRight,
        ControlNode bottomRight,
        ControlNode bottomLeft,
        Node centerTop,
        Node centerRight,
        Node centerBottom,
        Node centerLeft,
        Node center, SquareConfiguration squareConfiguration) {
        configuration = squareConfiguration;
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomRight = bottomRight;
        this.bottomLeft = bottomLeft;
        this.centerTop = centerTop;
        this.centerRight = centerRight;
        this.centerBottom = centerBottom;
        this.centerLeft = centerLeft;
        this.center = center;
    }

    public ControlNode TopLeft => topLeft;

    public ControlNode TopRight => topRight;

    public ControlNode BottomRight => bottomRight;

    public ControlNode BottomLeft => bottomLeft;

    public Node CenterTop => centerTop;

    public Node CenterRight => centerRight;

    public Node CenterBottom => centerBottom;

    public Node CenterLeft => centerLeft;

    public Node Center => center;

    public List<ControlNode> getActiveControlNodes() {
        List<ControlNode> activeNodes = new List<ControlNode>();
        if(topLeft.material>0) activeNodes.Add(topLeft);
        if(topRight.material>0) activeNodes.Add(topRight);
        if(bottomRight.material>0) activeNodes.Add(bottomRight);
        if(BottomLeft.material>0) activeNodes.Add(bottomLeft);
        return activeNodes;
    }

    public override string ToString() {
        return "Config: " + configuration +
               " TopLeft: " + topLeft +
               " TopRight: " + topRight +
               " BottomRight: " + bottomRight +
               " BottomLeft: " + bottomLeft;
    }
}
