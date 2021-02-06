using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviorGenerator : MonoBehaviour {
    public LNode generate(GrammarNode root, Dictionary<char, Func<GrammarNode, LNode, int, LNode>> behaviorRules) {
        LNode lNode = new LNode(new Vector2(0,0),0,root );
        foreach (GrammarNode grammarNode in root.children) {
            LNode node = recursiveGenerate(lNode,grammarNode,behaviorRules);
            if (node == null) continue;
            lNode.addChild(node);
        }
        return lNode;
    }

    private LNode recursiveGenerate(LNode lNode,
                                   GrammarNode grammarNode,
                                   Dictionary<char, Func<GrammarNode, LNode, int, LNode>> behaviorRules) {
        if (!behaviorRules.ContainsKey(grammarNode.value)) return null;
        Func<GrammarNode,LNode,int,LNode> rule = behaviorRules[grammarNode.value];
        LNode node = rule.Invoke(grammarNode,lNode,grammarNode.generation);
        foreach (GrammarNode child in grammarNode.children) {
            LNode childLNode = recursiveGenerate(node,child,behaviorRules);
            if(childLNode == null) continue;
            node.addChild(childLNode);
        }

        return node;
    }
}
