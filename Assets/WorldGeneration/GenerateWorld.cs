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
        mat = GetComponent<MeshRenderer>().material;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateTerrain();
        GradientToTexture();
        ApplyMaterialSettings();
    }

    void Update()
    {
        
        // Only update if values change (optional optimization)
        ApplyMaterialSettings();
    }

    private void ApplyMaterialSettings()
    {
        float minTerrainHeight = mesh.bounds.min.y + transform.position.y - 0.1f;
        float maxTerrainHeight = mesh.bounds.max.y + transform.position.y + 0.1f;

        mat.SetTexture("_TerrainGradient", gradientTexture);
        mat.SetFloat("_MinTerrainHeight", minTerrainHeight);
        mat.SetFloat("_MaxTerrainHeight", maxTerrainHeight);
    }

    private void GradientToTexture()
    {
        gradientTexture = new Texture2D(1, 100);
        gradientTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixelColors = new Color[100];

        for (int i = 0; i < 100; i++)
        {
            pixelColors[i] = terrainGradient.Evaluate(i / 99f);
        }

        gradientTexture.SetPixels(pixelColors);
        gradientTexture.Apply();
    }

    private void GenerateTerrain()
    {
        Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float yPos = Mathf.PerlinNoise(
                    (x + xOffset) * noiseScale,
                    (z + zOffset) * noiseScale
                ) * heightMultiplier;

                vertices[i++] = new Vector3(x, yPos, z);
            }
        }

        int[] triangles = new int[xSize * zSize * 6];

        int vertex = 0;
        int triIndex = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[triIndex++] = vertex;
                triangles[triIndex++] = vertex + xSize + 1;
                triangles[triIndex++] = vertex + 1;

                triangles[triIndex++] = vertex + 1;
                triangles[triIndex++] = vertex + xSize + 1;
                triangles[triIndex++] = vertex + xSize + 2;

                vertex++;
            }
            vertex++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}