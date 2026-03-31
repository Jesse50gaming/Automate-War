using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkMesh : MonoBehaviour
{
    private Chunk chunk;

    private List<Vector3> vertices = new();
    private List<int> triangles = new();
    private List<Vector2> uvs = new();

    private int vertexIndex = 0;

    public void Init(Chunk chunk)
    {
        this.chunk = chunk;
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        for (int x = 0; x < Chunk.chunkWidth; x++)
        {
            for (int y = 0; y < Chunk.chunkHeight; y++)
            {
                for (int z = 0; z < Chunk.chunkLength; z++)
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

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
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

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));

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