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

                Color tempColor = new Color(currPNC, currPNC, currPNC, 1);

                mapFogTexture.SetPixel(x, y, tempColor);

            }
        }
        mapFogTexture.Apply();
        image.texture = mapFogTexture;
    }

    private void Update()
    {
        pixelToUnhideAround = new Vector2(player.transform.position.x, player.transform.position.z);
        SetTransparencyWherePlayerWas(pixelToUnhideAround, unhideRadius);
    }

    void SetTransparencyWherePlayerWas(Vector2 position, int radius)
    {
        ChangePixelAlphaTo(position, 0f);
    }

    void ChangePixelAlphaTo(Vector2 pixel, float alpha)
    {
        Color tempColor = mapFogTexture.GetPixel((int)pixel.x, (int)pixel.y);
        tempColor.a = alpha;
        mapFogTexture.SetPixel((int)pixel.x, (int)pixel.y, tempColor);
    }

}
