using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItem : ScriptableObject
{
    //public IconAttribute("Assets/0Textures/Ammo 12x70 rem Metallic.png");

    public string id; //id must match name of prefab
    public string displayName;
    public Sprite icon;
    public GameObject prefab; // tool to be spawned
    public bool isStackable;
    [InspectorName("Original Amount")]
    public int originalAmount = 1;

    //public bool isEquippable;
    public Vector3 prefabSpawnPosition;

    //public something videoForPopupVisual;
    [TextArea(10, 10)]
    public string descrip;

    //[HideInInspector]
    //public ItemMonoAsset masset = GetComponentInChildren<ItemMonoAsset>();


}
