using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;


public class RealItemDispenser : MonoBehaviour
{
    public GameObject gun;
    public GameObject bomb;

    public GameObject itemSpawn1;
    public GameObject itemSpawn2;

    public float secondsToNextRespawn;

    public Action onItemCollected;
    // Start is called before the first frame update
    void Start()
    {
        onItemCollected += DeleteItem;
        SpawnItem();
    }

    private void DeleteItem()
    {
        StartCoroutine(StartNewRound());
    }

    IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(secondsToNextRespawn);
        SpawnItem();
    }

    private void SpawnItem()
    {
        int x = Random.Range(0, 2);
       
        if (x == 0)
        {
            GameObject gunO = Instantiate(gun, itemSpawn1.transform);
            ItemDispenser itemDispenser = gunO.GetComponent<ItemDispenser>();
            itemDispenser.RealItemDispenser = this;
        }
        else
        {
            GameObject bombO =Instantiate(bomb, itemSpawn2.transform);
            ItemDispenser itemDispenser = bombO.GetComponent<ItemDispenser>();
            itemDispenser.RealItemDispenser = this;
        }
    }

    
}
