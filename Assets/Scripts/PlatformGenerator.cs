using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator {

    public int[,] CreateIsland(int height, int width)
    {
        int[,] platform = new int[height, width];
        platform = PerlinNoiseIsland(platform);

        return platform;
    }

    int[,] PerlinNoiseIsland(int[,] platform)
    {
        int platformWidth = platform.GetLength(1);
        int platformHeight = platform.GetLength(0);

        float islandHeight = platformHeight / 10;
        float elevationFrequency = Random.Range(2f, 4f);
        float depressionFrequency = Random.Range(1f, 3f); // x coordinate;
        float yCoordinate = Random.Range(0f, 100f);

        int elevation;
        int depression;

        for (int i = 0; i < platformWidth; i++) {
            elevation = (int)(10 * Mathf.PerlinNoise(i/elevationFrequency * 0.1f, yCoordinate));

            if (elevation < platformHeight) {
                platform[elevation, i] = 1;
            }

            // Increase divisor of islandHeight when closer to the edge of the island.
            float heightScale = islandHeight;
            /*
            if (i < 6) {
                heightScale = islandHeight / ((7 - i)/2);
            }
            else if (i > platformWidth - 6)
            {
                heightScale = islandHeight / ((7 - (platformWidth - i))/2);
            }
            */
            depression = (int)(heightScale * 10 * Mathf.PerlinNoise(i / depressionFrequency * 0.1f, yCoordinate));

            // Floor of platform will never be above ceiling.
            if (depression > elevation)
            {

                // Fill in area between ceiling and floor.
                for (int k = elevation; k <= depression; k++)
                {
                    if (k < platformHeight)
                    {
                        platform[k, i] = 1;
                    }
                }
            }
        }
        return platform;
    }
}
