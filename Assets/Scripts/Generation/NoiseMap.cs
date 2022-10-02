using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap
{
    private static Lehmer.Random randNumGen;
    public static float[,] GenerateNoiseMap(uint _chunckSize, uint seed)
    {
        randNumGen = new Lehmer.Random(seed);
        float[,] noiseMap = new float[_chunckSize, _chunckSize];

        Vector2Int currentSector = Vector2Int.zero;
        if (randNumGen == null)
            randNumGen = new Lehmer.Random(seed);

        for (currentSector.x = 0; currentSector.x < _chunckSize; currentSector.x++)
        {
            for (currentSector.y = 0; currentSector.y < _chunckSize; currentSector.y++)
            {
                uint nSeed = (uint)((currentSector.y) << 16 | (currentSector.x)) * (seed);
                bool starExists = randNumGen.GetNextInt(nSeed, 0, 150) == 1;
                noiseMap[currentSector.x, currentSector.y] = starExists ? 1 : 0;
            }
        }
        return noiseMap;
    }
}
