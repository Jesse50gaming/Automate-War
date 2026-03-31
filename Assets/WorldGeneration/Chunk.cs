using UnityEngine;

public class Chunk
{
    public const int Size = 16;

    private BlockType[,,] blocks = new BlockType[Size, Size, Size];
    private Vector3 position;

    public Chunk(Vector3 position)
    {
        this.position = position;
        GenerateBlocks();
    }

    private void GenerateBlocks()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int z = 0; z < Size; z++)
            {
                int height = Mathf.FloorToInt(Mathf.PerlinNoise(
                    (x + position.x) * 0.05f,
                    (z + position.z) * 0.05f) * Size);

                for (int y = 0; y < Size; y++)
                {
                    if (y < height)
                        blocks[x, y, z] = BlockType.DIRT;
                    else
                        blocks[x, y, z] = BlockType.AIR;
                }
            }
        }
    }

    public BlockType GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }
}

