using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragUsableSlot : MonoBehaviour, IDropHandler
{
    public int hotbarKey; //Input.GetKeyDown();

    public void OnDrop(PointerEventData eventData) //if item is dropped on this box
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<ItemDraggable>().objectDroppedCorrectly = true;
            eventData.pointerDrag.GetComponent<ItemDraggable>().hotbarLocation = hotbarKey;

            eventData.pointerDrag.GetComponent<ItemDraggable>().objectDroppedInHotbar = true;
        }
    }
}
