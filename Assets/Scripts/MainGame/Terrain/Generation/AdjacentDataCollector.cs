using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AdjacentType;
using static GameManager;

public class AdjacentDataCollector : MonoBehaviour {
    private GameObject chunks;

    public GameObject Chunks {
        get => chunks;
        set => chunks = value;
    }

    public void gather(Vector2 center, GenerationData genData) {
        float offset = (SQUARE_SIZE * CHUNK_SIZE) / 2f;
        ChunksManager chunksManager = chunks.GetComponent<ChunksManager>();
        Dictionary<Vector2,int> adjacentData = new Dictionary<Vector2, int>();
        Dictionary<AdjacentType,List<KeyValuePair<Vector2, int>>> vals = new Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>>(); 
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0) continue;
                gatherData(center, x, y, chunksManager, offset, vals);
            }
        }
        transformToResult(vals, adjacentData,center);
        genData.AdjacentData = adjacentData;

    }

    private static void transformToResult(Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>> vals,
                                          Dictionary<Vector2, int> adjacentData,
                                          Vector2 center) {
        foreach (KeyValuePair<AdjacentType, List<KeyValuePair<Vector2, int>>> val in vals) {
            foreach (KeyValuePair<Vector2, int> keyValue in val.Value) {
                Vector2 adjustedPosition = adjustPosition(keyValue.Key,val.Key, center);
                if (adjacentData.ContainsKey(adjustedPosition)) continue;
                adjacentData.Add(adjustedPosition,keyValue.Value);
            }
        }
    }
