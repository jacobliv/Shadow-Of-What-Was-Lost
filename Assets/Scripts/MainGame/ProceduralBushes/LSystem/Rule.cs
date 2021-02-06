using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Rule {
    public string key;
    public string value;

    public override string ToString() {
        return $"{nameof(key)}: {key}, {nameof(value)}: {value}";
    }
}
