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
    public GameObject itemSpawn3;
    public GameObject itemSpawn4;

    public float secondsToNextRespawn;

    public Action<int> onItemCollected;

    private List<int> currentSpwans = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        onItemCollected += DeleteItem;
        InvokeRepeating("SpawnItem", 5f, 5f);
    }

    private void DeleteItem(int spawnpoint)
    {
        var targetIndex = currentSpwans.IndexOf(spawnpoint);
        currentSpwans.RemoveAt(targetIndex);
        //StartCoroutine(StartNewRound());
    }

    IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(secondsToNextRespawn);
        SpawnItem();
    }

    private void SpawnItem()
    {
        int x = Random.Range(0, 6);
        GameObject item = Random.Range(0, 1) == 0 ? gun : bomb;
       
        if (x == 0)
        {
            if (currentSpwans.Contains(0))
                return;
            GameObject gunO = Instantiate(item, itemSpawn1.transform);
            ItemDispenser itemDispenser = gunO.GetComponent<ItemDispenser>();
            itemDispenser.RealItemDispenser = this;
            itemDispenser.Spawnpoint = 0;
            currentSpwans.Add(0);
        }
        else if (x == 1)
        {
            if (currentSpwans.Contains(1))
                return;
            GameObject gunO = Instantiate(item, itemSpawn2.transform);
            ItemDispenser itemDispenser = gunO.GetComponent<ItemDispenser>();
            itemDispenser.RealItemDispenser = this;
            itemDispenser.Spawnpoint = 1;
            currentSpwans.Add(1);
        }
        else if (x == 2)
        {
            if (currentSpwans.Contains(2))
                return;
            GameObject gunO = Instantiate(item, itemSpawn3.transform);
            ItemDispenser itemDispenser = gunO.GetComponent<ItemDispenser>();
            itemDispenser.RealItemDispenser = this;
            itemDispenser.Spawnpoint = 2;
            currentSpwans.Add(2);
        }
        else if (x == 4)
        {
            if (currentSpwans.Contains(3))
                return;
            GameObject bombO =Instantiate(bomb, itemSpawn4.transform);
            ItemDispenser itemDispenser = bombO.GetComponent<ItemDispenser>();
            itemDispenser.RealItemDispenser = this;
            itemDispenser.Spawnpoint = 3;
            currentSpwans.Add(3);
        }
      
    }

    
}
