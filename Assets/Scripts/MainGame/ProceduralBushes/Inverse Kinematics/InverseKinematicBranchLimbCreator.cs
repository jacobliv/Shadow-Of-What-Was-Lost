using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class InverseKinematicBranchLimbCreator : MonoBehaviour {

    public List<InverseKinematicUnit> create(Bush bush) {
        List<InverseKinematicUnit> inverseKinematicUnits = new List<InverseKinematicUnit>();
        foreach (Branch branch in bush.rootBranches) {
            InverseKinematicUnit unit = new InverseKinematicUnit();
            inverseKinematicUnits.Add(unit);
            float length = Vector2.Distance(branch.inboard, branch.outboard);
            Func<Vector2> getInboard = () => {
                return branch.inboardLNode.position;
            };
            Func<Vector2> getOutboard = () => {
                return branch.outboardLNode.position;
            };
            Limb limb = new Limb(branch.inboard, branch.outboard, branch.angle, length, getInboard, getOutboard);
            unit.root = limb;
            List<InverseKinematicUnit> kinematicUnits = recurseBranches(limb,branch,unit);
            inverseKinematicUnits.AddRange(kinematicUnits);
        }

        return inverseKinematicUnits;
    }
    
    public List<InverseKinematicUnit> recurseBranches(Limb parent, Branch parentBranch, InverseKinematicUnit inverseKinematicUnit) {
        List<InverseKinematicUnit> inverseKinematicUnits = new List<InverseKinematicUnit>();

        if (parentBranch.children.Count == 0) {
            inverseKinematicUnit.tip = parent;
            // inverseKinematicUnits.Add(inverseKinematicUnit);
        }
        int i = 0;
        foreach (Branch branchChild in parentBranch.children) {
            if(i == 0) {
                float length = Vector2.Distance(branchChild.inboard, branchChild.outboard);
                Func<Vector2> getInboard = () => { return branchChild.inboardLNode.position;};
                Func<Vector2> getOutboard = () => { return branchChild.outboardLNode.position;};
                Limb childLimb = new Limb(branchChild.inboard, branchChild.outboard, branchChild.angle, length, getInboard, getOutboard);
                parent.child = childLimb;
                List<InverseKinematicUnit> kinematicUnits = recurseBranches(childLimb, branchChild, inverseKinematicUnit);
                inverseKinematicUnits.AddRange(kinematicUnits);

            }
            else {
                InverseKinematicUnit branchingUnit = new InverseKinematicUnit();
                inverseKinematicUnits.Add(branchingUnit);
                float length = Vector2.Distance(branchChild.inboard, branchChild.outboard);
                Func<Vector2> getInboard = () => { return branchChild.inboardLNode.position;};
                Func<Vector2> getOutboard = () => { return branchChild.outboardLNode.position;};
                Limb newLimb = new Limb(branchChild.inboard, branchChild.outboard, branchChild.angle, length, getInboard, getOutboard);
                branchingUnit.root = newLimb;
                List<InverseKinematicUnit> kinematicUnits = recurseBranches(newLimb, branchChild, branchingUnit);
                inverseKinematicUnits.AddRange(kinematicUnits);
            }
            i++;
        }

        return inverseKinematicUnits;
    }
}
