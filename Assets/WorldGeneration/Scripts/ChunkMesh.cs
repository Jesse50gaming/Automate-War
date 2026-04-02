using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkMesh : MonoBehaviour
{
    private Chunk chunk;

    private List<Vector3> vertices = new();
    private List<int> triangles = new();
    private List<Vector2> uvs = new();

    private int vertexIndex = 0;

    [SerializeField] private Gradient terrainGradient;

    private Mesh mesh;
    private MeshCollider meshCollider;
    private Texture2D gradientTexture;

    public void Init(Chunk chunk)
    {
        this.chunk = chunk;

        mesh = new Mesh();
        meshCollider = GetComponent<MeshCollider>();

        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
        GradientToTexture();
        ApplyTexture();
        UpdateCollider();
    }

    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        vertexIndex = 0;

        for (int x = 0; x < Chunk.chunkLength; x++)
        {
            for (int y = 0; y < Chunk.chunkHeight; y++)
            {
                for (int z = 0; z < Chunk.chunkWidth; z++)
                {
                    BlockType block = chunk.GetBlock(x, y, z);
                    if (block == BlockType.AIR) continue;

                    TryAddFace(x, y, z, Vector3.forward);
                    TryAddFace(x, y, z, Vector3.back);
                    TryAddFace(x, y, z, Vector3.left);
                    TryAddFace(x, y, z, Vector3.right);
                    TryAddFace(x, y, z, Vector3.up);
                    TryAddFace(x, y, z, Vector3.down);
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private void ApplyTexture()
    {
        var mat = GetComponent<MeshRenderer>().material;
        mat.mainTexture = gradientTexture;
    }

    private void UpdateCollider()
    {
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
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

    private void TryAddFace(int x, int y, int z, Vector3 dir)
    {
        int nx = x + (int)dir.x;
        int ny = y + (int)dir.y;
        int nz = z + (int)dir.z;

        if (IsInside(nx, ny, nz) && chunk.GetBlock(nx, ny, nz) != BlockType.AIR)
            return;

        AddFace(new Vector3(x, y, z), dir);
    }

    private bool IsInside(int x, int y, int z)
    {
        return x >= 0 && x < Chunk.chunkWidth &&
               y >= 0 && y < Chunk.chunkHeight &&
               z >= 0 && z < Chunk.chunkLength;
    }

    private void AddFace(Vector3 pos, Vector3 dir)
    {
        Vector3[] faceVertices = GetFaceVertices(pos, dir);
        vertices.AddRange(faceVertices);

        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);

        float height = pos.y / Chunk.chunkHeight;

        uvs.Add(new Vector2(0, height));
        uvs.Add(new Vector2(1, height));
        uvs.Add(new Vector2(0, height));
        uvs.Add(new Vector2(1, height));

        vertexIndex += 4;
    }

    private Vector3[] GetFaceVertices(Vector3 pos, Vector3 dir)
    {
        if (dir == Vector3.up)
            return new[] { pos + new Vector3(0,1,0), pos + new Vector3(1,1,0), pos + new Vector3(0,1,1), pos + new Vector3(1,1,1) };

        if (dir == Vector3.down)
            return new[] { pos, pos + new Vector3(1,0,0), pos + new Vector3(0,0,1), pos + new Vector3(1,0,1) };

        if (dir == Vector3.forward)
            return new[] { pos + new Vector3(0,0,1), pos + new Vector3(1,0,1), pos + new Vector3(0,1,1), pos + new Vector3(1,1,1) };

        if (dir == Vector3.back)
            return new[] { pos, pos + new Vector3(1,0,0), pos + new Vector3(0,1,0), pos + new Vector3(1,1,0) };

        if (dir == Vector3.left)
            return new[] { pos, pos + new Vector3(0,0,1), pos + new Vector3(0,1,0), pos + new Vector3(0,1,1) };

        return new[] { pos + new Vector3(1,0,0), pos + new Vector3(1,0,1), pos + new Vector3(1,1,0), pos + new Vector3(1,1,1) };
    }
}