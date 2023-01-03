using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("finally" + gameObject.name);
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            //ItemDraggable[] DraggableItems = FindObjectsOfType<ItemDraggable>();
            //foreach(ItemDraggable i in DraggableItems)
            //{
            //i.objectDroppedCorrectly = true;
            //}
            eventData.pointerDrag.GetComponent<ItemDraggable>().objectDroppedCorrectly = true;
        }
    }
    //when you add canvas to an object for sorting, also add graphic raycaster for continued clicking abilities
}
