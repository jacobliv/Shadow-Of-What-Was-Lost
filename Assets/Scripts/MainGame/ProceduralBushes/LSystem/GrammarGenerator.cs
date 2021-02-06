using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrammarGenerator : MonoBehaviour {
    /*
     * F -> F+F+F 
     * + -> F-F+F-
     * - -> F++F-F
     */
    void Start() {
        
    }

    public GrammarNode generate(string start, Dictionary<char,string> rules, int maxGenerations) {
        GrammarNode root = new GrammarNode(' ',-1);
        for (var i = 0; i < start.Length; i++) {
            GrammarNode next = new GrammarNode(start[i],0);
            root.addChild(next);
            recursiveGenerate(next, rules, 0,0, maxGenerations);
        }
        
        return root;
    }

    private void recursiveGenerate(GrammarNode currentNode, Dictionary<char, string> rules, int moveGens, int turnGens, int maxGenerations) {
        if (moveGens + (turnGens*.85f) >= maxGenerations) return;
        if (!rules.ContainsKey(currentNode.value)) return;
        string rule = rules[currentNode.value];
        currentNode.addChildren(rule,moveGens);
        foreach (GrammarNode currentNodeChild in currentNode.children) {
            int nextMoveGen = currentNodeChild.value == 'F' ? moveGens + 1 : moveGens;
            int nextTurnGens = currentNodeChild.value != 'F' ? turnGens +1 : turnGens;
            recursiveGenerate(currentNodeChild,rules,nextMoveGen,nextTurnGens,maxGenerations);
        }
    }

    void Update() {
        
    }
}
