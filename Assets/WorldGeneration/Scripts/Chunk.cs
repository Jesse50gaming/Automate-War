using UnityEngine;

public class Chunk
{
    public const int chunkWidth = 16;
    public const int chunkLength = 16;
    public const int chunkHeight = 128;

    public const float noiseMult = 0.005f;

    private BlockType[,,] blocks = new BlockType[chunkWidth, chunkHeight, chunkLength];
    private Vector3 position;

    public Chunk(Vector3 position)
    {
        this.position = position;
        GenerateBlocks();
    }

    private void GenerateBlocks()
    {
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int z = 0; z < chunkLength; z++)
            {
                int height = Mathf.FloorToInt(Mathf.PerlinNoise(
                    (x + position.x) * noiseMult,
                    (z + position.z) * noiseMult) * chunkHeight);

                for (int y = 0; y < chunkHeight; y++)
                {
                    if (y == height)
                    {
                        blocks[x, y, z] = BlockType.GRASS; 
                    } else if (y < height)
                    {
                       blocks[x, y, z] = BlockType.DIRT; 
                    } else
                    {
                        blocks[x, y, z] = BlockType.AIR;
                    }   
                        
                }
            }
        }
    }

    public BlockType GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }
}

