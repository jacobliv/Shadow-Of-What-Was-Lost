
using UnityEngine;
using Utils;
using static GameManager;

public class MeshGenerator : MonoBehaviour {
    [SerializeField] public GameObject chunks;
    [SerializeField] public GameObject chunkPrefab;
    [Tooltip("Number of chunks to offset base noise to avoid the y and x axes (Mirroring - Refactoring 8.1.1)")]
    [SerializeField] public Vector2Int baseOffset;

    private CellularAutomata _cellularAutomata;
    private ControlNodeCreator _controlNodeCreator;
    private SquareCreator _squareCreator;
    private Triangulator _triangulator;
    private AdjacentDataCollector _adjacentDataCollector;

    //  PG GP P G|P G   P G 
    //        G P  G P G P
    void Start() {
        _cellularAutomata = GetComponent<CellularAutomata>();
        _cellularAutomata.noiseOffset = baseOffset;
        _controlNodeCreator = GetComponent<ControlNodeCreator>();
        _squareCreator = GetComponent<SquareCreator>();
        _triangulator = GetComponent<Triangulator>();
        _adjacentDataCollector = GetComponent<AdjacentDataCollector>();
        _adjacentDataCollector.Chunks = chunks;
        // generateAllConfigurations();

        // generateCloseCluster();

        // generate3();
        // for (int x = 0; x < 10; x++) {
        //     for (int y = 0; y < 10; y++) {
        //         generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * x, SQUARE_SIZE * CHUNK_SIZE * y),
        //             new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        //     }
        // }
        generate(new Bounds(new Vector2(0, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
    }

    private void generate3() {
        int x = 0;
        for (int y = 0; y <= 1; y++) {
            generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * x, SQUARE_SIZE * CHUNK_SIZE * y),
                new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        }
    }

    private void generateCloseCluster() {
        generate(new Bounds(new Vector2(0, 0), new Vector2(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));

        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                if (x == 0 && y == 0) continue;
                generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * x, SQUARE_SIZE * CHUNK_SIZE * y),
                    new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
            }
        }
    }

    private void generateAllConfigurations() {
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -9, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -8, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -5, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -6, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -3, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -3, SQUARE_SIZE * CHUNK_SIZE * -1),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));

        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -1, SQUARE_SIZE * CHUNK_SIZE * -1),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * -1, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));

        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 1, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 2, SQUARE_SIZE * CHUNK_SIZE * -1),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 5, SQUARE_SIZE * CHUNK_SIZE * -1),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 4, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 8, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 7, SQUARE_SIZE * CHUNK_SIZE * -1),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 10, SQUARE_SIZE * CHUNK_SIZE * -1),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
        generate(new Bounds(new Vector2(SQUARE_SIZE * CHUNK_SIZE * 11, 0),
            new Size(SQUARE_SIZE * CHUNK_SIZE, SQUARE_SIZE * CHUNK_SIZE)));
    }

    public void generate(Bounds bounds) {
        GenerationData data = new GenerationData();
        Debug.Log(bounds);
        _adjacentDataCollector.gather(bounds.Center,data);
        data.Bounds = new Bounds(new Vector2(0, 0),
            new Vector2(SQUARE_SIZE * CHUNK_SIZE,
                SQUARE_SIZE * CHUNK_SIZE));
        _cellularAutomata.generate(bounds.MIN, data);
        _controlNodeCreator.generate(data.Bounds, data);
        _squareCreator.generate(data);
        _triangulator.generate(data);
        GameObject chunk = Instantiate(chunkPrefab, new Vector3(bounds.MIN.x,bounds.MIN.y, 0), Quaternion.identity);
        chunk.transform.SetParent(chunks.transform);
        chunk.GetComponent<ChunkManager>().assignData(data);
        ChunksManager chunksManager = chunks.GetComponent<ChunksManager>();
        chunksManager.addChunkData(bounds.Center,data);
    }

}
