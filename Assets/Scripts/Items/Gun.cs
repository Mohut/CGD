using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerDetails playerDetails = col.gameObject.GetComponent<PlayerDetails>();
        Debug.Log(playerDetails.HasGun);
        playerDetails.HasGun = true;
        Debug.Log(playerDetails.HasGun);
        Destroy(gameObject);
    }

  
}
