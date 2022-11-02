using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void gameOverHandler(int playerIndex);
    public event gameOverHandler gameOverEvent;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void GameOver(int playerIndex)
    {
        gameOverEvent?.Invoke(playerIndex);
    }
}
