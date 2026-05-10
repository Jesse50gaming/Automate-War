using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private int fontSize = 7;

    public bool isOpen = false;

    private Image inventoryImage;
    private RectTransform gridRect;
    private Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();

    private Item pickedItem;
    private bool pickedFromHotbar;
    private int pickedRow = -1;
    private int pickedCol = -1;
    private int pickedHotbarSlot = -1;

    private Canvas uiCanvas;
    private GameObject pickedItemIcon;
    private Image pickedItemIconImage;
    private TextMeshProUGUI pickedItemCountText;

    private float toggleCooldown = 0.2f; // seconds
    private float lastToggleTime = 0f;

    void Start()
    {
        // inventory background
        inventoryImage = GetComponent<Image>();
        inventoryImage.sprite = Texture2DToSprite(texture);
        inventoryImage.raycastTarget = false;

        Graphic gridGraphic = gridContainer.GetComponent<Graphic>();
        if (gridGraphic != null)
        {
            gridGraphic.raycastTarget = false;
        }

        gridRect = gridContainer.GetComponent<RectTransform>();
        DisableGridLayoutGroup();
        ConfigureGridContainer();

        uiCanvas = GetComponentInParent<Canvas>();
        if (uiCanvas == null)
        {
            uiCanvas = FindFirstObjectByType<Canvas>();
        }

        CreatePickedItemIcon();

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

    private void ConfigureGridContainer()
    {
        if (gridRect == null) return;

        float width = (InventoryScript.columns * slotWidth) + ((InventoryScript.columns - 1) * padding);
        float height = (InventoryScript.rows * slotHeight) + ((InventoryScript.rows - 1) * padding);

        gridRect.anchorMin = new Vector2(0.5f, 1f);
        gridRect.anchorMax = new Vector2(0.5f, 1f);
        gridRect.pivot = new Vector2(0.5f, 1f);
        gridRect.anchoredPosition = new Vector2(0, -padding);
        gridRect.sizeDelta = new Vector2(width, height);
    }

    // ================= GRID SETUP =================
    private void DisableGridLayoutGroup()
    {
        GridLayoutGroup grid = gridContainer.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.enabled = false;
        }
    }

    private void SetupGrid()
    {
        // No automatic layout for slots. Manual positioning is used to match exact Minecraft-style grid behavior.
        if (gridRect != null)
        {
            gridRect.localScale = Vector3.one;
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
            InventorySlot slotScript = slot.AddComponent<InventorySlot>();

            int row = i / columns;
            int col = i % columns;

            slotScript.inventoryGUI = this;
            slotScript.row = row;
            slotScript.col = col;

            image.color = new Color32(0, 0, 0, 0); // transparent
            image.raycastTarget = true;

            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector2(col * (slotWidth + padding), -row * (slotHeight + padding));
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
            img.raycastTarget = false;
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
            countText.raycastTarget = false;

            // Position bottom-right
            textRect.anchorMin = new Vector2(1, 0);
            textRect.anchorMax = new Vector2(1, 0);
            textRect.pivot = new Vector2(1, 0);
            textRect.anchoredPosition = new Vector2(0, 0);
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

    // ================= SLOT CLICK HANDLING =================
    public void OnInventorySlotClicked(int row, int col)
    {
        if (!isOpen) return;

        if (pickedItem == null)
        {
            PickUpFromInventory(row, col);
        }
        else
        {
            PlaceIntoInventory(row, col);
        }
    }

    public void OnHotbarSlotClicked(int slot)
    {
        if (!isOpen) return;

        if (pickedItem == null)
        {
            PickUpFromHotbar(slot);
        }
        else
        {
            PlaceIntoHotbar(slot);
        }
    }

    private void PickUpFromInventory(int row, int col)
    {
        Item item = inventoryScript.inventory.getItem(row, col);
        if (item == null) return;

        inventoryScript.inventory.removeItem(row, col);
        ClearSlot(row, col);

        pickedItem = item;
        pickedFromHotbar = false;
        pickedRow = row;
        pickedCol = col;
        pickedHotbarSlot = -1;

        UpdatePickedItemIcon();
    }

    private void PlaceIntoInventory(int row, int col)
    {
        if (pickedItem == null) return;

        Item destinationItem = inventoryScript.inventory.getItem(row, col);
        if (!pickedFromHotbar && row == pickedRow && col == pickedCol)
        {
            inventoryScript.inventory.putItem(row, col, pickedItem);
            DrawItem(pickedItem, row, col);
            ClearPickedItem();
            return;
        }

        Item previous = inventoryScript.inventory.putItem(row, col, pickedItem);
        DrawItem(pickedItem, row, col);

        pickedItem = previous;
        pickedFromHotbar = false;
        pickedRow = row;
        pickedCol = col;
        pickedHotbarSlot = -1;

        if (previous == null)
        {
            ClearPickedItem();
        }
        else
        {
            UpdatePickedItemIcon();
        }
    }

    private void PickUpFromHotbar(int slot)
    {
        Item item = inventoryScript.hotbar.getItem(slot);
        if (item == null) return;

        pickedItem = inventoryScript.hotbar.takeItem(slot);
        pickedFromHotbar = true;
        pickedRow = -1;
        pickedCol = -1;
        pickedHotbarSlot = slot;

        UpdatePickedItemIcon();
    }

    private void PlaceIntoHotbar(int slot)
    {
        if (pickedItem == null) return;

        Item existing = inventoryScript.hotbar.getItem(slot);
        if (pickedFromHotbar && slot == pickedHotbarSlot)
        {
            inventoryScript.hotbar.addItem(slot, pickedItem);
            ClearPickedItem();
            return;
        }

        inventoryScript.hotbar.addItem(slot, pickedItem);
        pickedItem = existing;
        pickedFromHotbar = true;
        pickedHotbarSlot = slot;
        pickedRow = -1;
        pickedCol = -1;

        if (existing == null)
        {
            ClearPickedItem();
        }
        else
        {
            UpdatePickedItemIcon();
        }
    }

    private void ClearPickedItem()
    {
        pickedItem = null;
        pickedFromHotbar = false;
        pickedRow = -1;
        pickedCol = -1;
        pickedHotbarSlot = -1;
    }

    private void ClearSlot(int row, int col)
    {
        int slotIndex = inventoryScript.inventory.columns * row + col;
        Transform slot = gridContainer.Find("Slot_" + slotIndex);
        if (slot == null) return;

        Transform itemTransform = slot.Find("Item");
        if (itemTransform != null)
            Destroy(itemTransform.gameObject);

        Transform textTransform = slot.Find("CountText");
        if (textTransform != null)
            Destroy(textTransform.gameObject);
    }

    private void CreatePickedItemIcon()
    {
        if (uiCanvas == null) return;

        pickedItemIcon = new GameObject("PickedItemIcon");
        pickedItemIcon.transform.SetParent(uiCanvas.transform, false);

        RectTransform rect = pickedItemIcon.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(slotWidth * 2, slotHeight * 2);

        pickedItemIconImage = pickedItemIcon.AddComponent<Image>();
        pickedItemIconImage.raycastTarget = false;
        pickedItemIcon.SetActive(false);

        GameObject textObj = new GameObject("PickedCountText");
        textObj.transform.SetParent(pickedItemIcon.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(1, 0);
        textRect.anchorMax = new Vector2(1, 0);
        textRect.pivot = new Vector2(1, 0);
        textRect.anchoredPosition = new Vector2(0, 0);
        textRect.sizeDelta = new Vector2(30, 20);

        pickedItemCountText = textObj.AddComponent<TextMeshProUGUI>();
        pickedItemCountText.fontSize = fontSize;
        pickedItemCountText.alignment = TextAlignmentOptions.BottomRight;
        pickedItemCountText.color = Color.white;
        pickedItemCountText.raycastTarget = false;
    }

    private void UpdatePickedItemIcon()
    {
        if (pickedItem == null)
        {
            if (pickedItemIcon != null && pickedItemIcon.activeSelf)
                pickedItemIcon.SetActive(false);
            return;
        }

        if (pickedItemIcon == null)
        {
            CreatePickedItemIcon();
            if (pickedItemIcon == null) return;
        }

        pickedItemIcon.SetActive(true);
        if (pickedItemIconImage != null)
        {
            Vector2Int coords = pickedItem.getAtlasCoords();
            Texture2D tex = ItemAtlasCoords.getItemTexture(coords, itemAtlas);
            pickedItemIconImage.sprite = Texture2DToSprite(tex);
            pickedItemIconImage.SetNativeSize();
            pickedItemIconImage.rectTransform.sizeDelta = new Vector2(slotWidth * UIscale, slotHeight * UIscale);
        }

        if (pickedItemCountText != null)
        {
            pickedItemCountText.text = pickedItem.getCount() > 1 ? pickedItem.getCount().ToString() : string.Empty;
        }

        if (uiCanvas != null)
        {
            RectTransform canvasRect = uiCanvas.transform as RectTransform;
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, uiCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : uiCanvas.worldCamera, out pos);
            pickedItemIcon.GetComponent<RectTransform>().anchoredPosition = pos;
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

    public void Update()
    {
        checkToggle();
        UpdatePickedItemIcon();
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