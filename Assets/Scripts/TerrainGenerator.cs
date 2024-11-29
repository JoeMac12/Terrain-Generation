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
}
