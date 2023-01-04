using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class ItemDispenser : MonoBehaviour
    {
        public Item item;
        public RealItemDispenser RealItemDispenser;
        

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.TryGetComponent<PlayerController>(out PlayerController controller)) return;
            RealItemDispenser.onItemCollected?.Invoke();
            controller.AddItem(item);
            Destroy(gameObject);
        }
    }
}