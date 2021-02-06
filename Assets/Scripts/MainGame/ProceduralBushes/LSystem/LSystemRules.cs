using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LSystemRules {
    public List<Rule> grammarRules;

    public List<Rule> behaviorRules;

    public int maxGenerations;

    public int maxSegmentLength;

    public bool angleRandomness;

    public string angleRandomAmount;

    public bool segmentLengthRandomness;

    public float segmentLengthRandomAmount;
    public string axiom;
    public Dictionary<char,string> interpretedGrammarRules { get; set; }
    public Dictionary<char, Func<GrammarNode, LNode, int, LNode>> interpretedBehaviorRules { get; }

    public LSystemRules() {
        interpretedBehaviorRules = new Dictionary<char, Func<GrammarNode, LNode, int, LNode>>();
        interpretedGrammarRules = new Dictionary<char, string>();
    }

    public override string ToString() {
        return $"{nameof(grammarRules)}: {string.Join(",",grammarRules)}," +
               $" {nameof(behaviorRules)}: {string.Join(",",behaviorRules)}," +
               $" {nameof(maxGenerations)}: {maxGenerations}," +
               $" {nameof(maxSegmentLength)}: {maxSegmentLength}," +
               $" {nameof(angleRandomness)}: {angleRandomness}," +
               $" {nameof(angleRandomAmount)}: {angleRandomAmount}," +
               $" {nameof(segmentLengthRandomness)}: {segmentLengthRandomness}," +
               $" {nameof(segmentLengthRandomAmount)}: {segmentLengthRandomAmount}";
    }

    public void addGrammarRule(Rule grammarRule) {
        interpretedGrammarRules.Add(grammarRule.key[0], grammarRule.value);
    }

    public void addBehaviorRule(char c, Func<GrammarNode, LNode, int, LNode> behavior) {
        interpretedBehaviorRules.Add(c, behavior);
    }

}
