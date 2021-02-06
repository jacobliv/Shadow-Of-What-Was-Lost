using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush {
    public List<Branch> rootBranches { get; }
    public List<Vector3> vertices { get; set; }
    public List<int> indices { get; set; }
    public Bush(List<Branch> rootBranches) {
        this.rootBranches = rootBranches;
    }
}
