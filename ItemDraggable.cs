using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Canvas myCanvas;

    private InventoryManager theInventoryManager;
    private levelManager theLevelManager;

    private Vector3 originalPosition;

    public bool objectSwitched = false;
    public bool objectDroppedCorrectly = false;
    public bool objectDroppedInHotbar = false;

    public int hotbarLocation;

    public bool levelPaused;

    public bool isInHotbar = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        theInventoryManager = FindObjectOfType<InventoryManager>();
        theLevelManager = FindObjectOfType<levelManager>();
        myCanvas = GetComponent<Canvas>();

        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        myCanvas.sortingOrder = 20; //make object appear in front of everything else 
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        theLevelManager.Select2.Play();
        canvasGroup.alpha = 1f;
        myCanvas.sortingOrder = 10; // put it back to normal
        canvasGroup.blocksRaycasts = true;
        if (!objectSwitched && !objectDroppedCorrectly)
        {
            rectTransform.anchoredPosition = originalPosition;
            objectSwitched = false;
            objectDroppedCorrectly = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        theLevelManager.Select2.Play();
        //Debug.Log("cum");
        theInventoryManager.selectedItemPlace = int.Parse(gameObject.name);
        theInventoryManager.RefreshPopupBar();
    }

    public void OnDrop(PointerEventData eventData) // something dropped on this object
    {
        theLevelManager.Select2.Play();
        objectSwitched = true;
        //swtich items
        int i = int.Parse(gameObject.name);
        int p = int.Parse(eventData.pointerDrag.gameObject.name);
        if (i < 0 || p < 0)
        {
            theInventoryManager.SwapBetweenHotbar(i, p);
        }
        else
        {
            theInventoryManager.SwapInventoryItems(i, p);
        }
    }

    public void Update()
    {
        if (theInventoryManager.refreshPositionItem)
        {
            theInventoryManager.refreshPositionItem = false;
            originalPosition = rectTransform.anchoredPosition;
        }
        if (levelPaused)
        {
            rectTransform.anchoredPosition = Vector3.zero;
            theInventoryManager.refreshPositionItem = true;
        }
        if (!isInHotbar)
        {
            if (objectDroppedInHotbar)
            {
                int i = int.Parse(gameObject.name);
                if (theInventoryManager.hotbarArray[hotbarLocation - 1] == null)
                {
                    theInventoryManager.ItemToHotbar(i, hotbarLocation);
                }
                isInHotbar = true;
            }
        }
        else
        {
            if (objectDroppedInHotbar)
            {
                int i = int.Parse(gameObject.name);
                if (theInventoryManager.hotbarArray[hotbarLocation - 1] == null)
                {
                    theInventoryManager.HotbarItemToHotbar(i, hotbarLocation);
                }
                isInHotbar = true;
            }
        }
    }

}
