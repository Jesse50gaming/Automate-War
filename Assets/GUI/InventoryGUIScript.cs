using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Items;
using Container;
using JetBrains.Annotations;

public class InventoryGUIScript : MonoBehaviour
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

    [SerializeField] private KeyCode inventoryKey = KeyCode.E;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private int fontSize = 6;

    public bool isOpen = false;

    private Image inventoryImage;
    private Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();

    private float toggleCooldown = 0.2f; // seconds
    private float lastToggleTime = 0f;

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

        // Apply global scale 
        rect.localScale = new Vector3(UIscale, UIscale, 1);


        SetupGrid();
        CreateSlots();
        CloseInventory();
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
            slot.AddComponent<guiItemScript>();

            image.color = new Color32(0, 0, 0, 0); // transparent

            rect.sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    // ================= DRAW ITEM =================
    public void DrawItem(Item item, int row, int col)
    {
        if (item == null) return;

        if (gridContainer.childCount == 0)
        {
            CreateSlots();
        }

        int slotIndex = inventoryScript.inventory.columns * row + col;
        print(slotIndex);
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

           RectTransform textRect = textObj.AddComponent<RectTransform>();
            TextMeshProUGUI countText = textObj.AddComponent<TextMeshProUGUI>();

            // Position bottom-right
            textRect.anchorMin = new Vector2(1, 0);
            textRect.anchorMax = new Vector2(1, 0);
            textRect.pivot = new Vector2(1, 0);
            textRect.anchoredPosition = new Vector2(0, -2);
            textRect.sizeDelta = new Vector2(12, 8);

            // Style
            countText.fontSize = fontSize;
            countText.alignment = TextAlignmentOptions.BottomRight;
            countText.color = Color.white;

            // Set text
            countText.text = item.getCount() > 1 ? item.getCount().ToString() : "";
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

    public void Update()
    {
        checkToggle();

    }

    private void checkToggle()
    {
        if (Input.GetKeyDown(inventoryKey) && Time.time - lastToggleTime > toggleCooldown)
        {
            
            isOpen = !isOpen;
            lastToggleTime = Time.time;
            if (isOpen) OpenInventory();
            else CloseInventory();
            
        }
    }

    public void OpenInventory()
    {
        inventoryImage.enabled = true;
        gridContainer.gameObject.SetActive(true);
        playerCamera.setMovement(false);
    }

    public void CloseInventory()
    {
        inventoryImage.enabled = false;
        gridContainer.gameObject.SetActive(false);
        playerCamera.setMovement(true);
    }
    
}