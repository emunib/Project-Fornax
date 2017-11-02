﻿using System.Collections;
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
        float platformWidth = platform.GetLength(1);
        float platformHeight = platform.GetLength(0);

        float islandHeight = platformHeight / 10;
        float elevationFrequency = Random.Range(2f, 4f);
        float depressionFrequency = Random.Range(1f, 3f); // x coordinate;
        float yCoordinate = Random.Range(0f, 100f);

        int elevation;
        int depression;

        for (int i = 0; i < platformWidth; i++) {
            elevation = (int)(10 * Mathf.PerlinNoise(i/elevationFrequency * 0.1f, yCoordinate));

            if (elevation < platformHeight) {
                platform[elevation, i] = Tiles.GROUND_TILE;
            }

            // Increase multiplier of islandHeight when closer to the center of the island.
            float heightScale;
            float fractionDistToCenter;

            if (i < platformWidth/2) {
                fractionDistToCenter = i / (platformWidth / 2);
            }
            else
            {
                fractionDistToCenter = ((platformWidth / 2) - (i - (platformWidth / 2))) / (platformWidth / 2);

            }
            // Prevent log from returning a negative number.
            if (fractionDistToCenter < 0.1f)
            {
                fractionDistToCenter = 0.1f;
            }
            // islandHeight is some constants
            // Take the log of a fraction of distance to center converted to 1-10 (10 being at the center) with base 6 (higher base = less curve of depression at edges).
            heightScale = islandHeight * Mathf.Log(fractionDistToCenter * 10, 6) * 10;
            //Debug.Log(heightScale);
            depression = (int)(heightScale * Mathf.PerlinNoise(i / depressionFrequency * 0.1f, yCoordinate));

            // Floor of platform will never be above ceiling.
            if (depression > elevation)
            {

                // Fill in area between ceiling and floor.
                for (int k = elevation; k <= depression; k++)
                {
                    if (k < platformHeight)
                    {
                        platform[k, i] = Tiles.GROUND_TILE;
                    }
                }
            }
        }
        return platform;
    }
}
