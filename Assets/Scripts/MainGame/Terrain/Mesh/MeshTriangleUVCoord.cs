using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangleUVCoord {

    // coords in this class will be a percentage of a single texture size. Aka, 
    // if we have a 2x2 grid of textures (it will be much larger in practice
    // we will have to have an offset to get the position of the actual texture
    public Vector2 c1 { get; set; }
    public Vector2 c2 { get; set; }
    public Vector2 c3 { get; set; }

    public MeshTriangleUVCoord(Vector2 c1, Vector2 c2, Vector2 c3) {
        this.c1 = c1;
        this.c2 = c2;
        this.c3 = c3;
    }
}
