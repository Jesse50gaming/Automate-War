using UnityEngine;

public static class AtlasUV
{
    private const int atlasPixelSize = 256;  // full atlas size
    private const int tilePixelSize = 16;    // single tile size
    private const int atlasSize = atlasPixelSize / tilePixelSize; // 16 tiles per row/column
    private const float tileUVSize = 1f / atlasSize; // 0.0625

    // x = column, y = row (bottom-left = 0,0)
    public static Vector2[] GetUVs(int x, int y)
    {
        float u = x * tileUVSize;
        float v = y * tileUVSize;

        // match vertex order in AddFace triangles
        return new Vector2[]
        {
            new Vector2(u, v),                     // vertexIndex + 0
            new Vector2(u + tileUVSize, v),        // vertexIndex + 1
            new Vector2(u, v + tileUVSize),        // vertexIndex + 2
            new Vector2(u + tileUVSize, v + tileUVSize) // vertexIndex + 3
        };
    }


    public static Vector2[] GetRotatedUVs(int x, int y)
    {
        
        float u = x * tileUVSize;
        float v = y * tileUVSize;

        // Rotate 90° clockwise
        return new Vector2[]
        {
            new Vector2(u + tileUVSize, v),         // bottom-left
            new Vector2(u + tileUVSize, v + tileUVSize), // top-left
            new Vector2(u, v),                       // bottom-right
            new Vector2(u, v + tileUVSize)           // top-right
        };
    }
}