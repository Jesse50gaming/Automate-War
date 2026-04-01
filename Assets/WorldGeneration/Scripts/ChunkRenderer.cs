using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour
{
    public int size = 16;

    public int xOffset;
    public int zOffset;

    public float noiseScale = 0.03f;
    public float heightMultiplier = 7;

    private Mesh mesh;

    public void Init(int xOffset, int zOffset)
    {
        this.xOffset = xOffset;
        this.zOffset = zOffset;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateChunk();
    }

    private void GenerateChunk()
    {
        Vector3[] vertices = new Vector3[(size + 1) * (size + 1)];

        int i = 0;
        for (int z = 0; z <= size; z++)
        {
            for (int x = 0; x <= size; x++)
            {
                float y = Mathf.PerlinNoise(
                    (x + xOffset) * noiseScale,
                    (z + zOffset) * noiseScale
                ) * heightMultiplier;

                vertices[i++] = new Vector3(x, y, z);
            }
        }

        int[] triangles = new int[size * size * 6];

        int vert = 0;
        int tri = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                triangles[tri++] = vert;
                triangles[tri++] = vert + size + 1;
                triangles[tri++] = vert + 1;

                triangles[tri++] = vert + 1;
                triangles[tri++] = vert + size + 1;
                triangles[tri++] = vert + size + 2;

                vert++;
            }
            vert++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}