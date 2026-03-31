using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] int xSize = 10;
    [SerializeField] int zSize = 10;

    [SerializeField] int xOffset;
    [SerializeField] int zOffset;

    [SerializeField] float noiseScale = 0.03f;
    [SerializeField] float heightMultiplier = 7;

    [SerializeField] Gradient terrainGradient;
    private Material mat;

    private Mesh mesh;
    private Texture2D gradientTexture;

    void Start()
    {
        
    }

    void Update()
    {

    }

}