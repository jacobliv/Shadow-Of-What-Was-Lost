using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataDebugRenderer : MonoBehaviour {
    public void debugDraw() {
        float size = GameManager.SQUARE_SIZE / 100f;
        // Debug.DrawLine(Vector3.zero, new Vector3(0, 5, 0), Color.red);

        // if(!GameManager.SHOW_AUTOMATA) return;
        // foreach (KeyValuePair<Vector2,int> values in dictionary) {
        //     // {
        //     //     Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        //     if (values.Value == 1)
        //     {
        //         Vector3 tl = new Vector3(values.Key.x - size,
        //             values.Key.y + size, 0);
        //         Vector3 tr = new Vector3(values.Key.x + size,
        //             values.Key.y + size, 0);
        //         Vector3 br = new Vector3(values.Key.x + size,
        //             values.Key.y - size, 0);
        //         Vector3 bl = new Vector3(values.Key.x - size,
        //             values.Key.y - size, 0);
        //         // Debug.DrawLine(tl, tr, Color.red);
        //         // Debug.DrawLine(tr, br, Color.red);
        //         // Debug.DrawLine(br, bl, Color.red);
        //         // Debug.DrawLine(bl, tl, Color.red);
        //         // Debug.Log("Topleft: (" +tl.x+","+tl.y + ") BottomLeft: (" +bl.x+","+bl.y +")");
        //
        //         Gizmos.DrawCube(new Vector3(values.Key.x,values.Key.y,0), new Vector3(size,size, 0));
        //         // UnityEditor.Handles.DrawLine(new Vector3(values.Key.x, values.Key.y, 0),
        //         //     new Vector3(values.Key.x + 5, values.Key.y, 0));
        //     }
        //     //
        // }
        //
    }
}
