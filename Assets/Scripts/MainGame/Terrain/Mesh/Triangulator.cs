using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TriangleType;

public class Triangulator : MonoBehaviour {
    private ExternalEdgeDetector edgeDetector;
    private void Start() {
        
    }

    public void setupEdgeDetector() {
        if(edgeDetector != null) return;
        edgeDetector = GetComponent<ExternalEdgeDetector>();
    }

    public void generate(GenerationData data) {
        setupEdgeDetector();
        List<int> indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        Dictionary<ControlNode,List<int>> controlNodeToIndicies = new Dictionary<ControlNode, List<int>>();
        Dictionary<ControlNode,SquareConfig> squareConfigs = new Dictionary<ControlNode, SquareConfig>();
        Dictionary<ExternalEdgeType,List<ExternalEdge>> exEdges = new Dictionary<ExternalEdgeType, List<ExternalEdge>>();
        execute(data, indices, exEdges, squareConfigs);
        
        addToGenerationData(data, indices, exEdges, squareConfigs, controlNodeToIndicies);
    }

    private static void addToGenerationData(GenerationData data,
                                            List<int> indices,
                                            Dictionary<ExternalEdgeType, List<ExternalEdge>> edges,
                                            Dictionary<ControlNode, SquareConfig> squareConfigs,
                                            Dictionary<ControlNode, List<int>> controlNodeToIndicies) {
        data.indices = indices;
        data.squareConfigs = squareConfigs;
        data.controlNodeToIndicies = controlNodeToIndicies;
        data.edges = edges;
    }

    private void execute(GenerationData data, 
                         List<int> indices,
                         Dictionary<ExternalEdgeType, List<ExternalEdge>> externalEdges,
                         Dictionary<ControlNode, SquareConfig> squareConfigs) {
        foreach (KeyValuePair<Vector2, Square> values in data.squares) {
            Square square = values.Value;
            SquareConfig squareConfig = triangulateSquare(square, indices, data.posToIndex);
            if (squareConfig == null) continue;
            Dictionary<ExternalEdgeType,List<ExternalEdge>> edges = edgeDetector.detect(squareConfig);
            // addToExternal(externalEdges, edges);
            squareConfigs.Add(values.Value.BottomLeft, squareConfig);
        }
    }

    public void addToExternal(Dictionary<ExternalEdgeType, List<ExternalEdge>> externalEdges,
                              Dictionary<ExternalEdgeType, List<ExternalEdge>> newEdges) {
        foreach (KeyValuePair<ExternalEdgeType, List<ExternalEdge>> edge in newEdges) {
            if (externalEdges.ContainsKey(edge.Key)) {
                List<ExternalEdge> exEdges = externalEdges[edge.Key];
                exEdges.AddRange(edge.Value);
                externalEdges[edge.Key] = exEdges;
                continue;
            }

            externalEdges.Add(edge.Key, edge.Value);
        }
    }

    private SquareConfig triangulateSquare(Square square,
                                           List<int> indices,
                                           Dictionary<Vector2, int> posToIndex) {
        if (square.configuration == SquareConfiguration.EMPTY) return null;
        SquareConfig squareConfig = new SquareConfig();
        squareConfig.square = square;
        squareConfig.activeNodes = square.getActiveControlNodes();
        squareConfig.triangleTypes = getTriangleTypes(squareConfig);
        squareConfig.triangles = createTriangles(squareConfig);
        addTriangles(indices, posToIndex, squareConfig);
        // getExternalPoints()
        return squareConfig;

    }

