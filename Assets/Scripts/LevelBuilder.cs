using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
	static int width = 200;
	static int height = 100;
	public GameObject GTile;
	public GameObject Player;
	public GameObject Hazard;
    public GameObject Ramp_Left;
    public GameObject Ramp_Right;

    int[,] map = new int[height, width];
	/*int[,] map = 
	{
		{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
		{1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1}
	};
    */

	// Use this for initialization
	void Start()
	{
        //CellularAutomata();

        //BuildMap();
        PlatformGenerator pg = new PlatformGenerator();
        replaceArea(5, 0, pg.CreateIsland(40, 40));
        replaceArea(10, 50, pg.CreateIsland(20, 40));
        replaceArea(80, 70, pg.CreateIsland(30, 50));
        replaceArea(60, 40, pg.CreateIsland(20, 30));
        replaceArea(145, 55, pg.CreateIsland(30, 60));
        replaceArea(100, 5, pg.CreateIsland(30, 60));
        map[0, width / 2] = 2;

        for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{

				switch (map[height - y - 1, x])
				{
					case Tiles.GROUND_TILE:
						Instantiate(GTile, new Vector3(x, y, 0), Quaternion.identity);
						break;
					case Tiles.PLAYER:
						Instantiate(Player, new Vector3(x, y, 0), Quaternion.identity);
                        break;
					case Tiles.HAZARD:
						Instantiate(Hazard, new Vector3(x, y, 0), Quaternion.identity);
						break;
                    case Tiles.RAMP_LEFT:
                        Instantiate(Ramp_Left, new Vector3(x, y, 0), Quaternion.identity);
                        break;
                    case Tiles.RAMP_RIGHT:
                        Instantiate(Ramp_Right, new Vector3(x, y, 0), Quaternion.identity);
                        break;
                }
			}
		}
    }

    // Replaces elements of map with elements of array at location x,y
    void replaceArea(int x, int y, int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (y + i < map.GetLength(0) && x + j < map.GetLength(1))
                {
                    if (array[i, j] != Tiles.EMPTY_TILE)
                    {
                        map[y + i, x + j] = array[i, j];
                    }
                }
            }
        }
    }
    


    /*
     * Test of cellular automata to generate islands as platforms.
     * Play around with the numbers for chance of tiles.
     */
    int tileChance = 30; // Chance of tile being placed at each x,y at initialization.
    int deathLimit = 3;  // Kills tiles with less than deathLimit neighboring tiles each step.
    int birthLimit = 3;  // Creates tiles at locations with more than birthLimit neighboring tiles each step.
    int numberOfSteps = 4;

    void CellularAutomata()
    {
        //Create a new map
        int[,] levelMap = new int[height, width];
        //Set up the map with random values
        levelMap = InitiliazeLevel(levelMap); ;
        //And now run the simulation for a set number of steps
        for (int i = 0; i < numberOfSteps; i++)
        {
            levelMap = DoSimulationStep(levelMap);
        }
        // ADD A FINAL STEP SIMULATION HERE TO CLEAN UP THE LEVEL (ie remove small islands by only killing tiles). 
		for (int i = 0; i < 3; i++)
		{
			levelMap = CleanUpTiles(levelMap, 4);
		}
		for (int i = 0; i < 4; i++)
		{
			levelMap = CleanUpTiles(levelMap, 2);
		}
		levelMap[0, width / 2] = 2;
        map = levelMap;

        // Concept:
        // Flood fill algorithm can be used to determine "islands"/platform areas.
        // Then find the mean of connected tiles to find the center of the islands and we can place a platform there.
    }

	int[,] InitiliazeLevel(int[,] level)
	{
		for (int x = 0; x < height - 1; x++)
		{
			for (int y = 0; y < width - 1; y++)
			{
				if (Random.Range(0, 100) < tileChance)
				{
					level[x, y] = 1;
				}
			}
		}
		return level;
	}

    int[,] DoSimulationStep(int[,] oldLevel)
    {
        int[,] newLevel = new int[height, width];
        //Loop over each row and column of the map
        for (int x = 0; x < height - 1; x++)
        {
            for (int y = 0; y < width - 1; y++)
            {
                int neighbors = CountNeighbourTiles(oldLevel, x, y);
                //The new value is based on our simulation rules
                //First, if a cell is alive but has too few neighbours, kill it.
                if (oldLevel[x, y] == 1)
                {
                    if (neighbors < deathLimit)
                    {
                        newLevel[x, y] = Tiles.EMPTY_TILE;
                    }
                    else
                    {
                        newLevel[x, y] = Tiles.GROUND_TILE;
                    }
                } //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
                else
                {
                    if (neighbors > birthLimit)
                    {
                        newLevel[x, y] = Tiles.GROUND_TILE;
                    }
                    else
                    {
                        newLevel[x, y] = Tiles.EMPTY_TILE;
                    }
                }
            }
        }
        return newLevel;
    }


    //Returns the number of cells in a ring around (x,y) that have a tile.
    int CountNeighbourTiles(int[,] level, int x, int y)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbourX = x + i;
                int neighbourY = y + j;
                if (i == 0 && j == 0)
                {
                    //Do nothing when looking at itself
                }
                // Checking at edges.
                else if (neighbourX < 0 || neighbourY < 0 || neighbourX >= height-1 || neighbourY >= width-1)
                {
                    // Incrementing count here will create a border. 
                    //count++;
                }
                // Normal check.
                else if (level[neighbourX, neighbourY] == Tiles.GROUND_TILE)
                {
                    count++;
                }
            }
        }
        return count;
    }

	//int cleanUpLimit - Kills tiles with less than cleanIpLimit neighboring tiles during each clean up step.
	int[,] CleanUpTiles(int[,] oldLevel, int cleanUpLimit)
	{
		int[,] newLevel = new int[height, width];
		for (int x = 0; x < height - 1; x++)
		{
			for (int y = 0; y < width - 1; y++)
			{
				int neighbors = CountNeighbourTiles (oldLevel, x, y);
				// If a cell is alive but has too few neighbours, kill it.
				if (oldLevel [x, y] == Tiles.GROUND_TILE)
				{
					if (neighbors < cleanUpLimit) {
						newLevel [x, y] = Tiles.EMPTY_TILE;
					}
					else {
						newLevel [x, y] = Tiles.GROUND_TILE;
					}
				}
				else
				{
					newLevel [x, y] = oldLevel [x, y];
				}
			}
		}
		return newLevel;
	}


    /*
     * Possible Level Construction Stages:
     * 1) Decide if horizontally/vertically symmetrical or assymetrical.  Construct with half array for symmetrical levels.
     * 2) Randomize optional choice of solid walls, ceiling, and floor.
     * 3) Randomize spawn points, only 2 if symmetrical level, constrained by minimum distance between each other and mirrored side.
     * 4) Place (generic) premade platforms or generated platforms (platform generator) based on the following possible constraints:
     *      - There must be at least one platform located below the spawn points or within reach of grappling hook/initial acceleration movement. (spawns can be in the air)
     *      - If spawns are vertically aligned (within +/- x tiles), there must be a platform between them.
     *      - Build from bottom up, when placing platforms, randomize platform distances based on a density constraint to achieve:
     *          - Some platforms directly above each other have vertical distances which are equal or less than grappling hook rope max length. (smooth swinging)
     *          - Other platforms areas would be more suitable for jumping between and above.
     *      - Example density: Tile/platform density is higher at the bottom than at the top.
     *      - Delete tiles/platforms if necessary.
     * 5) Trap placement algorithm. (could be during platform creation or use of AI and A* pathfinding to determine most/least travelled tiles on the map)
     * 6) For symmetrical levels, mirror array depending on vertical/horizontal symmetry.
     * 7) Randomly choose a theme and construct level using appropriate themed tiles and background.
    */

    void BuildMap()
	{
		for (int i = 0; i < 13; i++)
		{
			var randLen = Random.Range(4, 10);
			int randX, randY;
			
			if (i%2 == 0)
			{
				randX = Random.Range(0, width);
				randY = Random.Range(0, height-randLen);

				for (int j = -1; j <= randLen; j++)
				{
					if (randY+j >= height) break;
					if (randY+j < 0) continue;
					
					if (randX > 0) map[randY + j, randX-1] = 0;
					map[randY + j, randX] = 0;
					if (randX < width-1) map[randY + j, randX+1] = 0;
				}

				for (int j = 0; j < randLen; j++)
				{
					map[randY + j, randX] = Tiles.GROUND_TILE;
				}
			}
			else
			{
				randY = Random.Range(0, height);
				randX = Random.Range(0, width-randLen);
				
				for (int j = -1; j <= randLen; j++)
				{
					if (randX+j >= width) break;
					if (randX+j < 0) continue;
					
					if (randY > 0) map[randY-1, randX + j] = 0;
					map[randY, randX + j] = 0;
					if (randY < height-1) map[randY+1, randX + j] = 0;
				}


				for (int j = 0; j < randLen; j++)
				{
					map[randY, randX + j] = Tiles.GROUND_TILE;
				}
			}

			map[0, width / 2] = 2;
		}
		
		
	}
}