/*
 *    ____________________________________
 *   |           |           |           |
 *   |     x     |     x     |      x    |
 *   |___________|___________|___________|
 *   |           |           |           |
 *   |     P     |     G     |     x     |
 *   |___________|___________|___________|
 *   |           |           |           |
 *   |     x     |     x     |    x      |
 *   |___________|___________|___________|
 */
    private static Vector2 adjustPosition(Vector2 originalPos, AdjacentType adjacentType, Vector2 center) {
        switch (adjacentType) {
            case TOP_LEFT:
                return new Vector2(originalPos.x-(CHUNK_SIZE * SQUARE_SIZE), originalPos.y + (CHUNK_SIZE * SQUARE_SIZE));
            case TOP:
                return new Vector2(originalPos.x, originalPos.y + (CHUNK_SIZE * SQUARE_SIZE));
            case TOP_RIGHT:
                return new Vector2(originalPos.x+(CHUNK_SIZE * SQUARE_SIZE), originalPos.y + (CHUNK_SIZE * SQUARE_SIZE)); 
            case LEFT:
                return new Vector2(originalPos.x-(CHUNK_SIZE * SQUARE_SIZE), originalPos.y);
            case RIGHT:
                return new Vector2(originalPos.x+(CHUNK_SIZE * SQUARE_SIZE), originalPos.y);
            case BOTTOM_LEFT:
                return new Vector2(originalPos.x-(CHUNK_SIZE * SQUARE_SIZE), originalPos.y - (CHUNK_SIZE * SQUARE_SIZE));
            case BOTTOM:
                return new Vector2(originalPos.x, originalPos.y - (CHUNK_SIZE * SQUARE_SIZE));
            case BOTTOM_RIGHT:
                return new Vector2(originalPos.x+(CHUNK_SIZE * SQUARE_SIZE), originalPos.y - (CHUNK_SIZE * SQUARE_SIZE)); 
            default:
                Debug.Log("Some weird adjacent type");
                throw new ArgumentOutOfRangeException(nameof(adjacentType), adjacentType, null);
        }
    }

    private static void gatherData(Vector2 center,
                                   int x,
                                   int y,
                                   ChunksManager chunksManager,
                                   float offset,
                                   Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>> vals) {
        Vector2 pos = new Vector2(center.x + (x * SQUARE_SIZE * CHUNK_SIZE), center.y + (y * SQUARE_SIZE * CHUNK_SIZE));
        Optional<GenerationData> data = chunksManager.get(pos);
        if (!data.HasValue) return;
        Vector2 cent = new Vector2((SQUARE_SIZE*CHUNK_SIZE)/2f,(SQUARE_SIZE*CHUNK_SIZE)/2f);
        float xP =  /*center.x*/ cent.x- x * offset - (Math.Sign(x) * SQUARE_SIZE);
        float xP2 = /*center.x*/ cent.x- x * offset + (Math.Sign(x) * SQUARE_SIZE);
        float xP3 = /*center.x*/ cent.x- x * offset;

        float yP  = cent.y - y * offset - (Math.Sign(y) * SQUARE_SIZE);
        float yP2 = cent.y - y * offset + (Math.Sign(y) * SQUARE_SIZE);
        float yP3 = cent.y - y * offset;

        getCorners(x, y, vals, data, xP, yP, xP2, yP2, xP3, yP3);
        getLeftAndRight(x, y, vals, data, xP, xP2, xP3);
        getTopAndBottom(x, y, vals, data, yP, yP2, yP3);
    }

    private static void getTopAndBottom(int x, int y, Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>> vals, Optional<GenerationData> data, float yP, float yP2, float yP3) {
        if (x == 0 && y != 0) {
            IEnumerable<KeyValuePair<Vector2, int>> val = data.Value.CellularAutomata.Where(values =>
                Math.Abs(values.Key.y - yP) < Tolerance ||
                Math.Abs(values.Key.y - yP2) < Tolerance ||
                Math.Abs(values.Key.y - yP3) < Tolerance);
            AdjacentType type = TOP_LEFT;
            if (y == -1) {
                type = BOTTOM;
            }
            else if (y == 1) {
                type = TOP;
            }

            insertIntoDictionary(vals, val, type);
        }
    }

    private static void getLeftAndRight(int x, int y, Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>> vals, Optional<GenerationData> data, float xP, float xP2, float xP3) {
        if (x != 0 && y == 0) {
            IEnumerable<KeyValuePair<Vector2, int>> val = data.Value.CellularAutomata.Where(values =>
                Math.Abs(values.Key.x - xP) < Tolerance ||
                Math.Abs(values.Key.x - xP2) < Tolerance ||
                Math.Abs(values.Key.x - xP3) < Tolerance);
            AdjacentType type = TOP_LEFT;
            if (x == -1) {
                type = LEFT;
            }
            else if (x == 1) {
                type = RIGHT;
            }

            insertIntoDictionary(vals, val, type);
        }
    }

    private static void getCorners(int x, int y, Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>> vals, Optional<GenerationData> data, float xP, float yP, float xP2, float yP2,
        float xP3, float yP3) {
        if (x != 0 && y != 0) {
            IEnumerable<KeyValuePair<Vector2, int>> val = data.Value.CellularAutomata.Where(values =>
                (Math.Abs(values.Key.x - xP) < Tolerance && Math.Abs(values.Key.y - yP) < Tolerance) ||
                (Math.Abs(values.Key.x - xP2) < Tolerance && Math.Abs(values.Key.y - yP2) < Tolerance) ||
                (Math.Abs(values.Key.x - xP3) < Tolerance && Math.Abs(values.Key.y - yP3) < Tolerance));
            AdjacentType type = TOP_LEFT;
            if (x == -1 && y == 1) {
                type = TOP_LEFT;
            }
            else if (x == 1 && y == 1) {
                type = TOP_RIGHT;
            }
            else if (x == 1 && y == -1) {
                type = BOTTOM_RIGHT;
            }
            else if (x == -1 && y == -1) {
                type = BOTTOM_LEFT;
            }

            insertIntoDictionary(vals, val, type);
        }
    }

    private static void insertIntoDictionary(Dictionary<AdjacentType, List<KeyValuePair<Vector2, int>>> vals,
                                             IEnumerable<KeyValuePair<Vector2, int>> val,
                                             AdjacentType type) {
        // getOffset()
        if (!vals.ContainsKey(type)) {
            vals.Add(type, new List<KeyValuePair<Vector2, int>>(val));
        }
        else {
            List<KeyValuePair<Vector2, int>> keyValuePairs = vals[type];
            keyValuePairs.AddRange(val);
            vals[type] = keyValuePairs;
        }
    }
}
