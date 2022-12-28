using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
    public abstract class Item : ScriptableObject
    {
        public string name;

        public abstract void TriggerEffect(GameObject player);

    }
}