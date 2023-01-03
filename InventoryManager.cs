using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{
    private levelManager theLevelManager;

    public List<InventoryItem> itemList;

    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public GameObject InventoryScreen;

    public float rowLength = 4;

    //public InventoryItem addThisnow;

    public float itemSlotCellSize;
    public float firstBoxPositionx;
    public float firstBoxPositiony;

    public float[] itemAmounts;
    [HideInInspector] public float[] uniqueValues;
    private float[] amountOfInventoryItemsSharing;//should be private
    private float[] amountToIncreaseBy; // should be private
    private float indexerUnique = 0;

    public bool refreshPositionItem;

    [Header("hotbarstuff ig")]
    public InventoryItem[] hotbarArray;
    public float[] hotbarAmounts;

    public Transform hotBarItemTemplate;

    [Header("popupbar")]

    public Image popupVisual;
    public TMP_Text popupDescription;
    public TMP_Text popupItemName;
    public InventoryItem selectedItem;
    public int selectedItemPlace;

    public Animator InventoryMenuAnimator;
    //public int amountOfStoredItems = 0; //obviously - 1

    void Awake()
    {
        //AddItem(new Itemv2 { itemType = Itemv2.ItemType.Ammo, amount = 1 });
        //AddItem(new Itemv2 { itemType = Itemv2.ItemType.Knife1, amount = 1 });
        //AddItem(new Itemv2 { itemType = Itemv2.ItemType.AK47, amount = 1 });

        theLevelManager = FindObjectOfType<levelManager>();

        if (itemSlotContainer == null || itemSlotTemplate == null)
        {
            itemSlotContainer = transform.Find("ItemSlotContainer");
            itemSlotTemplate = transform.Find("ItemSlotTemplate");
        }
        if (itemSlotContainer == null || itemSlotTemplate == null)
        {
            Debug.Log("Manually insert these two (container and template for iteminventory) cuz this shit sucks");
        }

        itemList = new List<InventoryItem>();

        for (var l = 0; l < PlayerPrefs.GetInt("amountOfStoredItems"); l++) //do this same thing with the hotbar later (in refreshhotbar loop)
        {
            InventoryItem spoon = Resources.Load<InventoryItem>(PlayerPrefs.GetString("ItemList#" + l));
            //Debug.Log(PlayerPrefs.GetString("ItemList#" + l));
            //InventoryItem spoon = AssetDatabase.FindAssets(PlayerPrefs.GetString("ItemList#") + l, typeof(ScriptableObject));
            AddItem(spoon);
        }

    }
    /*
    public void SetInventory(InventoryManager inventory)
    {
        this.inventory = inventory;
    }*/

    public void RedistributeItemsToHotbarUponStartGame()
    {
        //Debug.Log("what");
        for (var ty = 0; ty < hotbarArray.Length; ty++)
        {
            if (PlayerPrefs.GetString("HotbarList#" + ty) != "") //PlayerPrefs.GetInt("HotbarPlace#" + ty) != 0 thsi could aslo work
            {
                InventoryItem fork = Resources.Load<InventoryItem>(PlayerPrefs.GetString("HotbarList#" + ty));
                //Debug.Log(PlayerPrefs.GetString("HotbarList#" + ty));
                //Debug.Log(PlayerPrefs.GetString("HotbarList#" + ty));
                //Debug.Log(fork.displayName.ToString()); //these two debugs equal the same thing (or should
                //AddItem(fork); create a way to add item directly to hotbar
                AddHotbarItem(fork, PlayerPrefs.GetInt("HotbarPlace#" + ty));
            }
        }
    }

    public void AddItem(InventoryItem item)
    {
        if (item.isStackable)
        {
            bool itemAlreadyInInventory = false;
            foreach (InventoryItem i in itemList)
            {
                if (i.id == item.id)
                {
                    //UpdateUniqueValues();
                    UpdateAmounts(item, item.originalAmount);
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
                //UpdateUniqueValues();
                UpdateAmounts(item, item.originalAmount);
            }
        }
        else
        {
            itemList.Add(item);
            //UpdateUniqueValues();
            UpdateAmounts(item, item.originalAmount);
        }
        if(item.id == "Note1")
        {
            if(PlayerPrefs.GetInt("NoteNumber") < 1) PlayerPrefs.SetInt("NoteNumber", 1);
        } else if(item.id == "Note2")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 2) PlayerPrefs.SetInt("NoteNumber", 2);
        }
        else if (item.id == "Note3")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 3) PlayerPrefs.SetInt("NoteNumber", 3);
        }
        else if (item.id == "Note4")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 4) PlayerPrefs.SetInt("NoteNumber", 4);
        }
        else if (item.id == "Note5")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 5) PlayerPrefs.SetInt("NoteNumber", 5);
        }
        else if (item.id == "Note6")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 6) PlayerPrefs.SetInt("NoteNumber", 6);
        }
        else if (item.id == "Note7")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 7) PlayerPrefs.SetInt("NoteNumber", 7);
        }
        else if (item.id == "Note8")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 8) PlayerPrefs.SetInt("NoteNumber", 8);
        }
        else if (item.id == "Note9")
        {
            if (PlayerPrefs.GetInt("NoteNumber") < 9) PlayerPrefs.SetInt("NoteNumber", 9);
        }
        if (item.id == "Note10")
        {
            //theLevelManager.finalKey = true;
            PlayerPrefs.SetInt("finalKey", 1); // 1 corresponds to true, 0 to false == false is default value
        }
    }

    public void AddHotbarItem(InventoryItem item, int placeInHotbar)
    {
        if (item.isStackable)
        {
            
            //bro idek 
        }
        else
        {
            //add item to hotbar  array in correct place (probably involves remaking the array like seen before
            //InventoryItem[] bloat = hotbarArray;

            for (var g = 0; g < hotbarArray.Length; g++)
            {
                if(g == placeInHotbar - 1)
                {
                    hotbarArray[g] = item;
                }
            }

            //RefreshInventoryItems();

            // update amounts if you feel like it(i think this is broken and not being used anyways)
        }
    }

    public List<InventoryItem> GetItemList()
    {
        return itemList;
    }

    public void UpdateAmounts(InventoryItem ip, int amountToAdd)
    {
        float[] boat = itemAmounts;
        //List<float> boat = new List<float>(itemAmounts);
        itemAmounts = new float[itemList.Count];
        //boat.CopyTo(0, itemAmounts, 0, itemList.Count - 1); // specifies amount of boat that is copied to itemAmounts to be 1 less than the ItemList.Count, which makes sense
        boat.CopyTo(itemAmounts, 0);
            //also concat

        for(var w = 0; w < itemList.Count; w++)
        {
            if (itemList[w].id == ip.id)
            {
                if (itemList[w].isStackable)
                {
                    itemAmounts[w] += amountToAdd;
                }
                else
                {
                    itemAmounts[w] = 1;
                }
            }
        }
    }

    public void UpdateAmountsInCaseOfRemoval(int numberInInventory)
    {
        List<float> moat = new List<float>(itemAmounts);
        moat.RemoveAt(numberInInventory);
        if(moat.Count != itemList.Count)
        {
            Debug.Log("error please fix");
        }
        itemAmounts = new float[moat.Count];
        itemAmounts = moat.ToArray();
    }

    public void UpdateUniqueValues() //this function no longer needs to exist, but I put like 2 hours into making it and it was informatory so I'm keeping it.
    {
        float[] tomate = uniqueValues;
        uniqueValues = new float[itemList.Count];
        tomate.CopyTo(uniqueValues, 0);
        //uniqueValues.Clear();

        for (var x = 0; x < uniqueValues.Length; x++)
        {
            uniqueValues[x] = 0f;
        }

        //float amountOfInventoryItemsSharingName = 0f;
        //float amountToAdd = 0f;

        amountOfInventoryItemsSharing = new float[itemList.Count]; // automatically "clears" array (zeros)

        amountToIncreaseBy = new float[itemList.Count];

        for (var c = 0; c < amountToIncreaseBy.Length; c++)
        {
            amountToIncreaseBy[c] = 1f;
        }

        /*
        foreach(InventoryItem i in itemList)
        { 
            if (i.id == itemExpectedToIncreaseUniqueness.id)
            {
                amountToAdd = amountOfInventoryItemsSharingName++;

                //if (amountOfInventoryItemsSharingName > 1f)
                //{
                    //uniqueValues[itemList.IndexOf(i)] += 1;
                //}
            }
            uniqueValues.Add(amountToAdd);
            amountToAdd = 0f;
        }*/

        for (var i = 0; i < itemList.Count; i++)
        {
                amountOfInventoryItemsSharing[itemList.IndexOf(itemList[i])]++;

            if (amountOfInventoryItemsSharing[itemList.IndexOf(itemList[i])] > 1f)
            {
                uniqueValues[i] += amountToIncreaseBy[itemList.IndexOf(itemList[i])];
                amountToIncreaseBy[itemList.IndexOf(itemList[i])] += 1f;
            }
        }
        /*
        foreach(float z in uniqueValues)
        {
            if(z == null)
            {
                z = 0f;
            }
        }*/
    }

    public void RefreshInventoryItems()
    {
        refreshPositionItem = true;

        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;

            Destroy(child.gameObject);
        }
        
        int x = 0;
        int y = 0;
        /*
        foreach (InventoryItem i in GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize - firstBoxPositionx, y* itemSlotCellSize - firstBoxPositiony);
            Image image = itemSlotRectTransform.Find("DraggableItem").GetComponentInChildren<Image>();
            image.sprite = i.icon;
            TMP_Text uiText = itemSlotRectTransform.Find("DraggableItem").GetComponentInChildren<TMP_Text>();

            if (uniqueValues[itemList.LastIndexOf(i)] > 0f) { 

            }

            string namecheese = itemList.IndexOf(i).ToString();
            itemSlotRectTransform.Find("DraggableItem").name = namecheese;


            if (itemAmounts[itemList.IndexOf(i)] > 1 && i.isStackable)
            {
                uiText.SetText(itemAmounts[itemList.IndexOf(i)].ToString());
            }
            else
            {
                uiText.SetText("");
            }

            x++;
            if(x>= rowLength)
            {
                x = 0;
                y--;
            }
        }*/

        for (var r = 0; r < itemList.Count; r++)
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize - firstBoxPositionx, y * itemSlotCellSize - firstBoxPositiony);
            Image image = itemSlotRectTransform.Find("DraggableItem").GetComponentInChildren<Image>();
            image.sprite = itemList[r].icon;
            TMP_Text uiText = itemSlotRectTransform.Find("DraggableItem").GetComponentInChildren<TMP_Text>();


            float cheesen = indexerUnique;

            indexerUnique += 1f;

            string namecheese = cheesen.ToString();
            itemSlotRectTransform.Find("DraggableItem").name = namecheese;


            if (itemList[r].isStackable)
            {
                uiText.SetText(itemAmounts[r].ToString());
            }
            else
            {
                uiText.SetText("");
            }

            x++;
            if (x >= rowLength)
            {
                x = 0;
                y--;
            }
        }
        indexerUnique = 0f;

        //now set playerprefs
        for (var m = 0; m < itemList.Count; m++)
        {
            PlayerPrefs.SetString("ItemList#" + m, itemList[m].id);
        }
        PlayerPrefs.SetInt("amountOfStoredItems",(int)itemList.Count);

        RefreshHotbar();
    }

    public void SwapInventoryItems(int firstItem, int secondItem)
    {
        //switch items' places in itemList
        InventoryItem temp = itemList[firstItem];
        itemList[firstItem] = itemList[secondItem];
        itemList[secondItem] = temp;

        //switch amounts too
        float amountOfFirst = itemAmounts[firstItem];
        itemAmounts[firstItem] = itemAmounts[secondItem];
        itemAmounts[secondItem] = amountOfFirst;

        RefreshInventoryItems();

        selectedItemPlace = firstItem;
        RefreshPopupBar();
    }

    public void SwapBetweenHotbar(int firstItem, int secondItem) // only call if one of items is negative
    {

        //Debug.Log("catas in the bag");
        int itemInHotbar = 0;
        int itemInInventory = 0;

        if(firstItem < 0 && secondItem < 0)
        {
            InventoryItem tempe = hotbarArray[Mathf.Abs(firstItem) -1];
            hotbarArray[Mathf.Abs(firstItem)-1] = hotbarArray[Mathf.Abs(secondItem) -1];
            hotbarArray[Mathf.Abs(secondItem) -1] = tempe;
            RefreshInventoryItems();
            selectedItemPlace = firstItem; //value is already negative
            RefreshPopupBar();
            return;
        }
        else if(firstItem < 0)
        {
            itemInHotbar = firstItem;
            itemInInventory = secondItem;
        } else if(secondItem < 0)
        {
            itemInHotbar = secondItem;
            itemInInventory = firstItem;
        }

        InventoryItem tempr = itemList[itemInInventory];
        itemList[itemInInventory] = hotbarArray[Mathf.Abs(itemInHotbar)-1];
        hotbarArray[Mathf.Abs(itemInHotbar)-1] = tempr;

        RefreshInventoryItems();

        selectedItemPlace = firstItem;
        RefreshPopupBar();
    }

    public void RemoveItem(int numberInInventory)
    {
        for (var k = 0; k < itemList.Count; k++)
        {
            if(k == numberInInventory)
            {
                itemList.Remove(itemList[k]);

                RefreshInventoryItems();
            }
        }
    }

    public void ItemToHotbar(int numberInInventory, int hotbarNumber) //to empty hotbar space
    {
        for (var e = 0; e < itemList.Count; e++)
        {
            if (e == Mathf.Abs(numberInInventory))
            {
                hotbarArray[hotbarNumber - 1] = itemList[e];
                UpdateHotbarAmounts(hotbarArray[hotbarNumber - 1], itemAmounts[e]);

                itemList.Remove(itemList[e]);
                UpdateAmountsInCaseOfRemoval(numberInInventory); // must be int

                RefreshInventoryItems();

                selectedItemPlace = -hotbarNumber;
                RefreshPopupBar();
            }
        }

        // why is this in a for loop
        // couldn't i just do hotbarArray[hotbarNumber - 1} = itemList[Mathf.Abs(numberInInventory)];
        //whatever it works
    }

    public void HotbarItemToHotbar(int numberInHotbar, int hotbarNumber) //to empty hotbar space
    {
        for (var v = 0; v < hotbarArray.Length; v++)
        {
            if (v == (Mathf.Abs(numberInHotbar)-1))
            {
                hotbarArray[hotbarNumber - 1] = hotbarArray[v];
                hotbarArray[v] = null;
                //UpdateHotbarAmounts(hotbarArray[hotbarNumber - 1], hotbarAmounts[v]);

                RefreshInventoryItems();
                //Debug.Log("memes");
                selectedItemPlace = -hotbarNumber;
                RefreshPopupBar();
            }
        }
    }

    public void RefreshHotbar()
    {

        foreach (ItemDragUsableSlot hotbox in theLevelManager.OrderedHotbar)
        {
            ItemDraggable itemToDelete = hotbox.GetComponentInChildren<ItemDraggable>();
            if (itemToDelete != null)
            {
                Destroy(itemToDelete.gameObject);
            }
        }

        for (var h = 0; h < hotbarArray.Length; h++)
        {
            if (hotbarArray[h] != null)
            {
                RectTransform HotBarItem = Instantiate(hotBarItemTemplate, theLevelManager.OrderedHotbar[h].transform).GetComponent<RectTransform>();
                HotBarItem.GetComponent<ItemDraggable>().isInHotbar = true;
                Image image = HotBarItem.GetComponentInChildren<Image>();
                image.sprite = hotbarArray[h].icon;
                TMP_Text uiText = HotBarItem.GetComponentInChildren<TMP_Text>();
                if (hotbarArray[h].isStackable)
                {
                    uiText.SetText(hotbarAmounts[h].ToString());
                }
                else
                {
                    uiText.SetText("");
                }

                HotBarItem.name = (-(h + 1)).ToString();

            }
        }
        theLevelManager.OrderHotbarPrefabs();

        //now set playerprefs hotbar mode morb
        for (var v = 0; v < hotbarArray.Length; v++)
        {
            if (hotbarArray[v] != null)
            {
                PlayerPrefs.SetString("HotbarList#" + v, hotbarArray[v].id);
                PlayerPrefs.SetInt("HotbarPlace#" + v, v + 1);
                //Debug.Log("help");
            } else
            {
                //if the space is null
                PlayerPrefs.SetString("HotbarList#" + v, "");
                //PlayerPrefs.SetInt("HotbarPlace#" + v, 0); unnnecceasry for now
            }
        }
        //PlayerPrefs.SetInt("amountOfStoredHotbarItems", (int)hotbarArray.Length);
        // this is always 9 (constant)

    }

    public void UpdateHotbarAmounts(InventoryItem ip, float amountToAdd)
    {
        float[] mouth = hotbarAmounts;
        hotbarAmounts = new float[hotbarArray.Length];
        //mouth.CopyTo(itemAmounts, 0);

        for (var j = 0; j < itemList.Count; j++)
        {
            if (itemList[j].id == ip.id)
            {
                if (itemList[j].isStackable)
                {
                    itemAmounts[j] += amountToAdd;
                }
                else
                {
                    itemAmounts[j] = 1;
                }
            }
        }
    }

    public void RefreshPopupBar()
    {
        //RefreshPopupBar(null); < do this for having no item

        if(selectedItemPlace < 0) //in hotbar
        {
            if ((Mathf.Abs(selectedItemPlace) - 1) <= hotbarArray.Length) 
            {
                selectedItem = hotbarArray[Mathf.Abs(selectedItemPlace) - 1];
            } else
            {
                selectedItem = null;
            }
        } else //in inventory
        {
            if (selectedItemPlace <= itemList.Count -1)
            {
                selectedItem = itemList[selectedItemPlace];
            } else
            {
                selectedItem = null;
            }
        }

        if (selectedItem != null)
        {
            popupVisual.sprite = selectedItem.icon;
            popupDescription.SetText(selectedItem.descrip);
            popupItemName.SetText(selectedItem.displayName);
        } else
        {
            popupVisual.sprite = default;
            popupDescription.SetText("This is the item description menu. Here you can discover interesting details about your items, such as their place of origin, site of discovery, and/or details found on the item itself. ");
            popupItemName.SetText("XXXXXXXX");
        }
    }

    public void DiscardItem()
    {
        theLevelManager.Select2.Play();
        if (selectedItem == null) { return; }

        if (selectedItemPlace < 0) //in hotbar
        {
            hotbarArray[Mathf.Abs(selectedItemPlace) - 1] = null;
            RefreshInventoryItems();
            selectedItem = null;
            //you could put something here so popupbar refreshes to be blank
            RefreshPopupBar();
        }
        else //in inventory
        {
            itemList.RemoveAt(selectedItemPlace);
            UpdateAmountsInCaseOfRemoval(selectedItemPlace);
            RefreshInventoryItems();
            selectedItem = null;
            //you could put something here so popupbar refreshes to be blank
            RefreshPopupBar();
        }
    }

    public void ReturnItemToInventory() //from hotbar to inventory
    {
        theLevelManager.Select2.Play();
        if (selectedItem == null) { return; }

        if (selectedItemPlace >= 0) { return; }

        //remove itemfrom hotbar
        hotbarArray[Mathf.Abs(selectedItemPlace) - 1] = null;

        //put item in inv
        AddItem(selectedItem);

        RefreshInventoryItems();

        //could switch selecteditemplace instead for more intuitive
        selectedItem = null;
        RefreshPopupBar();
    }

    public void InventoryMenuAnimationToggleMenu()
    {
        InventoryMenuAnimator.SetBool("togglePopup", !InventoryMenuAnimator.GetBool("togglePopup"));
    }

}
