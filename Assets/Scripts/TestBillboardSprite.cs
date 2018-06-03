using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBillboardSprite : MonoBehaviour {

    public Material material;
    Mesh mesh;

    void Start () {
        mesh = new Mesh ();
        mesh.name = "Quad";
        mesh.SetVertices (new List<Vector3> () { new Vector3 (-0.5f, 0.5f, 0), new Vector3 (0.5f, 0.5f, 0), new Vector3 (0.5f, -0.5f, 0), new Vector3 (-0.5f, -0.5f, 0) });
        mesh.SetUVs (0, new List<Vector2> () { new Vector2 (0, 1), new Vector2 (1, 1), new Vector2 (1, 0), new Vector2 (0, 0) });
        mesh.SetTriangles (new int[] { 0, 1, 2, 0, 2, 3 }, 0, true);
    }

    void Update () {
        if (material != null)
            Graphics.DrawMesh (mesh, transform.position, transform.rotation, material, 0);
    }
}