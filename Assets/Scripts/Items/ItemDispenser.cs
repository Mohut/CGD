using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class ItemDispenser : MonoBehaviour
    {
        public Item item;
        private List<Item> currentItems = new List<Item>();

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.TryGetComponent<PlayerController>(out PlayerController controller)) return;
            controller.AddItem(item);
            Destroy(gameObject);
        }
    }
}