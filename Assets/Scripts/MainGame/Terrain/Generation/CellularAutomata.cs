using System;
using System.Collections.Generic;
using System.Numerics;
using MainGame.Terrain.Generation;
using UnityEngine;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;

public class CellularAutomata : MonoBehaviour {
    public Vector2Int noiseOffset { get; set; }
    [SerializeField]
    public TerrainMatter terrainMatter;

    [SerializeField] 
    public int surfaceLevel;

    public int worldHeight;
    
    private Random random;

    public void Awake() {
        random = new Random();
    }

    public void generate(Vector2 worldOffset, GenerationData generationData) {
        Dictionary<Vector2, int> automata = createAutomata(worldOffset, generationData);
        Dictionary<Vector2, int> smoothedAutomata = smoothAutomata(automata, generationData);
        addInAdjacentData(generationData, smoothedAutomata);
        generationData.CellularAutomata = smoothedAutomata;
    }

    private void addInAdjacentData(GenerationData generationData, Dictionary<Vector2, int> smoothedAutomata) {
        foreach (KeyValuePair<Vector2, int> values in generationData.AdjacentData)
            if (smoothedAutomata.ContainsKey(values.Key))
                smoothedAutomata[values.Key] = values.Value;
            else
                smoothedAutomata.Add(values.Key, values.Value);
    }

    private Dictionary<Vector2, int> smoothAutomata(Dictionary<Vector2, int> automata,
                                                    GenerationData generationData) {
        Dictionary<Vector2, int> smoothedAutomata = new Dictionary<Vector2, int>();
        foreach (KeyValuePair<Vector2, int> values in automata) {
            int value = getValue(values.Key, generationData.AdjacentData, automata);
            smoothedAutomata.Add(values.Key, value);

        }

        return smoothedAutomata;
    }

    private int getValue(Vector2 pos, Dictionary<Vector2, int> adData, Dictionary<Vector2, int> automata) {
        var count = 0;
        Dictionary<int,int> counts = new Dictionary<int, int>();
        for (int x = -1; x <= 1; x++){
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue;
                Vector2 nPos = new Vector2(pos.x + GameManager.SQUARE_SIZE * x, pos.y + GameManager.SQUARE_SIZE * y);
                if (!automata.ContainsKey(nPos) && !adData.ContainsKey(nPos)) continue;
                if (automata.ContainsKey(nPos)) {
                    if (automata[nPos] >= 1) {
                        count++;
                        addToCounts(counts, automata[nPos]);
                    }
                    continue;
                }

                if (adData[nPos] >= 1) {
                    count++;
                    addToCounts(counts, adData[nPos]);
                }
                
            }
        }

