using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlineGenerator : MonoBehaviour
{
    /*public int width = 64; 
    public int height = 64; 
    public float scale = 20f; 

    void Start()
    {
        Texture2D noiseTexture = GeneratePerlinNoise();
        SaveTexture(noiseTexture, "PerlinNoise");

        GetComponent<Renderer>().material.mainTexture = noiseTexture;
    }

    Texture2D GeneratePerlinNoise()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = new Color(sample, sample, sample); 
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }
    void SaveTexture(Texture2D texture, string filename)
    {
        System.IO.File.WriteAllBytes(Application.dataPath + "/" + filename + ".png", texture.EncodeToPNG());
        UnityEditor.AssetDatabase.Refresh();
    }*/
}
