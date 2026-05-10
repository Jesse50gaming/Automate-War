using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryGUIScript inventoryGUI;
    public int row;
    public int col;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (rectTransform != null && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera))
        {
            return;
        }

        if (inventoryGUI == null)
        {
            inventoryGUI = FindFirstObjectByType<InventoryGUIScript>();
        }

        if (inventoryGUI != null)
        {
            inventoryGUI.OnInventorySlotClicked(row, col);
        }
    }
}
