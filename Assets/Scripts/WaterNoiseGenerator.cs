using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNoiseGenerator : MonoBehaviour {

    [SerializeField] float power = 3f;
    [SerializeField] float scale = 1f;
    [SerializeField] float timeScale = 1f;

    float offsetX;
    float offsetY;
    MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
        meshFilter = GetComponent<MeshFilter>();
        MakeNoise();
	}
	
	// Update is called once per frame
	void Update () {
        MakeNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetY += Time.deltaTime * timeScale;
	}

    void MakeNoise()
    {
        Vector3[] verts = meshFilter.mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].y = CalculateHeight(verts[i].x, verts[i].z) * power;
        }

        meshFilter.mesh.vertices = verts;
    }

    float CalculateHeight(float x, float y)
    {
        float xCord = x * scale + offsetX;
        float yCord = y * scale + offsetY;

        return Mathf.PerlinNoise(xCord, yCord);
    }
}
