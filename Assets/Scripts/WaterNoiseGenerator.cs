using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate a plane to use as a water mesh
/// </summary>
public class WaterNoiseGenerator : MonoBehaviour {

    #region Fields

    [SerializeField] float power = 3f; // How high the waves will get
    [SerializeField] float scale = 1f; // How big the waves get
    [SerializeField] float timeScale = 1f; // How fast the waves go over the plane

    float offsetX;
    float offsetY;
    MeshFilter meshFilter;

    #endregion

    #region Unity Messages

    // Get Components and generate the noise plane
    void Start () {
        meshFilter = GetComponent<MeshFilter>();
        MakeNoise();
	}
	
	// Updates the plane to create the waves
	void Update () {
        MakeNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetY += Time.deltaTime * timeScale;
	}

    #endregion

    #region Helper Methods

    // Generate the verticies in different heights to create the plane
    void MakeNoise()
    {
        Vector3[] verts = meshFilter.mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].y = CalculateHeight(verts[i].x, verts[i].z) * power;
        }

        meshFilter.mesh.vertices = verts;
    }

    // Calculate the height of a given verticy
    float CalculateHeight(float x, float y)
    {
        float xCord = x * scale + offsetX;
        float yCord = y * scale + offsetY;

        return Mathf.PerlinNoise(xCord, yCord);
    }

    #endregion

}
