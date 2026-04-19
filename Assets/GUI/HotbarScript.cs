
using Container;
using Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HotbarScript : MonoBehaviour
{
    [SerializeField] public Texture2D texture;
    [SerializeField] private Texture2D ItemAtlas;
    private Image hotbarImage;

    private Vector2 pos;

    [SerializeField] private InventoryScript inventoryScript;
    private Hotbar hotbar;

    void Start()
    {
        if (inventoryScript == null)
        {
            inventoryScript = FindObjectOfType<InventoryScript>();
        }

        if (inventoryScript != null)
        {
            hotbar = inventoryScript.hotbar;
        }
        

        hotbarImage = GetComponent<Image>();
        pos = GetComponent<RectTransform>().position;
        hotbarImage.sprite = Texture2DToSprite(texture);
        
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(100, 100);
        rect.position = pos;
        updateHotbar();
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }

    public void updateHotbar()
    {
        for (int i = 0; i < Hotbar.hotbarWidth; i++)
        {
            if (hotbar.hasItem(i)) {
                drawItem(hotbar.getItem(i), i);
            }
        }
    }

    private void drawItem(Item item, int slot) {
        Vector2Int atlasCoord = item.getAtlasCoords();
        Texture2D itemTexture = ItemAtlasCoords.getItemTexture(atlasCoord, ItemAtlas);
        
        // Find or create a slot image
        Transform slotTransform = transform.Find("Slot_" + slot);
        Image slotImage;
        
        if (slotTransform == null) {
            GameObject slotObject = new GameObject("Slot_" + slot);
            slotObject.transform.SetParent(transform);
            slotImage = slotObject.AddComponent<Image>();
        } else {
            slotImage = slotTransform.GetComponent<Image>();
        }
        
        slotImage.sprite = Texture2DToSprite(itemTexture);
    }
}