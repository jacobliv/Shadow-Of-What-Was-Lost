using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Terrain.Generation;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static int square_size = 10;
    private static int chunk_size = 50;
    private static int smooth_threshold = 4;
    private static double TOLERANCE = 1;
    public static TerrainMatter TERRAIN_MATTER;
    private void Awake() {
        TERRAIN_MATTER = GetComponent<TerrainMatter>();
    }

    public static double Tolerance {
        get => TOLERANCE;
        set => TOLERANCE = value;
    }

    [SerializeField]
    public bool SHOW_AUTOMATA = true;

    public static int CHUNK_SIZE => chunk_size;

    public static int SQUARE_SIZE
    {
        get => square_size;
    }

    public static int SMOOTH_THRESHOLD
    {
        get => smooth_threshold;
    }
}
