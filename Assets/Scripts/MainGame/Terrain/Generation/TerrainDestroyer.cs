using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SquareConfiguration;

public class TerrainDestroyer : MonoBehaviour {
    private GenerationData data;
    private Mesh mesh;
    private ChunkManager chunkManager;
    private Triangulator triangulator;
    private ExternalEdgeDetector edgeDetector;


    void Start() {
        data = GetComponent<ChunkManager>().Data;
        mesh = GetComponent<MeshFilter>().mesh;
        chunkManager = GetComponent<ChunkManager>();
        triangulator = GetComponent<Triangulator>();
        edgeDetector = GetComponent<ExternalEdgeDetector>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            destroyPosition(new Vector2(pos.x,pos.y));
        }
    }

    private void destroyPosition(Vector2 pos) {
        float xP = Mathf.Round((pos.x) / GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
        float yP = Mathf.Round((pos.y) / GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
        Vector2 discretePos = new Vector2(xP, yP);
        // Debug.Log("Destroying at: " + discretePos);
        
        if(!data.ControlNodes.ContainsKey(discretePos)) return;
        ControlNode node = data.ControlNodes[discretePos];
        if(node.material <= 0) return;
        Debug.Log("Triangles: "+data.indices.Count/3);

        List<ControlNode> controlNodes = getLocalNodes(node);
        // List<ControlNode> controlNodes = new List<ControlNode>();
        // controlNodes.Add(node);
        // disable node in squares
        List<SquareConfig> squareConfigs = getSquareConfigs(controlNodes);

        removeTriangles(squareConfigs);
        Debug.Log("Triangles After Remove: "+data.indices.Count/3);

        removeControlNode(node);
        reassignSquareType(squareConfigs);
        //Recreate indices
        recreateIndices();
        
        chunkManager.assignData(data);
        Debug.Log("Triangles After: "+data.indices.Count/3);

        
        
        // get nodes that are close by to delete tris
        // delete tris (indices)
        // delete node
        // don't remove vertex. This will probably 
        
        // remake mesh
    }

    private void reassignSquareType(List<SquareConfig> squareConfigs) {
        foreach (SquareConfig squareConfig in squareConfigs) {
            SquareConfiguration squareConfiguration = getConfiguration(squareConfig.square.TopLeft,
                squareConfig.square.TopRight,
                squareConfig.square.BottomRight,
                squareConfig.square.BottomLeft);
            squareConfig.square.configuration = squareConfiguration;
        }
    }

    private SquareConfiguration getConfiguration(ControlNode topLeft,
                                                ControlNode topRight,
                                                ControlNode bottomRight,
                                                ControlNode bottomLeft) {
        bool tl = topLeft.material > 0;
        bool tr = topRight.material > 0;
        bool br = bottomRight.material > 0;
        bool bl = bottomLeft.material > 0;

        if (tl && tr && br && bl) {
            return FULL;
        } else if (tl && tr && br) {
            return TL_TR_BR;
        }else if (tl && tr && bl) {
            return TL_TR_BL;
        }else if (tl && br && bl) {
            return TL_BR_BL;
        }else if (tr && br && bl) {
            return TR_BR_BL;
        }else if (tl && tr) {
            return TL_TR;
        }else if (tl && br) {
            return TL_BR;
        }else if (tl && bl) {
            return TL_BL;
        }else if (tr && br) {
            return TR_BR;
        }else if (tr && bl) {
            return TR_BL;
        }else if (br && bl) {
            return BR_BL;
        }else if (tl) {
            return TL;
        }else if (tr) {
            return TR;
        }else if (br) {
            return BR;
        }else if (bl) {
            return BL;
        }

        return EMPTY;
    }
    private void recreateIndices() {
        List<int> indices = new List<int>();
        edgeDetector.clearEdges();
        foreach (SquareConfig squareConfig in data.squareConfigs.Values) {
            squareConfig.triangleTypes = triangulator.getTriangleTypes(squareConfig);
            squareConfig.triangles = triangulator.createTriangles(squareConfig);
            Dictionary<ExternalEdgeType,List<ExternalEdge>> edges = edgeDetector.detect(squareConfig);
            // triangulator.addToExternal(data.edges,edges);
            triangulator.addTriangles(indices,data.posToIndex,squareConfig);
        }
        data.indices = indices;
    }
    

    private List<SquareConfig> getSquareConfigs(List<ControlNode> controlNodes) {
        List<SquareConfig> squareConfigs = new List<SquareConfig>();
        foreach (ControlNode controlNode in controlNodes) {
            if(!data.squareConfigs.ContainsKey(controlNode)) continue;
            squareConfigs.Add(data.squareConfigs[controlNode]);
        }

        return squareConfigs;
    }

    private List<ControlNode> getLocalNodes(ControlNode controlNode) {
        List<Vector2> controlNodePositions = getControlNodePositions(controlNode.Position);
        List<ControlNode> controlNodes = new List<ControlNode>();
        foreach (Vector2 position in controlNodePositions) {
            if(!data.ControlNodes.ContainsKey(position)) continue;
            controlNodes.Add(data.ControlNodes[position]);
        }

        return controlNodes;
    }
    // This will disable this node in all locations due to C#'s memory model
    private void removeControlNode(ControlNode controlNode) {
        controlNode.material = 0;
    }

    private void removeTriangles(List<SquareConfig> squareConfigs) {
        foreach (SquareConfig squareConfig in squareConfigs) {
            for (var j = squareConfig.triangles.Count - 1; j >= 0; j--) {
                data.indices.RemoveAt(squareConfig.triangles[j].indicesI3);
                data.indices.RemoveAt(squareConfig.triangles[j].indicesI2);
                data.indices.RemoveAt(squareConfig.triangles[j].indicesI1);

            }
            squareConfig.triangles = new List<MeshTriangle>();
            squareConfig.triangleTypes = new List<TriangleType>();
        }

    }
    
    private List<Vector2> getControlNodePositions(Vector2 discretePos) {
        List<Vector2> squarePos = new List<Vector2>();
        for (int x = 0; x >= -1; x--) {
            for (int y = 0; y >= -1; y--) {
                Vector2 pos = new Vector2( discretePos.x + (x*GameManager.SQUARE_SIZE),
                                           discretePos.y + (y*GameManager.SQUARE_SIZE));
                squarePos.Add(pos);
            }
        }

        return squarePos;
    }

}
