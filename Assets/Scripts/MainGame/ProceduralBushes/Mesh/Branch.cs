using System;
using System.Collections.Generic;
using UnityEngine;

public class Branch {
    public LNode inboardLNode { get; }
    public Vector2 inboard { get; set; }
    private Tuple<Vector2, Vector2> inboardAdjacentPoints;

    public LNode outboardLNode { get; }
    public Vector2 outboard { get; set; }
    
    public List<Branch> children { get; set; }
    
    public Branch parent { get; set; }

    private Tuple<Vector2, Vector2> outboardAdjacentPoints;

    public float angle { get; set; }

    public Branch(LNode inboard,
                  LNode outboard,
                  Tuple<Vector2, Vector2> inboardAdjacentPoints,
                  Tuple<Vector2, Vector2> outboardAdjacentPoints,
                  Branch parent) {
        this.inboardLNode = inboard;
        this.outboardLNode = outboard;
        this.inboard = new Vector2(inboard.position.x,inboard.position.y);
        this.outboard = new Vector2(outboard.position.x,outboard.position.y);
        this.inboardAdjacentPoints = inboardAdjacentPoints;
        this.outboardAdjacentPoints = outboardAdjacentPoints;
        children = new List<Branch>();
        this.parent = parent;
    }
    

    public void addChild(Branch branch) {
        children.Add(branch);
    }
    public Vector2 inboundLeft() {
        return inboardAdjacentPoints.Item1;
    }
    public Vector2 inboundRight() {
        return inboardAdjacentPoints.Item2;
    }
    public Vector2 outboundLeft() {
        return outboardAdjacentPoints.Item1;
    }
    public Vector2 outboundRight() {
        return outboardAdjacentPoints.Item2;
    }

    public override bool Equals(object obj) {
        if (obj.GetType() != typeof(Branch)) return false;
        Branch other = (Branch) obj;
        
        return this.inboard == other.inboard && this.outboard == other.outboard;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
