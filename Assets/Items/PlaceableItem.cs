using UnityEngine;

public abstract class PlaceableItem : Item
{
    protected BlockType block;

    public PlaceableItem(BlockType block, Vector2Int atlasCoord) : base(atlasCoord)
    {
        this.block = block;
    }

    public BlockType GetBlock()
    {
        return block;
    }
}