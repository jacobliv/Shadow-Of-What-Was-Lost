using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrammarNode {
    public char value { get; }
    public List<GrammarNode> children { get; }
    public int generation { get; }

    public GrammarNode(char value, int generation) {
        this.value = value;
        this.generation = generation;
        children = new List<GrammarNode>();
    }

    public void addChild(char c,int generation) {
        children.Add(new GrammarNode(c,generation));
    }

    public void addChild(GrammarNode n) {
        children.Add(n);
    }
    
    public void addChildren(string childs, int generation) {
        for (var i = 0; i < childs.Length; i++) {
            addChild(childs[i],generation);
        }
    }
    
}
