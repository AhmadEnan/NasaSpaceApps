using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    
    [SerializeField]
    private Renderer DisplayPlane;

    public void DrawMap(float [,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        Texture2D mapTexture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, map[x, y]);
            }
        }

        mapTexture.SetPixels(colorMap);
        mapTexture.Apply();
        DisplayPlane.sharedMaterial.mainTexture = mapTexture;
        DisplayPlane.transform.localScale = new Vector3(width, 1, height);
    }
}
