
using UnityEngine;

namespace Items
{
    public static class ItemAtlasCoords
    {
        public static readonly Vector2Int DIRT = new Vector2Int(0, 0);
        public static readonly Vector2Int GRASS = new Vector2Int(0, 1);
        public static readonly Vector2Int STONE = new Vector2Int(1, 0);


       
        private const int tilePixelSize = 16;    // single tile size

        public static Texture2D getItemTexture(Vector2Int coords, Texture2D atlas) {
            int x = coords.x * tilePixelSize;
            int y = coords.y * tilePixelSize;
            
            Color[] pixels = atlas.GetPixels(x, y, tilePixelSize, tilePixelSize);
            
            Texture2D itemTexture = new Texture2D(tilePixelSize, tilePixelSize, TextureFormat.RGBA32, false);
            itemTexture.SetPixels(pixels);
            itemTexture.Apply();
            
            return itemTexture;
        }
    }
}