        if (count >= GameManager.SMOOTH_THRESHOLD) {
            int mat = 0;
            int max = 0;
            foreach (KeyValuePair<int,int> values in counts) {
                if (values.Value > max) {
                    max = values.Value;
                    mat = values.Key;
                }
            }
            
            return mat>0?mat:1;
        }
        return 0;
    }

    private void addToCounts(Dictionary<int, int> counts, int material) {
        int count = 0;
        if (counts.ContainsKey(material)) {
            count = counts[material];
            
        }
        counts[material] = count+1;
    }

    /// <summary>
    /// Create the cellular automata at the specified location in the world
    /// </summary>
    /// <param name="worldOffset">Location in the world that the automata is created</param>
    /// <param name="data">Where we store all of the generation data that may be used later on</param>
    /// <returns></returns>
    private Dictionary<Vector2, int> createAutomata(Vector2 worldOffset,
                                                    GenerationData data) {
        (float min,float max) subSurfaceNoiseMinMax = (float.MaxValue,float.MinValue);

        int seed = 2461532;
        Dictionary<Vector2,float> automata = new Dictionary<Vector2, float>();
        for (int x = -1; x <= data.Bounds.Size.Width / GameManager.SQUARE_SIZE + 1; x++) {

            for (int y = -1; y <= data.Bounds.Size.Height / GameManager.SQUARE_SIZE + 1; y++) {
                Vector2 noiseCoords = getNoiseCoords(x,y,seed,worldOffset);
                Vector2 worldCoords = getWorldCoords(x,y,worldOffset);
                
                if (data.AdjacentData.ContainsKey(worldCoords)) continue;
                subSurfaceNoiseMinMax = generateSubSurface(worldCoords,noiseCoords, subSurfaceNoiseMinMax, automata);

            }
        }

        return normalizeNoise(subSurfaceNoiseMinMax, automata);
    }
    
    /// <summary>
    /// Get the world coords of the specific iteration
    /// </summary>
    /// <param name="x">The current iteration x to calculate the noise coord with</param>
    /// <param name="y">The current iteration y to calculate the noise coord with</param>
    /// <param name="worldOffset">How far from the origin this chunk is offset</param>
    /// <returns></returns>
    private Vector2 getWorldCoords(float x, float y, Vector2 worldOffset) {
        return new Vector2((x + worldOffset.x) * GameManager.SQUARE_SIZE,
                           (y + worldOffset.y) * GameManager.SQUARE_SIZE);
    }

    /// <summary>
    /// Get the coords for generating the noise. This has to be offset from both the x and y axis because the perlin noise
    /// will reflect across both axes
    /// (TODO: create a custom perlin noise library to remove this code)
    /// </summary>
    /// <value name="noiseOffset">The number of chunks in the x and y direction to be away from the axes (see above description)</value>
    /// <param name="x">The current iteration x to calculate the noise coord with</param>
    /// <param name="y">The current iteration y to calculate the noise coord with</param>
    /// <param name="seed">The seed that can generate the same values for the same seed</param>
    /// <param name="worldOffset">How far from the origin this chunk is offset</param>
    /// <returns></returns>
    private Vector2 getNoiseCoords(float x, float y, int seed, Vector2 worldOffset) {
        return new Vector2(x + seed +
                           (worldOffset.x / GameManager.SQUARE_SIZE) +
                           (noiseOffset.x * GameManager.CHUNK_SIZE),
                            y + seed +
                            (worldOffset.y / GameManager.SQUARE_SIZE) +
                            (noiseOffset.y * GameManager.CHUNK_SIZE)) / 20f;
    }

    /// <summary>
    /// Generate the cave system below the surface of the world
    /// </summary>
    /// <param name="worldCoords">The location the noise is generating at</param>
    /// <param name="noiseCoords">The noise coords to move the perlin noise away from the x/y axes</param>
    /// <param name="minMax">The current min max of the noise</param>
    /// <param name="automata">The automata map to add the values to</param>
    /// <returns>The current min max of the noise</returns>
    private (float min, float max) generateSubSurface(Vector2 worldCoords,
                                                      Vector2 noiseCoords,
                                                      (float min, float max) minMax,
                                                      Dictionary<Vector2, float> automata) {
        float value = Mathf.PerlinNoise(noiseCoords.x,noiseCoords.y);

        automata.Add(worldCoords, value);
        Debug.Log("Adding: " +worldCoords + " - " + value );
        return minMaxer(minMax,value);
    }
    
    /// <summary>
    /// Return the overall min max of the chunk. If value is below or above current min max, update
    /// </summary>
    /// <param name="minMax">Current min and max noise value</param>
    /// <param name="value">Value of the current iterations noise</param>
    /// <returns>The min max after evaluation</returns>
    private (float min, float max) minMaxer((float min, float max) minMax, float value) {
        if (value < minMax.min)
            minMax.min = value;
        else if (value > minMax.max) 
            minMax.max = value;
        return minMax;
    }

    private Dictionary<Vector2, int> normalizeNoise((float min, float max) minMax, Dictionary<Vector2, float> automata) {
        Dictionary<Vector2, int> finalAutomata = new Dictionary<Vector2, int>();
        float total = minMax.min < 0 ? Math.Abs(minMax.min) + Math.Abs(minMax.max) : minMax.max - minMax.min;
        foreach (var keyValue in automata) {
            float val;
            if (minMax.min < 0) {
                val = (keyValue.Value + Math.Abs(minMax.min)) / total;
            }
            else {
                val = (keyValue.Value - minMax.min) / total;
            }

            if (val > .45f) {
                int material = addBaseMaterial();
                int oreMaterial = addOreMaterial();
                material = oreMaterial > 0 ? oreMaterial : material; 
                finalAutomata.Add(keyValue.Key, material);
            } else {
                finalAutomata.Add(keyValue.Key, 0);
            }
        }

        return finalAutomata;
    }

    private int addOreMaterial() {
        Dictionary<string,Matter> oreMatters = terrainMatter.oreMatters;
        foreach (Matter matter in oreMatters.Values) {
            float r = random.Next(0, 1000)/5000f;
            if (matter.frequency >= r) {
                return matter.index;
            }
        }
        return 0;
    }

    private int addBaseMaterial() {
        return 1;
    }
}