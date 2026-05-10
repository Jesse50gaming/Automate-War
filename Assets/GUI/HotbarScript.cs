using System.Collections.Generic;
using Container;
using Items;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HotbarScript : MonoBehaviour
{
    [SerializeField] public Texture2D texture;
    [SerializeField] private Texture2D ItemAtlas;
    
    private Image hotbarImage;
    private int fontSize = 8;

    [SerializeField] private InventoryScript inventoryScript;
    private InventoryGUIScript inventoryGUI;
    private Hotbar hotbar;
    private Canvas uiCanvas;

    // Global UI scale (apply to whole hotbar instead of per-slot math)
    [SerializeField] private int UIscale = 5;

    // Sprite cache (prevents recreating sprites every update)
    private Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();
    
    void Start()
    {
        if (inventoryScript == null)
        {
            inventoryScript = FindFirstObjectByType<InventoryScript>();
        }

        if (inventoryScript != null)
        {
            hotbar = inventoryScript.hotbar;
            inventoryGUI = FindFirstObjectByType<InventoryGUIScript>();
        }

        // Hotbar background
        hotbarImage = GetComponent<Image>();
        hotbarImage.sprite = Texture2DToSprite(texture);
        hotbarImage.raycastTarget = false;

        uiCanvas = GetComponentInParent<Canvas>();
        if (uiCanvas == null)
        {
            uiCanvas = FindFirstObjectByType<Canvas>();
        }

        DisableLayoutGroup();
        CreateSlots();

        // IMPORTANT: layout-safe positioning only
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = Hotbar.hotbarSize;
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);

        rect.anchoredPosition = new Vector2(0, Hotbar.gapFromBottomOfScreen);

        // Apply global scale (THIS replaces all per-slot scaling)
        rect.localScale = new Vector3(UIscale, UIscale, 1);

        updateHotbar();
    }

    private void CreateSlots()
    {
        for (int slot = 0; slot < Hotbar.hotbarWidth; slot++)
        {
            if (transform.Find("Slot_" + slot) != null)
                continue;

            GameObject slotObject = new GameObject("Slot_" + slot);
            slotObject.transform.SetParent(transform, false);

            HotbarSlot slotScript = slotObject.AddComponent<HotbarSlot>();
            slotScript.inventoryGUI = inventoryGUI;
            slotScript.slot = slot;

            RectTransform slotRect = slotObject.AddComponent<RectTransform>();
            Image slotImage = slotObject.AddComponent<Image>();

            slotImage.color = new Color32(0, 0, 0, 0);
            slotImage.raycastTarget = true;

            slotRect.anchorMin = new Vector2(0f, 1f);
            slotRect.anchorMax = new Vector2(0f, 1f);
            slotRect.pivot = new Vector2(0f, 1f);
            slotRect.localScale = Vector3.one;
            slotRect.anchoredPosition = new Vector2(slot * (Hotbar.slotWidth + Hotbar.slotPadding) + 0.5f, -0.5f);
            slotRect.sizeDelta = new Vector2(
                Hotbar.slotWidth + (Hotbar.slotPadding * 1),
                Hotbar.slotWidth + (Hotbar.slotPadding * 1)
            );
        }
    }

    private void DisableLayoutGroup()
    {
        LayoutGroup group = GetComponent<LayoutGroup>();
        if (group != null)
        {
            group.enabled = false;
        }
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
        rect.sizeDelta = Hotbar.hotbarSize;

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

            Transform textTransform = slotTransform.Find("CountText");
            if (textTransform != null)
            {
                TextMeshProUGUI countText = textTransform.GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = string.Empty;
                }
            }
        }
    }

    private void drawItem(Item item, int slot)
    {
        Vector2Int atlasCoord = item.getAtlasCoords();
        Texture2D itemTexture = ItemAtlasCoords.getItemTexture(atlasCoord, ItemAtlas);

        Transform slotTransform = transform.Find("Slot_" + slot);
        if (slotTransform == null)
        {
            return;
        }

        GameObject slotObject = slotTransform.gameObject;

        // ================= ITEM =================
        Transform itemTransform = slotTransform.Find("Item");
        if (itemTransform == null)
        {
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
            Image itemImage = itemTransform.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = Texture2DToSprite(itemTexture);
            }
        }

        // ================= COUNT TEXT =================
        Transform textTransform = slotTransform.Find("CountText");
        if (textTransform == null)
        {
            GameObject textObject = new GameObject("CountText");
            textObject.transform.SetParent(slotObject.transform, false);

            RectTransform textRect = textObject.AddComponent<RectTransform>();
            TextMeshProUGUI countText = textObject.AddComponent<TextMeshProUGUI>();

            // Position bottom-right
            textRect.anchorMin = new Vector2(1, 0);
            textRect.anchorMax = new Vector2(1, 0);
            textRect.pivot = new Vector2(1, 0);
            textRect.anchoredPosition = new Vector2(8, -9);
            textRect.sizeDelta = new Vector2(15, 10);

            // Style
            countText.fontSize = fontSize;
            countText.alignment = TextAlignmentOptions.BottomRight;
            countText.color = Color.white;
            countText.text = item.getCount() > 1 ? item.getCount().ToString() : "";
        }
        else
        {
            TextMeshProUGUI countText = textTransform.GetComponent<TextMeshProUGUI>();
            if (countText != null)
            {
                countText.text = item.getCount() > 1 ? item.getCount().ToString() : "";
            }
        }
    }
}