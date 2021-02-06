using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class InverseKinematics : MonoBehaviour {
    private List<Vector2> points;
    // private Limb root;
    // private Limb tip;

    private void OnDrawGizmos() {
        // drawLimbs(root);

    }

    private void drawLimbs(Limb limb) {
        if(limb == null) return;
        // Gizmos.DrawSphere(new Vector3(limb.inboard.x,limb.inboard.y,0), 2);

        Gizmos.DrawLine(new Vector3(limb.inboard.x,limb.inboard.y,0), new Vector3(limb.outboard.x,limb.outboard.y,0));
        drawLimbs(limb.child);
    }

    void Start() {
        // float length = 2;
        // root = new Limb(new Vector2(0,0), new Vector2(0,length), Mathf.PI/2f, length );
        // root.maxAngle = root.angle + Mathf.PI / 2f;
        // root.minAngle = root.angle + Mathf.PI / 2f;
        //
        // Limb current = root;
        // for (int i = 1; i < 50; i++) {
        //     Limb next = new Limb(current.outboard, new Vector2(0, length * i), Mathf.PI, length);
        //     current.child = next;
        //     next.parent = current;
        //     current = next;
        // }
        //
        // tip = current;
    }

    void Update() {
        return;
        // Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // float angleInboardToTarget = Mathf.Atan2(pos.y - tip.inboard.y, pos.x - tip.inboard.x);
        //
        // tip.angle = angleInboardToTarget;
        //
        // float targetToInboard = angleInboardToTarget - Mathf.PI;
        // Vector2 nOutboard = new Vector2(pos.x,pos.y);
        // float x = Mathf.Cos(targetToInboard) * tip.length + nOutboard.x;
        // float y = Mathf.Sin(targetToInboard) * tip.length + nOutboard.y;
        // tip.inboard = new Vector2(x,y);
        // tip.outboard = nOutboard;
        // Limb current = tip.parent;
        // while (current != null) {
        //     angleInboardToTarget = Mathf.Atan2(current.child.inboard.y - current.inboard.y, current.child.inboard.x - current.inboard.x);
        //     targetToInboard = angleInboardToTarget - Mathf.PI;
        //     if (angleInboardToTarget > tip.maxAngle && current.parent == null) {
        //         angleInboardToTarget = tip.maxAngle;
        //     }
        //
        //     if (angleInboardToTarget < tip.minAngle && current.parent == null) {
        //         angleInboardToTarget = tip.minAngle;
        //     }
        //     
        //     current.angle = angleInboardToTarget;
        //     nOutboard = new Vector2(current.child.inboard.x,current.child.inboard.y);
        //     x = Mathf.Cos(targetToInboard) * tip.length + nOutboard.x;
        //     y = Mathf.Sin(targetToInboard) * tip.length + nOutboard.y;
        //     current.inboard = new Vector2(x,y);
        //     current.outboard = nOutboard;
        //     current = current.parent;
        // }
        //
        // // moveBranchBack();

    }

    public void moveUnit(InverseKinematicUnit unit) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleInboardToTarget = Mathf.Atan2(pos.y - unit.tip.inboard.y, pos.x - unit.tip.inboard.x);
   
        unit.tip.angle = angleInboardToTarget;

        float targetToInboard = angleInboardToTarget - Mathf.PI;
        Vector2 nOutboard = new Vector2(pos.x,pos.y);
        float x = Mathf.Cos(targetToInboard) * unit.tip.length + nOutboard.x;
        float y = Mathf.Sin(targetToInboard) * unit.tip.length + nOutboard.y;
        unit.tip.inboard = new Vector2(x,y);
        unit.tip.outboard = nOutboard;
        Limb current = unit.tip.parent;
        while (current != null) {
            angleInboardToTarget = Mathf.Atan2(current.child.inboard.y - current.inboard.y, current.child.inboard.x - current.inboard.x);
            targetToInboard = angleInboardToTarget - Mathf.PI;
            // if (angleInboardToTarget > tip.maxAngle && current.parent == null) {
            //     angleInboardToTarget = tip.maxAngle;
            // }
            //
            // if (angleInboardToTarget < tip.minAngle && current.parent == null) {
            //     angleInboardToTarget = tip.minAngle;
            // }
            
            current.angle = angleInboardToTarget;
            nOutboard = new Vector2(current.child.inboard.x,current.child.inboard.y);
            x = Mathf.Cos(targetToInboard) * unit.tip.length + nOutboard.x;
            y = Mathf.Sin(targetToInboard) * unit.tip.length + nOutboard.y;
            current.inboard = new Vector2(x,y);
            current.outboard = nOutboard;
            current = current.parent;
        }
        moveBranchBack(unit);

    }

    private void moveBranchBack(InverseKinematicUnit unit) {
        Limb current = unit.tip;
        while (current != null) {
            current.outboard = new Vector2(current.outboard.x - unit.root.inboardOriginal().x,current.outboard.y - unit.root.inboardOriginal().y);
            current.inboard = new Vector2(current.inboard.x - unit.root.inboardOriginal().x,current.inboard.y - unit.root.inboardOriginal().y);


            current = current.parent;
        }
    }
}
