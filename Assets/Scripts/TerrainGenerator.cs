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

	public Material terrainMaterial;

	void Start()
	{
		GenerateTerrain();
	}

	// Make terrain
	public void GenerateTerrain()
	{
		float[,] heightMap = GenerateHeightMap();

		GenerateTerrainMesh(heightMap);

		GetComponent<MeshRenderer>().material = terrainMaterial;
	}

	// Clear mesh data and make new terrain
	public void RegenerateTerrain()
	{
		mesh.Clear();
		GenerateTerrain();
	}

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

	// Generate perlin noise with octaves
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

	// Generate terrain mesh from height map
	private void GenerateTerrainMesh(float[,] heightMap)
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		vertices = new Vector3[(width + 1) * (height + 1)];
		triangles = new int[width * height * 6];

		int vertIndex = 0;
		int triIndex = 0;

		// Create vertices
		for (int z = 0; z <= height; z++)
		{
			for (int x = 0; x <= width; x++)
			{
				float y = heightMap[x % width, z % height] * heightMultiplier;
				vertices[vertIndex] = new Vector3(x, y, z);
				vertIndex++;
			}
		}

		// Reset before triangles to fix?
		vertIndex = 0;

		// Create triangles
		for (int z = 0; z < height; z++)
		{
			for (int x = 0; x < width; x++)
			{
				int a = vertIndex;
				int b = vertIndex + width + 1;
				int c = vertIndex + 1;
				int d = vertIndex + width + 2;

				triangles[triIndex + 0] = a;
				triangles[triIndex + 1] = b;
				triangles[triIndex + 2] = c;

				triangles[triIndex + 3] = c;
				triangles[triIndex + 4] = b;
				triangles[triIndex + 5] = d;

				vertIndex++;
				triIndex += 6;
			}
			vertIndex++;
		}

		UpdateMesh();
	}

	// Update vertices and triangles after generation (It should fix lighting issue I think?)
	private void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}
