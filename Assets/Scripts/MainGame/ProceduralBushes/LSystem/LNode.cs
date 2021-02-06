using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNode {
    public Vector2 position { get; }
    public GrammarNode grammarNode { get; }
    public float angle { get; }
    public List<LNode> children { get; }
    
    public LNode(Vector2 position, float angle, GrammarNode grammarNode) {
        this.position = position;
        this.grammarNode = grammarNode;
        this.angle = angle;
        this.children = new List<LNode>();
    }

    public void addChild(LNode c) {
        children.Add(c);
    }
    
}
