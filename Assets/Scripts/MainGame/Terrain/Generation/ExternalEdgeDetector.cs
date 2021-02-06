using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExternalEdgeType;
using static TriangleType;

public class ExternalEdgeDetector : MonoBehaviour {
    private Dictionary<ExternalEdgeType, List<ExternalEdge>> exEdges;

    private void OnDrawGizmos() {
        if(exEdges == null) return;
        foreach (KeyValuePair<ExternalEdgeType,List<ExternalEdge>> edges in this.exEdges) {
            switch (edges.Key) {
                case TOP:
                    Gizmos.color = Color.black;
                    break;
                case BOTTOM:
                    Gizmos.color = Color.blue;
                    break;
                case LEFT:
                    Gizmos.color = Color.cyan;
                    break;
                case RIGHT:
                    Gizmos.color = Color.gray;
                    break;
                case TL_SLANT:
                    Gizmos.color = Color.green;
                    break;
                case TR_SLANT:
                    Gizmos.color = Color.magenta;
                    break;
                case BR_SLANT:
                    Gizmos.color = Color.red;
                    break;
                case BL_SLANT:
                    Gizmos.color = Color.white;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            foreach (ExternalEdge edge in edges.Value) {
                Gizmos.DrawLine(new Vector3(edge.point1.x, edge.point1.y, 0),
                    new Vector3(edge.point2.x, edge.point2.y, 0));
            }
            
        }

    }

    public Dictionary<ExternalEdgeType, List<ExternalEdge>> detect(SquareConfig squareConfig) {
        Dictionary<ExternalEdgeType, List<ExternalEdge>> edges = getExternalPoints(squareConfig);
        addToInternalList(edges);
        
        
        return edges;
    }

    private void addToInternalList(Dictionary<ExternalEdgeType, List<ExternalEdge>> edges) {
        if(exEdges == null) exEdges = new Dictionary<ExternalEdgeType, List<ExternalEdge>>();
        foreach (KeyValuePair<ExternalEdgeType,List<ExternalEdge>> edge in edges) {
            if (exEdges.ContainsKey(edge.Key)) {
                List<ExternalEdge> existingEdges = exEdges[edge.Key];
                existingEdges.AddRange(edge.Value);
                exEdges[edge.Key] = existingEdges;
                continue;
            }
            // BUG This is causing issues with the external lists. Figure out why the hell it is happening
            exEdges.Add(edge.Key,edge.Value);
        }
    }

    private Dictionary<ExternalEdgeType, List<ExternalEdge>> getExternalPoints(SquareConfig squareConfig) {
        Dictionary<TriangleType,MeshTriangle> meshTriangles = getExternalTriangle(squareConfig);
        if(meshTriangles.Values.Count == 0) return new Dictionary<ExternalEdgeType, List<ExternalEdge>>();
        Dictionary<ExternalEdgeType, List<ExternalEdge>> edges = new Dictionary<ExternalEdgeType, List<ExternalEdge>>();
        foreach (KeyValuePair<TriangleType,MeshTriangle> keyValuePair in meshTriangles) {
            KeyValuePair<ExternalEdgeType,List<ExternalEdge>> triangleEdge = detectTriangleEdge(squareConfig.square.configuration, keyValuePair.Key, keyValuePair.Value);
            if (edges.ContainsKey(triangleEdge.Key)) {
                List<ExternalEdge> externalEdges = edges[triangleEdge.Key];
                externalEdges.AddRange(triangleEdge.Value);
                edges[triangleEdge.Key] = externalEdges;
                continue;
            }
            edges.Add(triangleEdge.Key,triangleEdge.Value);
        }

        return edges;
    }

    private KeyValuePair<ExternalEdgeType, List<ExternalEdge>> detectTriangleEdge(SquareConfiguration configuration,
                                                                          TriangleType triangleType,
                                                                          MeshTriangle meshTriangle) {
        List<ExternalEdge> points = new List<ExternalEdge>();
        ExternalEdgeType edgeType = TOP;
        switch (configuration) {
            case SquareConfiguration.TL:
                if (triangleType != TOP_TL) break;
                points.Add(new ExternalEdge(meshTriangle[1],meshTriangle[2]));
                edgeType = TL_SLANT;
                break;
            case SquareConfiguration.TR:
                if (triangleType != TOP_TR) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));

                edgeType = TR_SLANT;
                break;
            case SquareConfiguration.BR:
                if (triangleType != BOTTOM_BR) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                edgeType = BR_SLANT;
                break;
            case SquareConfiguration.BL:
                if (triangleType != BOTTOM_BL) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                edgeType = BL_SLANT;
                break;
            case SquareConfiguration.TL_TR:
                if (triangleType != BOTTOM_TL && triangleType != BOTTOM_TR) break;
                points.Add(new ExternalEdge(meshTriangle[1],meshTriangle[2]));
                edgeType = TOP;
                break;
            case SquareConfiguration.TL_BR:
                if (triangleType != BOTTOM_TR && triangleType != TOP_BL) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[1]));
                if (triangleType == BOTTOM_TR) {
                    edgeType = BL_SLANT;
                } else {
                    edgeType = TR_SLANT;
                }
                break;
            case SquareConfiguration.TL_BL:
                if (triangleType != BOTTOM_TL && triangleType != TOP_BL) break;
                if (triangleType == BOTTOM_TL) {
                    points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[1]));
                } else {
                    points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                }

                edgeType = LEFT;
                break;
            case SquareConfiguration.TR_BR:
                if (triangleType != BOTTOM_TR && triangleType != TOP_BR) break;
                if (triangleType == BOTTOM_TR) {
                    points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                } else {
                    points.Add(new ExternalEdge(meshTriangle[1],meshTriangle[2]));
                }

                edgeType = RIGHT;
                break;
            case SquareConfiguration.TR_BL:
                if(triangleType != BOTTOM_TL && triangleType != TOP_BR) break;
                if (triangleType == BOTTOM_TL) {
                    points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                    edgeType = BR_SLANT;
                } else {
                    points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[1]));
                    edgeType = TL_SLANT;
                }
                break;
            case SquareConfiguration.BR_BL:
                if(triangleType != TOP_BR && triangleType != TOP_BL) break;
                if (triangleType == TOP_BR) {
                    points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                } else {
                    points.Add(new ExternalEdge(meshTriangle[1],meshTriangle[2]));
                }
                edgeType = BOTTOM;

                break;
            case SquareConfiguration.TL_TR_BR:
                if(triangleType != TOP_BL) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[1]));
                edgeType = TR_SLANT;
                break;
            case SquareConfiguration.TL_TR_BL:
                if(triangleType != TOP_BR) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[1]));
                edgeType = TL_SLANT;
                break;
            case SquareConfiguration.TL_BR_BL:
                if(triangleType != BOTTOM_TR) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[1]));
                edgeType = BL_SLANT;
                break;
            case SquareConfiguration.TR_BR_BL:
                if(triangleType != BOTTOM_TL) break;
                points.Add(new ExternalEdge(meshTriangle[0],meshTriangle[2]));
                edgeType = BR_SLANT;
                break;
        }

        return new KeyValuePair<ExternalEdgeType, List<ExternalEdge>>(edgeType, points);
    }

    private Dictionary<TriangleType,MeshTriangle> getExternalTriangle(SquareConfig squareConfig) {
        Dictionary<TriangleType,MeshTriangle> triangles = new Dictionary<TriangleType, MeshTriangle>();
        switch (squareConfig.square.configuration) {
            case SquareConfiguration.TL:
                triangles.Add(TOP_TL,squareConfig.triangles[0]);
                break;
            case SquareConfiguration.TR:
                triangles.Add(TOP_TR,squareConfig.triangles[0]);
                break;
            case SquareConfiguration.BR:
                triangles.Add(BOTTOM_BR,squareConfig.triangles[0]);
                break;
            case SquareConfiguration.BL:
                triangles.Add(BOTTOM_BL,squareConfig.triangles[0]);
                break;
            case SquareConfiguration.TL_TR:
                triangles.Add(BOTTOM_TL,squareConfig.triangles[1]);
                triangles.Add(BOTTOM_TR,squareConfig.triangles[3]);
                break;
            case SquareConfiguration.TL_BR:
                triangles.Add(BOTTOM_TR,squareConfig.triangles[2]);
                triangles.Add(TOP_BL,squareConfig.triangles[5]); 
                break;
            case SquareConfiguration.TL_BL:
                triangles.Add(BOTTOM_TL,squareConfig.triangles[1]);
                triangles.Add(TOP_BL,squareConfig.triangles[2]);
                break;
            case SquareConfiguration.TR_BR:
                triangles.Add(BOTTOM_TR,squareConfig.triangles[1]);
                triangles.Add(TOP_BR,squareConfig.triangles[2]);
                break;
            case SquareConfiguration.TR_BL:
                triangles.Add(BOTTOM_TL,squareConfig.triangles[0]);
                triangles.Add(TOP_BR,squareConfig.triangles[3]);
                break;
            case SquareConfiguration.BR_BL:
                triangles.Add(TOP_BR,squareConfig.triangles[0]);
                triangles.Add(TOP_BL,squareConfig.triangles[2]);
                break;
            case SquareConfiguration.TL_TR_BR:
                triangles.Add(TOP_BL,squareConfig.triangles[6]);
                break;
            case SquareConfiguration.TL_TR_BL:
                triangles.Add(TOP_BR,squareConfig.triangles[4]);
                break;
            case SquareConfiguration.TL_BR_BL:
                triangles.Add(BOTTOM_TR,squareConfig.triangles[2]);
                break;
            case SquareConfiguration.TR_BR_BL:
                triangles.Add(BOTTOM_TL,squareConfig.triangles[0]);
                break;
        }

        return triangles;
    }

    public void clearEdges() {
        this.exEdges = new Dictionary<ExternalEdgeType, List<ExternalEdge>>();
    }
}
