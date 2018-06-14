using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapFogController : MonoBehaviour {

    [SerializeField] GameObject imageObject;
    RawImage image;

    GameObject player;

    Texture2D mapFogTexture;

    int textureWidth;
    int textureHeight;

    [SerializeField] int unhideRadius = 5;

    private void Awake()
    {
        image = imageObject.GetComponent<RawImage>();
        player = GameObject.FindGameObjectWithTag("Player");
        CreateMapFogTexture();
    }

    private void CreateMapFogTexture()
    {
        textureWidth = (int)imageObject.GetComponent<RectTransform>().rect.width;
        textureHeight = (int)imageObject.GetComponent<RectTransform>().rect.height;
        mapFogTexture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                int currPixl = textureWidth * y + x;
                float currPNC = Mathf.PerlinNoise((float)x / textureWidth * 10, (float)y / textureHeight * 10);
                
                // Set the transparency based on the white amount of the color
                Color tempColor = new Color(currPNC, currPNC, currPNC, (1f - currPNC) + 0.2f);

                mapFogTexture.SetPixel(x, y, tempColor);

            }
        }
        mapFogTexture.Apply();
        image.texture = mapFogTexture;
    }

    private void Start()
    {
        InvokeRepeating("SetTransparencyWherePlayerWas", 0f, 1f);
    }

    void SetTransparencyWherePlayerWas()
    {
        int x = (int)player.transform.position.x + 500;
        int y = (int)player.transform.position.z + 500;
        // TODO Make this a smoothed out circle
        for (int forX = x - unhideRadius; forX < x + unhideRadius; forX++)
        {
            for (int forY = y - unhideRadius; forY < y + unhideRadius; forY++)
            {
                ChangePixelAlphaTo(forX, forY, 0f);
            }
        }
        mapFogTexture.Apply();
        image.texture = mapFogTexture;
    }

    void ChangePixelAlphaTo(int x, int y, float alpha)
    {
        Color tempColor = new Color(0f, 0f, 0f, alpha);
        mapFogTexture.SetPixel(x, y, tempColor);
    }

}
