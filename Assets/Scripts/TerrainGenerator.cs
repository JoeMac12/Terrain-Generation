using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	// Terrain settings
	public int width = 100;
	public int height = 100;
	public float heightMultiplier = 5f;

	// Noise settings
	public float scale = 20f;
	public int octaves = 4;
	public float persistence = 0.5f;
	public float lacunarity = 2f;

	// Randomization settings
	public float offsetX = 100f;
	public float offsetY = 100f;

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;

	private float[,] GenerateHeightMap()
	{
		// Create height map array
		float[,] heightMap = new float[width, height];

		// Make different terrains
		offsetX = Random.Range(0f, 99999f);
		offsetY = Random.Range(0f, 99999f);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float sampleX = (x + offsetX) / scale;
				float sampleY = (y + offsetY) / scale;

				float noiseHeight = GeneratePerlinNoise(sampleX, sampleY);
				heightMap[x, y] = noiseHeight;
			}
		}

		return heightMap;
	}

	// Still testing atm
	private float GeneratePerlinNoise(float x, float y)
	{
		float total = 0f;
		float frequency = 1f;
		float amplitude = 1f;
		float maxValue = 0f;

		for (int i = 0; i < octaves; i++)
		{
			float perlinValue = Mathf.PerlinNoise(x * frequency, y * frequency) * 2 - 1;
			total += perlinValue * amplitude;

			maxValue += amplitude;

			amplitude *= persistence;
			frequency *= lacunarity;
		}

		return total / maxValue;
	}
}
