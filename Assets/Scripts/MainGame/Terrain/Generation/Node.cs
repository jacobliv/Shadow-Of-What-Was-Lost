using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    private Vector2 position;

    public Node(Vector2 position) {
        this.position = position;
    }

    public Vector2 Position => position;
}
