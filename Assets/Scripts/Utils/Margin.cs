using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Margin {
    [SerializeField]
    [Range(0,1000)]
    public int left;
    [SerializeField]
    [Range(0,1000)]
    public int right;
    [SerializeField]
    [Range(0,1000)]
    public int top;
    [SerializeField]
    [Range(0,1000)]
    public int bottom;

}
