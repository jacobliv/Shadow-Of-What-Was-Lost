using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb {
    private readonly Func<Vector2> _getInboardOrigional;
    private readonly Func<Vector2> _getOutboardOriginal;
    public float maxAngle;
    public float minAngle;
    public float angle { get; set; }
    public Vector2 outboard { get; set; }
    public Vector2 inboard { get; set; }

    public Limb child { get; set; }
    
    public Limb parent { get; set; }
    public float length { get; set; }

    
    
    public Vector2 outboardOriginal() {
        return _getOutboardOriginal.Invoke();
    }
    
    public Vector2 inboardOriginal() {
        return _getInboardOrigional.Invoke();
    }


    public Limb(Vector2 inboard,
                Vector2 outboard,
                float angle,
                float length,
                Func<Vector2> getInboardOrigional,
                Func<Vector2> getOutboardOriginal) {
        _getInboardOrigional = getInboardOrigional;
        _getOutboardOriginal = getOutboardOriginal;
        this.angle = angle;
        this.outboard = outboard;
        this.inboard = inboard;
        this.child = child;
        this.length = length;
    }
}
