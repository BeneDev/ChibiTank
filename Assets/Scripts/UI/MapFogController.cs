using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapFogController : MonoBehaviour {

    [SerializeField] GameObject imageObject;
    RawImage image;

    GameObject player;
    Vector2 pixelToUnhideAround;

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
        pixelToUnhideAround = new Vector2(player.transform.position.x + 500f, player.transform.position.z + 500f);
        SetTransparencyWherePlayerWas(pixelToUnhideAround, unhideRadius);
        //print(mapFogTexture.GetPixel((int)pixelToUnhideAround.x, (int)pixelToUnhideAround.y).a);
    }

    void SetTransparencyWherePlayerWas(Vector2 position, int radius)
    {
        for (int x = (int)position.x - radius; x < (int)position.x + radius; x++)
        {
            for (int y = (int)position.y - radius; y < (int)position.y + radius; y++)
            {
                //if ((int)Mathf.Sqrt(Mathf.Pow(position.x - x, 2) + Mathf.Pow(position.y - y, 2)) < radius)
                //{
                //    ChangePixelAlphaTo(position, 0f);
                //}
                ChangePixelAlphaTo(x, y, 0f);
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
