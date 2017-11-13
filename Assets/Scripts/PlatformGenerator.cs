using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PlatformGenerator {

    public int[,] CreateIsland(int height, int width)
    {
        int[,] platform = new int[height, width];
        platform = PerlinNoiseIsland(platform);
        platform = AddRamps(platform);
        platform = AddSurface(platform);
        return platform;
    }

    private int[,] AddSurface(int[,] platform)
    {
        float platformWidth = platform.GetLength(1);
        float platformHeight = platform.GetLength(0);

        for (int i = 0; i < platformWidth; i++)
        {
            for (int j = 0; j < platformHeight; j++)
            {
                if (platform[j, i] == Tiles.GROUND_TILE && j-1 >= 0 && (platform[j - 1, i] == Tiles.EMPTY_TILE || platform[j-1, i] == Tiles.SPACING))
                {
                    platform[j, i] = Tiles.SURFACE_TILE;
                }
            }
        }
        return platform;
    }

    int[,] AddRamps(int[,] platform)
    {
        float platformWidth = platform.GetLength(1);
        float platformHeight = platform.GetLength(0);

        for (int i = 0; i < platformWidth; i++)
        {
            for (int j = 0; j < platformHeight - 1; j++)
            {
                if (platform[j, i] == Tiles.EMPTY_TILE)
                {
                    // if bottom adjacent tile and right adjacent tile are ground tiles AND top and left tiles are empty tiles.  Also array edge checks.
                    if ((platform[j + 1, i] == Tiles.GROUND_TILE)
                        && (i + 1 < platformWidth && platform[j, i + 1] == Tiles.GROUND_TILE)
                        && (j - 1 < 0 || platform[j - 1, i] == Tiles.EMPTY_TILE)
                        && (i - 1 <= 0 || platform[j, i - 1] == Tiles.EMPTY_TILE))
                    {
                        // Add a Ramp Left tile here
                        platform[j, i] = Tiles.RAMP_LEFT;
                        platform[j + 1, i] = Tiles.LEFT_CORNER;
                    }
                    // if bottom adjacent tile and left adjacent tile are ground tiles AND top and right tiles are empty tiles.
                    if ((platform[j + 1, i] == Tiles.GROUND_TILE)
                        && (i + 1 >= platformWidth || platform[j, i + 1] == Tiles.EMPTY_TILE)
                        && (j - 1 < 0 || platform[j - 1, i] == Tiles.EMPTY_TILE)
                        && (i - 1 >= 0 && platform[j, i - 1] == Tiles.GROUND_TILE))
                    {
                        // Add a Ramp Left tile here
                        platform[j, i] = Tiles.RAMP_RIGHT;
                        platform[j + 1, i] = Tiles.RIGHT_CORNER;
                    }
                }
            }
        }
        return platform;
    }

    int[,] PerlinNoiseIsland(int[,] platform)
    {
        float platformWidth = platform.GetLength(1);
        float platformHeight = platform.GetLength(0);

        float islandHeight = platformHeight / 10;
        float elevationFrequency = Random.Range(3f, 6f);
        float depressionFrequency = Random.Range(1f, 3f); // x coordinate;
        float yCoordinate = Random.Range(0f, 1000f);

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
