using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class LSystemGenerator : MonoBehaviour {
    private LNode lNode;
    public int maxGenerations;
    [SerializeField]
    public string axiom;
    [SerializeField]
    public Dictionary<char, string> grammarRules;

    [SerializeField] public string rulesFile;
    private Bush bush;
    private List<InverseKinematicUnit> inverseKinematicUnits;
    [SerializeField]
    [Range(0,186)]
    private int value;

    private InverseKinematics inverseKinematics;

    private void OnDrawGizmos() {
        // if (lNode == null ) return;
        // foreach (Branch branch in bush.rootBranches) {
        //     // Gizmos.DrawLine(new Vector3(branch.lower.position.x, branch.lower.position.y, 0),
        //     //     new Vector3(branch.higher.position.x, branch.higher.position.y, 0));
        //     Gizmos.DrawLine(new Vector3(branch.inboundLeft().x, branch.inboundLeft().y, 0),
        //         new Vector3(branch.outboundLeft().x, branch.outboundLeft().y, 0));
        //     Gizmos.DrawLine(new Vector3(branch.inboundRight().x, branch.inboundRight().y, 0),
        //         new Vector3(branch.outboundRight().x, branch.outboundRight().y, 0));
        // }
        if(inverseKinematicUnits == null) return;
        foreach (InverseKinematicUnit inverseKinematicUnit in inverseKinematicUnits) {
        // InverseKinematicUnit inverseKinematicUnit = inverseKinematicUnits[value];
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(inverseKinematicUnit.root.inboard.x, inverseKinematicUnit.root.inboard.y, 0),2);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(inverseKinematicUnit.tip.outboard.x, inverseKinematicUnit.tip.outboard.y, 0),1);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(inverseKinematicUnit.root.inboard.x, inverseKinematicUnit.root.inboard.y, 0),new Vector3(inverseKinematicUnit.tip.outboard.x, inverseKinematicUnit.tip.outboard.y, 0));
        }
    }

    private void recursiveDraw(LNode node, int generation) {
        foreach (LNode nodeChild in node.children) {
            // if(Math.Abs(nodeChild.position.x - node.position.x) > .01) continue;
            if (nodeChild.grammarNode.value == 'F') {
                Gizmos.color = Color.black;

                Gizmos.DrawLine(new Vector3(node.position.x, node.position.y, 0),
                    new Vector3(nodeChild.position.x, nodeChild.position.y, 0));
                float angle = Mathf.Atan2(nodeChild.position.y - node.position.y,
                    nodeChild.position.x - node.position.x);
                Debug.Log("Nodes: " + node.position + " " + nodeChild.position);
                Debug.Log("Angle: " + angle);
                float plus = angle - (Mathf.PI / 2f);
                float minus = angle + (Mathf.PI / 2f);
                float thicknessThick = 1 - (generation / 10f);
                float thicknessThin = 1 - ((generation + 1) / 10f);

                float xThickPlus = Mathf.Cos(plus) * thicknessThick + node.position.x;
                float yThickPlus = Mathf.Sin(plus) * thicknessThick + node.position.y;
                float xThinPlus = Mathf.Cos(plus) * thicknessThin + nodeChild.position.x;
                float yThinPlus = Mathf.Sin(plus) * thicknessThin + nodeChild.position.y;
                // Debug.Log("PLUS "+generation+": Between: ("+xThickPlus+","+yThickPlus+") and ("+xThinPlus+","+yThinPlus+")");
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(new Vector3(xThickPlus, yThickPlus, 0), new Vector3(xThinPlus, yThinPlus, 0));


                float xThickMinus = Mathf.Cos(minus) * thicknessThick + node.position.x;
                float yThickMinus = Mathf.Sin(minus) * thicknessThick + node.position.y;
                float xThinMinus = Mathf.Cos(minus) * thicknessThin + nodeChild.position.x;
                float yThinMinus = Mathf.Sin(minus) * thicknessThin + nodeChild.position.y;
                Gizmos.color = Color.magenta;
                // Debug.Log("Minus "+generation+": Between: ("+xThickMinus+","+yThickMinus+") and ("+xThinMinus+","+yThinMinus+")");

                Gizmos.DrawLine(new Vector3(xThickMinus, yThickMinus, 0), new Vector3(xThinMinus, yThinMinus, 0));
                Gizmos.color = Color.red;

                Gizmos.DrawLine(new Vector3(xThinMinus, yThinMinus, 0), new Vector3(xThinPlus, yThinPlus, 0));
            }

            recursiveDraw(nodeChild,generation+1);
        }
    }

    void Start() {
        inverseKinematics = GetComponent<InverseKinematics>();
        readRuleFile();
        LSystemRules lSystemRules = JsonUtility.FromJson<LSystemRules>(rulesFile);
        Debug.Log(lSystemRules);
        interpretRules(lSystemRules);
        GrammarGenerator grammarGenerator = GetComponent<GrammarGenerator>();
        NodeBehaviorGenerator nodeBehaviorGenerator = GetComponent<NodeBehaviorGenerator>();
        BushCreator bushCreator = GetComponent<BushCreator>();
        BushMeshCreator bushMeshCreator = GetComponent<BushMeshCreator>();
        GrammarNode root = grammarGenerator.generate(lSystemRules.axiom, lSystemRules.interpretedGrammarRules, maxGenerations);

        LNode lNode = nodeBehaviorGenerator.generate(root, lSystemRules.interpretedBehaviorRules);
        Bush bush = bushCreator.create(lNode);
        this.bush = bush;
        bushMeshCreator.create(bush);
        // Tree
        this.lNode = lNode;
        InverseKinematicBranchLimbCreator inverseKinematicBranchLimbCreator = GetComponent<InverseKinematicBranchLimbCreator>();
        List<InverseKinematicUnit> inverseKinematicUnits = inverseKinematicBranchLimbCreator.create(bush);
        this.inverseKinematicUnits = inverseKinematicUnits;
    }

    private void interpretRules(LSystemRules lSystemRules) {
        translateGrammarRules(lSystemRules);
        interpretBehaviorRules(lSystemRules);
    }

    private void interpretBehaviorRules(LSystemRules lSystemRules) {
        Func<GrammarNode, LNode,int, LNode> forward = (GrammarNode current, LNode parent, int generation) => {
            Vector2 parentPosition = parent.position;
            float angle = parent.angle;
            float x = Mathf.Sin(angle) * ((lSystemRules.maxGenerations - generation)*3 + Random.Range(0,lSystemRules.segmentLengthRandomAmount)) + parentPosition.x;
            float y = Mathf.Cos(angle) * ((lSystemRules.maxGenerations - generation)*3 + Random.Range(0,lSystemRules.segmentLengthRandomAmount)) + parentPosition.y;

            return new LNode(new Vector2(x,y),angle,current);
        };
        
        
        foreach (Rule behaviorRule in lSystemRules.behaviorRules) {
            if (behaviorRule.value.Contains("FORWARD")) {
                lSystemRules.addBehaviorRule(behaviorRule.key[0], forward);
            } else if (behaviorRule.value.Contains("D")) {
                float degrees = float.Parse(behaviorRule.value.Split('D')[0]);
                float rad = degrees * Mathf.Deg2Rad;
                Func<GrammarNode, LNode,int, LNode> turnBehavior = (GrammarNode current, LNode parent,int generation) => {
                    Vector2 parentPosition = parent.position;
                    float angle = parent.angle+rad;
                    float x = Mathf.Sin(angle) * ((lSystemRules.maxGenerations - generation)*3 + Random.Range(0,lSystemRules.segmentLengthRandomAmount)) + parentPosition.x;
                    float y = Mathf.Cos(angle) * ((lSystemRules.maxGenerations - generation)*3 + Random.Range(0,lSystemRules.segmentLengthRandomAmount)) + parentPosition.y;

                    return new LNode(new Vector2(x,y), angle/*+ Random.Range(-1.0f, 1.0f)*/,current);

                };
                lSystemRules.addBehaviorRule(behaviorRule.key[0],turnBehavior);
            } else if (behaviorRule.key == ".") {
                Func<GrammarNode, LNode,int, LNode> dotBehavior = (GrammarNode current, LNode parent,int generation) => {
                    return new LNode(parent.position,parent.angle,current);

                };
                lSystemRules.addBehaviorRule(behaviorRule.key[0],dotBehavior);

            }
        }
    }

    private void translateGrammarRules(LSystemRules lSystemRules) {
        foreach (Rule grammarRule in lSystemRules.grammarRules) {
            lSystemRules.addGrammarRule(grammarRule);
        }
    }

    private void readRuleFile() {
        using (StreamReader sr = new StreamReader("./assets/LSystem/LSystemRules.json")) {
            string currentLine;
            // currentLine will be null when the StreamReader reaches the end of file
            while((currentLine = sr.ReadLine()) != null) {
                rulesFile += currentLine;
            }
        }
    }

    void Update() {
        foreach (InverseKinematicUnit inverseKinematicUnit in inverseKinematicUnits) {
            inverseKinematics.moveUnit(inverseKinematicUnit);
        }
    }
}
