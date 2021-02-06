using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushCreator : MonoBehaviour {

    public Bush create(LNode node) {
        List<Branch> rootBranches = recurseNodes(node,null);
        return new Bush(rootBranches);
    }

    private List<Branch> recurseNodes(LNode parentNode, Branch parent) {
        if (parentNode.children.Count == 0) return null;
        List<Branch> currentBranches = new List<Branch>();
        foreach (LNode nodeChild in parentNode.children) {
            
            float angle = Mathf.Atan2(nodeChild.position.y - parentNode.position.y,
                nodeChild.position.x - parentNode.position.x);

            float plus = angle - (Mathf.PI / 2f);
            float minus = angle + (Mathf.PI / 2f);
            float inboundThickness = .25f; //- (node.grammarNode.generation / (lSystemRules.maxGenerations*1f));
            float outboundThickness  = .25f; //- ((node.grammarNode.generation + 1) / (lSystemRules.maxGenerations*1f));
            Vector2 inboundLeft = getAdjacent(plus, inboundThickness, parentNode.position);
            Vector2 inboundRight = getAdjacent(minus, inboundThickness, parentNode.position);
            
            Vector2 outboundLeft = getAdjacent(plus, outboundThickness, nodeChild.position);
            Vector2 outboundRight = getAdjacent(minus, outboundThickness, nodeChild.position);
            Branch branch = new Branch(parentNode,nodeChild,
                new Tuple<Vector2, Vector2>(inboundLeft, inboundRight),
                new Tuple<Vector2, Vector2>(outboundLeft,outboundRight), parent);
            currentBranches.Add(branch);

            List<Branch> childBranches = recurseNodes(nodeChild, branch);
            if (childBranches == null)  continue;
            foreach (Branch childBranch in childBranches) {
                childBranch.parent = branch;
                branch.addChild(childBranch);
            }
        }

        return currentBranches;
    }

    private Vector2 getAdjacent(float plus, float inboundThickness, Vector2 nodePosition) {
        float x = Mathf.Cos(plus) * inboundThickness + nodePosition.x;
        float y = Mathf.Sin(plus) * inboundThickness + nodePosition.y;
        return new Vector2(x,y);
    }
}
