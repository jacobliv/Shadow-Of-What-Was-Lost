using System.Collections;
using System.Collections.Generic;
using MainGame.Terrain.Generation;
using UnityEngine;
using Random = System.Random;

public class ControlNodeCreator : MonoBehaviour {
    [SerializeField]
    public TerrainMatter terrainMatter;

    private Random random;

    public void generate(Bounds bound, GenerationData data) {
        random = new Random(0);
        Dictionary<Vector2,ControlNode> controlNodes = new Dictionary<Vector2, ControlNode>();
        Dictionary<Vector2,int> posToIndex = new Dictionary<Vector2,int>();
        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        int i = 0;
        Bounds bounds = new Bounds(new Vector2(bound.MIN.x-GameManager.SQUARE_SIZE,bound.MIN.y-GameManager.SQUARE_SIZE),
                                    new Vector2(bound.MAX.x+GameManager.SQUARE_SIZE,bound.MAX.y+GameManager.SQUARE_SIZE));
        foreach (KeyValuePair<Vector2,int> values in data.CellularAutomata) {
            i = createNodes(values, bounds, posToIndex, i, vertices, controlNodes, colors);
        }

        data.ControlNodes = controlNodes;
        data.posToIndex = posToIndex;
        data.vertices = vertices;
        data.colors = colors;
    }

    private int createNodes(KeyValuePair<Vector2, int> values,
                            Bounds bounds,
                            Dictionary<Vector2, int> posToIndex,
                            int i,
                            List<Vector3> vertices,
                            Dictionary<Vector2, ControlNode> controlNodes, List<Color> colors) {

        Node below = null;
        Node center = null;
        Node right = null;
        Vector2 belowPos = new Vector2(values.Key.x, values.Key.y - GameManager.SQUARE_SIZE / 2f);
        if (bounds.inside(belowPos)) {
            below = new Node(belowPos);
            colors.Add(new Color(0f,0f,0f,.5f));
            posToIndex.Add(belowPos, i++);
            vertices.Add(new Vector3(belowPos.x, belowPos.y, 0));
        }

        Vector2 centerPos = new Vector2(values.Key.x + GameManager.SQUARE_SIZE / 2f,
            values.Key.y - GameManager.SQUARE_SIZE / 2f);
        if (bounds.inside(centerPos)) {
            center = new Node(centerPos);
            colors.Add(new Color(0f,0f,0f,.5f));

            posToIndex.Add(centerPos, i++);
            vertices.Add(new Vector3(centerPos.x, centerPos.y, 0));
        }

        Vector2 rightPos = new Vector2(values.Key.x + GameManager.SQUARE_SIZE / 2f, values.Key.y);
        if (bounds.inside(rightPos)) {
            right = new Node(rightPos);
            colors.Add(new Color(0f,0f,0f,.5f));

            posToIndex.Add(rightPos, i++);
            vertices.Add(new Vector3(rightPos.x, rightPos.y, 0));
        }

        ControlNode controlNode = new ControlNode(values.Key, values.Value, below, center, right);
        if (values.Value > 0) {
            Matter indexedMatter = terrainMatter.getIndexedMatter(values.Value);
            colors.Add(indexedMatter.color);
        }
        else {
            colors.Add(Color.clear);
        }

        controlNodes.Add(values.Key, controlNode);
        posToIndex.Add(values.Key, i++);
        vertices.Add(new Vector3(values.Key.x, values.Key.y, 0));
        return i;
    }
}
