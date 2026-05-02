using UnityEngine;

public static class BlockDatabase
{
    
    public static Vector2Int GetTexture(BlockType type, Vector3 dir) // (0,0) is bottom left
    {
        if (type == BlockType.GRASS)
        {
            if (dir == Vector3.up) return new Vector2Int(0, 2);   //  top
            if (dir == Vector3.down) return new Vector2Int(0, 0); // bottom
            return new Vector2Int(0, 1); //side
        }

        if (type == BlockType.DIRT)
        {
            return new Vector2Int(0, 0);
        }

        if (type == BlockType.STONE)
        {
            return new Vector2Int(1,0);
        }
        
        return new Vector2Int(0, 0);
    }
}