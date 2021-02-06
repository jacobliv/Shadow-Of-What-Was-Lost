using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Terrain.Generation;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private AutomataDebugRenderer _automataDebugRenderer;
    private GenerationData data;
    private TerrainMatter terrainMatter;

    private void Awake() {
        terrainMatter = GameManager.TERRAIN_MATTER;
    }

    void Start() {
        _automataDebugRenderer = GetComponent<AutomataDebugRenderer>();
        
    }

    public void assignData(GenerationData data) {
        this.data = data;
        createMesh();
    }

    private void createMesh() {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = data.vertices.ToArray();
        // mesh.uv = newUV;
        mesh.triangles = data.indices.ToArray();

        mesh.colors = data.colors.ToArray();

    }

    public GenerationData Data {
        get => data;
    }

    void Update() {
        
    }

    private void OnDrawGizmos() {
        // _automataDebugRenderer.debugDraw(generationData);
    }
}