    public List<TriangleType> getTriangleTypes(SquareConfig squareConfig) {
        switch (squareConfig.square.configuration) {
            case SquareConfiguration.TL:
                return new List<TriangleType> {TOP_TL};
            case SquareConfiguration.TR:
                return new List<TriangleType> {TOP_TR};
            case SquareConfiguration.BR:
                return new List<TriangleType> {BOTTOM_BR};
            case SquareConfiguration.BL:
                return new List<TriangleType> {BOTTOM_BL};
            case SquareConfiguration.TL_TR:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, TOP_TR, BOTTOM_TR};
            case SquareConfiguration.TL_BR:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, BOTTOM_TR, TOP_BR, BOTTOM_BR, TOP_BL};
            case SquareConfiguration.TL_BL:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, TOP_BL, BOTTOM_BL};
            case SquareConfiguration.TR_BR:
                return new List<TriangleType> {TOP_TR, BOTTOM_TR, TOP_BR, BOTTOM_BR};
            case SquareConfiguration.TR_BL:
                return new List<TriangleType> {BOTTOM_TL, TOP_TR, BOTTOM_TR, TOP_BR, TOP_BL, BOTTOM_BL};
            case SquareConfiguration.BR_BL:
                return new List<TriangleType> {TOP_BR, BOTTOM_BR, TOP_BL, BOTTOM_BL};
            case SquareConfiguration.TL_TR_BR:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, TOP_TR, BOTTOM_TR, TOP_BR, BOTTOM_BR, TOP_BL};
            case SquareConfiguration.TL_TR_BL:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, TOP_TR, BOTTOM_TR, TOP_BR, TOP_BL, BOTTOM_BL};
            case SquareConfiguration.TL_BR_BL:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, BOTTOM_TR, TOP_BR, BOTTOM_BR, TOP_BL, BOTTOM_BL};
            case SquareConfiguration.TR_BR_BL:
                return new List<TriangleType> {BOTTOM_TL, TOP_TR, BOTTOM_TR, TOP_BR, BOTTOM_BR, TOP_BL, BOTTOM_BL};
            case SquareConfiguration.FULL:
                return new List<TriangleType> {TOP_TL, BOTTOM_TL, TOP_TR, BOTTOM_TR, TOP_BR, BOTTOM_BR, TOP_BL, BOTTOM_BL};
            default:
                return new List<TriangleType>();
        }
    }

    public List<MeshTriangle> createTriangles(SquareConfig squareConfig) {
        List<MeshTriangle> triangles = new List<MeshTriangle>();
        foreach (TriangleType triangleType in squareConfig.triangleTypes) {
            triangles.Add(createTriangle(squareConfig, triangleType));
        }
        return triangles;
    }

    private MeshTriangle createTriangle(SquareConfig squareConfig, TriangleType triangleType) {
        switch (triangleType) {
            case TOP_TL:
                return new MeshTriangle(squareConfig.square.TopLeft.Position,
                    squareConfig.square.CenterTop.Position,
                    squareConfig.square.CenterLeft.Position,
                    MeshTriangleUVCoordMap.getUVCoord(TOP_TL));
            case BOTTOM_TL:
                return new MeshTriangle(squareConfig.square.CenterTop.Position,
                    squareConfig.square.Center.Position,
                    squareConfig.square.CenterLeft.Position,
                    MeshTriangleUVCoordMap.getUVCoord(BOTTOM_TL));
            case TOP_TR:
                return new MeshTriangle(squareConfig.square.CenterTop.Position,
                    squareConfig.square.TopRight.Position,
                    squareConfig.square.CenterRight.Position,
                    MeshTriangleUVCoordMap.getUVCoord(TOP_TR));
            case BOTTOM_TR:
                return new MeshTriangle(squareConfig.square.CenterTop.Position,
                    squareConfig.square.CenterRight.Position,
                    squareConfig.square.Center.Position,
                    MeshTriangleUVCoordMap.getUVCoord(BOTTOM_TR));
            case TOP_BR:
                return new MeshTriangle(squareConfig.square.CenterRight.Position,
                    squareConfig.square.CenterBottom.Position,
                    squareConfig.square.Center.Position,
                    MeshTriangleUVCoordMap.getUVCoord(TOP_BR));
            case BOTTOM_BR:
                return new MeshTriangle(squareConfig.square.CenterRight.Position,
                    squareConfig.square.BottomRight.Position,
                    squareConfig.square.CenterBottom.Position,
                    MeshTriangleUVCoordMap.getUVCoord(BOTTOM_BR));
            case TOP_BL:
                return new MeshTriangle(squareConfig.square.CenterBottom.Position,
                    squareConfig.square.CenterLeft.Position,
                    squareConfig.square.Center.Position,
                    MeshTriangleUVCoordMap.getUVCoord(TOP_BL));
            case BOTTOM_BL:
                return new MeshTriangle(squareConfig.square.CenterBottom.Position,
                    squareConfig.square.BottomLeft.Position,
                    squareConfig.square.CenterLeft.Position,
                    MeshTriangleUVCoordMap.getUVCoord(BOTTOM_BL));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    

    public void addTriangles(List<int> indices,
                             Dictionary<Vector2, int> posToIndex,
                             SquareConfig squareConfig) {
        foreach (MeshTriangle triangle in squareConfig.triangles) {
            int i1 = posToIndex[triangle[0]];
            int i2 = posToIndex[triangle[1]];
            int i3 = posToIndex[triangle[2]];
            indices.Add(i1);
            indices.Add(i2);
            indices.Add(i3);
            triangle.i1 = i1;
            triangle.i2 = i2;
            triangle.i3 = i3;
            triangle.indicesI1 = indices.Count - 3;
            triangle.indicesI2 = indices.Count - 2;
            triangle.indicesI3 = indices.Count - 1;
        }
    }
}
