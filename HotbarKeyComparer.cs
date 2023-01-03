using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sorting
{

    public class HotbarKeyComparer : IComparer<ItemDragUsableSlot>
    {
        public int Compare(ItemDragUsableSlot box1, ItemDragUsableSlot box2)
        {
                return box1.hotbarKey.CompareTo(box2.hotbarKey);
        }
    }

}
