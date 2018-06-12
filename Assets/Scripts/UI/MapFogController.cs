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
        textureWidth = (int)imageObject.GetComponent<RectTransform>().rect.width;
        textureHeight = (int)imageObject.GetComponent<RectTransform>().rect.height;
        mapFogTexture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                int currPixl = textureWidth * y + x;
                float currPNC = Mathf.PerlinNoise((float)x / textureWidth * 10, (float)y / textureHeight * 10);

                // TODO make the perlin noise texture darker in general and set the transparency based on the white factor (the whiter, the more transparent)
                Color tempColor = new Color(currPNC, currPNC, currPNC, 1);

                mapFogTexture.SetPixel(x, y, tempColor);

            }
        }
        mapFogTexture.Apply();
        image.texture = mapFogTexture;
    }

    private void Update()
    {
        SetTransparencyWherePlayerWas((int)player.transform.position.x + 500, (int)player.transform.position.z + 500, unhideRadius);
    }

    void SetTransparencyWherePlayerWas(int x, int y, int radius)
    {
        // TODO Make this a smoothed out circle
        for (int forX = x - radius; forX < x + radius; forX++)
        {
            for (int forY = y - radius; forY < y + radius; forY++)
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
