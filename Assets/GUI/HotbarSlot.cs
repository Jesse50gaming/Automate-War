using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryGUIScript inventoryGUI;
    public int slot;

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
            inventoryGUI.OnHotbarSlotClicked(slot);
        }
    }
}
