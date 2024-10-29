using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public enum NormalizeMode { local, global};

    public static float[,] GenNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode mode)
    {
        float[,] noiseMap = new float[width, height];

        System.Random rand = new System.Random(seed);
        Vector2[] octOffset = new Vector2[octaves];


        float maxPossibleHeight = 0;
        float amp = 1;
        float freq = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offX = rand.Next(-10000, 10000) + offset.x;
            float offY = rand.Next(-10000, 10000) - offset.y;
            octOffset[i] = new Vector2(offX, offY);

            maxPossibleHeight += amp;
            amp *= persistance;

        }

        if(scale <= 0)
        {
            scale = 0.0001f;
        }
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {

                amp = 1;
                freq = 1;
                float noiseH = 0;

                for (int k = 0; k < octaves; k++)
                {
                    float x = (j + octOffset[k].x - (width/2)) / scale * freq;
                    float y = (i + octOffset[k].y -(height/2)) / scale * freq;

                    float perlinValue = Mathf.PerlinNoise(x, y);
                    noiseH += perlinValue * amp;

                    amp *= persistance;
                    freq *= lacunarity;
                }

                if(noiseH > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseH;
                }
                else if ( noiseH < minNoiseHeight)
                {
                    minNoiseHeight = noiseH;
                }

                noiseMap[j, i] = noiseH;
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (mode == NormalizeMode.local)
                {
                    noiseMap[j, i] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[j, i]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[j, i] + 1) / (2f * maxPossibleHeight / 2.3f);
                    noiseMap[j, i] = Mathf.Clamp(normalizedHeight - 0.55f,0,int.MaxValue);
                }
            }
        }

        return noiseMap;
    }
}
