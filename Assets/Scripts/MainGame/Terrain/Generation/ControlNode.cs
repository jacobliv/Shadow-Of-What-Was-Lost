using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNode : Node {
    private Node right;
    private Node below;
    private Node center;

    public ControlNode(Vector2 position, int material, Node below, Node center, Node right): base(position) {
        this.material = material;
        this.right = right;
        this.below = below;
        this.center = center;
    }

    public int material { get; set; }

    public Node Right => right;

    public Node Below => below;

    public Node Center => center;

    public override string ToString() {
        return $"(Material: {material} Position: {Position})";
    }
}
