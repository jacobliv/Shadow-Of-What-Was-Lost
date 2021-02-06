using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushMeshCreator : MonoBehaviour {

    public void create(Bush bush) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();
        Dictionary<Vector3,int> vertInd = new Dictionary<Vector3, int>();
        foreach (Branch branch in bush.rootBranches) {
            recurseBranches(branch, vertices, indices,vertInd, colors);




        }

        bush.indices = indices;
        bush.vertices = vertices;
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices.ToArray();
        // mesh.uv = newUV;
        mesh.triangles = indices.ToArray();
        mesh.colors = colors.ToArray();
    }

    private void recurseBranches(Branch branch, List<Vector3> vertices, List<int> indices,
        Dictionary<Vector3, int> vertInd, List<Color> colors) {
        addVertex(branch.inboundLeft(), vertices, vertInd, colors);
        addVertex(branch.outboundLeft(), vertices, vertInd, colors);
        addVertex(branch.outboundRight(), vertices, vertInd, colors);
        addVertex(branch.inboundRight(), vertices, vertInd, colors);

        indices.Add(vertInd[branch.inboundLeft()]);
        indices.Add(vertInd[branch.outboundLeft()]);
        indices.Add(vertInd[branch.inboundRight()]);
        indices.Add(vertInd[branch.outboundLeft()]);
        indices.Add(vertInd[branch.outboundRight()]);
        indices.Add(vertInd[branch.inboundRight()]);
        foreach (Branch branchChild in branch.children) {
            recurseBranches(branchChild,vertices,indices,vertInd,colors);
        }
    }

    private void addVertex(Vector2 vert, List<Vector3> vertices, Dictionary<Vector3, int> vertInd, List<Color> colors) {
        Vector3 v = new Vector3(vert.x,vert.y,0);
        if(vertInd.ContainsKey(v)) return;
        vertices.Add(v);
        vertInd.Add(v,vertices.Count-1);
        // colors.Add(new Color(83 / 256f, 53 / 256f, 10 / 256, 1));
    }
}
