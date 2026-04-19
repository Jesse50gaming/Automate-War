using System.Collections.Generic;
using Container;
using Items;
using UnityEngine;
using UnityEngine.UI;

public class HotbarScript : MonoBehaviour
{
    [SerializeField] public Texture2D texture;
    [SerializeField] private Texture2D ItemAtlas;

    private Image hotbarImage;

    [SerializeField] private InventoryScript inventoryScript;
    private Hotbar hotbar;

    // Global UI scale (apply to whole hotbar instead of per-slot math)
    [SerializeField] private int UIscale = 5;

    // Sprite cache (prevents recreating sprites every update)
    private Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();

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

        // Hotbar background
        hotbarImage = GetComponent<Image>();
        hotbarImage.sprite = Texture2DToSprite(texture);

        // IMPORTANT: layout-safe positioning only
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(171,18);
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);

        rect.anchoredPosition = new Vector2(0, Hotbar.gapFromBottomOfScreen);

        // Apply global scale (THIS replaces all per-slot scaling)
        rect.localScale = new Vector3(UIscale, UIscale, 1);

        updateHotbar();
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        if (texture == null) return null;

        if (spriteCache.TryGetValue(texture, out Sprite cached))
            return cached;

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        spriteCache[texture] = sprite;
        return sprite;
    }

    public void updateHotbar()
    {
        for (int i = 0; i < Hotbar.hotbarWidth; i++)
        {
            if (hotbar != null && hotbar.hasItem(i))
            {
                drawItem(hotbar.getItem(i), i);
            }
            else
            {
                clearSlot(i);
            }
        }
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(171,18);

    }

    private void clearSlot(int slot)
    {
        Transform slotTransform = transform.Find("Slot_" + slot);

        if (slotTransform != null)
        {
            Transform itemTransform = slotTransform.Find("Item");

            if (itemTransform != null)
            {
                Image itemImage = itemTransform.GetComponent<Image>();
                itemImage.sprite = null;
            }
        }
    }

    private void drawItem(Item item, int slot)
    {
        Vector2Int atlasCoord = item.getAtlasCoords();
        Texture2D itemTexture = ItemAtlasCoords.getItemTexture(atlasCoord, ItemAtlas);

        Transform slotTransform = transform.Find("Slot_" + slot);
        GameObject slotObject;

        if (slotTransform == null)
        {
            // ================= SLOT =================
            slotObject = new GameObject("Slot_" + slot);
            slotObject.transform.SetParent(transform, false);

            RectTransform slotRect = slotObject.AddComponent<RectTransform>();
            Image slotImage = slotObject.AddComponent<Image>();

            slotImage.color = new Color32(0, 0, 0, 0); // transparent slot background

            // Let Layout Group handle positioning → NO anchoredPosition
            slotRect.sizeDelta = new Vector2(Hotbar.slotWidth + (Hotbar.slotPadding * 2), Hotbar.slotWidth + (Hotbar.slotPadding * 2));

            // ================= ITEM =================
            GameObject itemObject = new GameObject("Item");
            itemObject.transform.SetParent(slotObject.transform, false);

            RectTransform itemRect = itemObject.AddComponent<RectTransform>();
            Image itemImage = itemObject.AddComponent<Image>();

            itemRect.sizeDelta = new Vector2(Hotbar.slotWidth, Hotbar.slotWidth);
            itemRect.anchorMin = itemRect.anchorMax = new Vector2(0.5f, 0.5f);
            itemRect.anchoredPosition = Vector2.zero;

            itemImage.sprite = Texture2DToSprite(itemTexture);
        }
        else
        {
            // ================= UPDATE ITEM =================
            Transform itemTransform = slotTransform.Find("Item");

            if (itemTransform != null)
            {
                Image itemImage = itemTransform.GetComponent<Image>();
                itemImage.sprite = Texture2DToSprite(itemTexture);
            }
            else
            {
                // recreate if missing
                GameObject itemObject = new GameObject("Item");
                itemObject.transform.SetParent(slotTransform, false);

                RectTransform itemRect = itemObject.AddComponent<RectTransform>();
                Image itemImage = itemObject.AddComponent<Image>();

                itemRect.sizeDelta = new Vector2(Hotbar.slotWidth, Hotbar.slotWidth);
                itemRect.anchorMin = itemRect.anchorMax = new Vector2(0.5f, 0.5f);
                itemRect.anchoredPosition = Vector2.zero;

                itemImage.sprite = Texture2DToSprite(itemTexture);
            }
        }
    }
}