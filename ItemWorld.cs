using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    /*
    public static ItemWorld SpawnItemWorld(Itemv2 item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
    }
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Itemv2 item)
    {
        this.SetItem = item;
        spriteRenderer.sprite = item.GetSprite();
    }*/
    [SerializeField] public InventoryItem item;
    void Awake()
    {

    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
