using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    private List<Collider2D> colliderList = new List<Collider2D>();
    private ContactFilter2D contactfilter2D = new ContactFilter2D();

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        collider.OverlapCollider(contactfilter2D, colliderList);
       
    }
}
