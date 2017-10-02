using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
	static int width = 28;
	static int height = 9;
	public Transform GTile;
	public Transform Player;
	public Transform Hazard;

	int[,] map = new int[height, width];
//	int[,] map = 
//	{
//		{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
//		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
//		{0, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
//		{0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
//		{1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1}
//	};

	// Use this for initialization
	void Start()
	{
		BuildMap();
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{

				switch (map[height - y - 1, x])
				{
					case 1:
						Instantiate(GTile, new Vector3(x, y, 0), Quaternion.identity);
						break;
					case 2:
						Instantiate(Player, new Vector3(x, y, 0), Quaternion.identity);
						break;
					case 3:
						Instantiate(Hazard, new Vector3(x, y, 0), Quaternion.identity);
						break;
				}
			}
		}
	}
	
	void BuildMap()
	{
		for (int i = 0; i < 10; i++)
		{
			var randLen = Random.Range(5, 10);
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
					map[randY + j, randX] = 1;
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
					map[randY, randX + j] = 1;
				}
			}

			map[0, width / 2] = 2;
		}
		
		
	}
}