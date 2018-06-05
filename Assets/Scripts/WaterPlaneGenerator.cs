using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script generates a mesh of the given attributes, which can be manipulated to create a wave effect
/// </summary>
public class WaterPlaneGenerator : MonoBehaviour {

    #region Fields

    [SerializeField] float planeSize = 1f; // The absolute size of the plane
    [SerializeField] int planeGridSize = 16; // The size of the single quads, the plane is made of. Effectively manipulates the resolution of the plane

    private MeshFilter meshFilter; // The mesh filter to generate the mesh in

    #endregion

    // Gets the reference to the mesh filter and creates the mesh for it
    void Start () {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GenerateMesh();
	}

    #region Helper Methods

    // Iterate through all the verticies and connect them to ultimately generate the mesh
    private Mesh GenerateMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> verticies = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < planeGridSize + 1; x++)
        {
            for (int y = 0; y < planeGridSize + 1; y++)
            {
                verticies.Add(new Vector3(-planeSize * 0.5f + planeSize * (x / (float)planeGridSize), 0, -planeSize * 0.5f + planeSize * (y / (float)planeGridSize)));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(x / (float)planeGridSize, y / (float)planeGridSize));
            }
        }

        List<int> tris = new List<int>();
        int vertCount = planeGridSize + 1;
        for (int i = 0; i < vertCount * vertCount - vertCount; i++)
        {
            if((i + 1) % vertCount == 0)
            {
                continue;
            }
            tris.AddRange
            (
                new List<int>()
                {
                    i + 1 + vertCount,
                    i + vertCount,
                    i,
                    i,
                    i + 1,
                    i + 1 + vertCount
                }
            );
        }

        m.SetVertices(verticies);
        m.SetNormals(normals);
        m.SetUVs(0, uvs);
        m.SetTriangles(tris, 0);

        return m;
    }

    #endregion

}
