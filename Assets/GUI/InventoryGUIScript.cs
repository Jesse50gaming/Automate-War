using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Items;

public class InventoryUIScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gridContainer; // Assign the Grid child here
    [SerializeField] private Texture2D itemAtlas;
    [SerializeField] public Texture2D texture;
    [SerializeField] public InventoryScript inventoryScript;

    [Header("Layout")]
    [SerializeField] private int slotWidth = 16;
    [SerializeField] private int slotHeight = 16;
    [SerializeField] private int padding = 1;
    [SerializeField] private int UIscale = 5;

    private Image inventoryImage;
    private Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();

    void Start()
    {
        // inventory background
        inventoryImage = GetComponent<Image>();
        inventoryImage.sprite = Texture2DToSprite(texture);

        // IMPORTANT: layout-safe positioning only
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = inventoryScript.inventorySize;
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);

        rect.anchoredPosition = new Vector2(0, InventoryScript.distanceFromFloor);

        // Apply global scale (THIS replaces all per-slot scaling)
        rect.localScale = new Vector3(UIscale, UIscale, 1);


        SetupGrid();
        CreateSlots();
    }

    // ================= GRID SETUP =================
    private void SetupGrid()
    {
        GridLayoutGroup grid = gridContainer.GetComponent<GridLayoutGroup>();

        
        if (grid != null)
        {   
            
            grid.cellSize = new Vector2(slotWidth, slotHeight);
            grid.spacing = new Vector2(padding, padding);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = InventoryScript.columns;
        }
    }

    // ================= CREATE SLOTS =================
    private void CreateSlots()
    {
        int rows = InventoryScript.rows;
        int columns = InventoryScript.columns;

        for (int i = 0; i < rows * columns; i++)
        {
            GameObject slot = new GameObject("Slot_" + i);
            slot.transform.SetParent(gridContainer, false);

            RectTransform rect = slot.AddComponent<RectTransform>();
            Image image = slot.AddComponent<Image>();

            image.color = new Color32(0, 0, 0, 0); // transparent

            rect.sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    // ================= DRAW ITEM =================
    public void DrawItem(Item item, int slotIndex)
    {
        Transform slot = gridContainer.Find("Slot_" + slotIndex);
        if (slot == null) return;

        // Get texture from atlas
        Vector2Int coords = item.getAtlasCoords();
        Texture2D tex = ItemAtlasCoords.getItemTexture(coords, itemAtlas);

        // ================= ITEM ICON =================
        Transform itemTransform = slot.Find("Item");

        if (itemTransform == null)
        {
            GameObject itemObj = new GameObject("Item");
            itemObj.transform.SetParent(slot, false);

            RectTransform rect = itemObj.AddComponent<RectTransform>();
            Image img = itemObj.AddComponent<Image>();

            rect.sizeDelta = new Vector2(slotWidth, slotHeight);
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;

            img.sprite = Texture2DToSprite(tex);
        }
        else
        {
            itemTransform.GetComponent<Image>().sprite = Texture2DToSprite(tex);
        }

        // ================= COUNT TEXT =================
        Transform textTransform = slot.Find("CountText");

        if (textTransform == null)
        {
            GameObject textObj = new GameObject("CountText");
            textObj.transform.SetParent(slot, false);

            RectTransform rect = textObj.AddComponent<RectTransform>();
            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();

            rect.anchorMin = new Vector2(1, 0);
            rect.anchorMax = new Vector2(1, 0);
            rect.pivot = new Vector2(1, 0);
            rect.anchoredPosition = new Vector2(-1, 1);
            rect.sizeDelta = new Vector2(30, 20);

            text.fontSize = 18;
            text.alignment = TextAlignmentOptions.BottomRight;
            text.color = Color.white;

            // optional outline
            var outline = textObj.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, -1);

            text.text = item.getCount() > 1 ? item.getCount().ToString() : "";
        }
        else
        {
            TextMeshProUGUI text = textTransform.GetComponent<TextMeshProUGUI>();
            text.text = item.getCount() > 1 ? item.getCount().ToString() : "";
        }
    }

    // ================= SPRITE CACHE =================
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
}