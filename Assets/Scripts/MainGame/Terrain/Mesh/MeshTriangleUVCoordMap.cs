using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TriangleType;

public class MeshTriangleUVCoordMap {
    private static Dictionary<TriangleType, MeshTriangleUVCoord> meshTriangleUVCoords;

    static MeshTriangleUVCoordMap() {
        meshTriangleUVCoords = new Dictionary<TriangleType, MeshTriangleUVCoord>();
        meshTriangleUVCoords.Add(TOP_TL,   new MeshTriangleUVCoord(new Vector2(.5f,.5f),
                                                                   new Vector2(1f,.5f),
                                                                   new Vector2(.5f,0f)));
                                                                   
        meshTriangleUVCoords.Add(BOTTOM_TL,new MeshTriangleUVCoord(new Vector2(1f,.5f),
                                                                   new Vector2(1f,0f),
                                                                   new Vector2(.5f,0f)));
                                                                   
        meshTriangleUVCoords.Add(TOP_TR,   new MeshTriangleUVCoord(new Vector2(0f,.5f),
                                                                   new Vector2(.5f,.5f),
                                                                   new Vector2(.5f,0f)));
                                                                   
        meshTriangleUVCoords.Add(BOTTOM_TR,new MeshTriangleUVCoord(new Vector2(0f,.5f),
                                                                   new Vector2(.5f,0f),
                                                                   new Vector2(0f,0f)));
                                                                   
        meshTriangleUVCoords.Add(TOP_BR,   new MeshTriangleUVCoord(new Vector2(.5f,1f),
                                                                   new Vector2(0f,.5f),
                                                                   new Vector2(0f,1f)));
                                                                   
        meshTriangleUVCoords.Add(BOTTOM_BR,new MeshTriangleUVCoord(new Vector2(.5f,1f),
                                                                   new Vector2(.5f,.5f),
                                                                   new Vector2(0f,.5f)));
                                                                   
        meshTriangleUVCoords.Add(TOP_BL,   new MeshTriangleUVCoord(new Vector2(1f,.5f),
                                                                   new Vector2(5f,1f),
                                                                   new Vector2(1f,1f)));
                                                                   
        meshTriangleUVCoords.Add(BOTTOM_BL,new MeshTriangleUVCoord(new Vector2(1f,.5f),
                                                                   new Vector2(.5f,.5f),
                                                                   new Vector2(.5f,1f)));
    }

    public static MeshTriangleUVCoord getUVCoord(TriangleType triangleType) {
        return meshTriangleUVCoords[triangleType];
    }
}
