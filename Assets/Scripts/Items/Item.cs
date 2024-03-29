﻿using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
    public abstract class Item : ScriptableObject
    {
        public string name;
        public Sprite sprite;
        public AudioClip sound;
        public abstract void TriggerEffect(GameObject player);

    }
}