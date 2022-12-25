using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void gameOverHandler(int playerIndex);
    public event gameOverHandler gameOverEvent;
    private bool gameStarted;

    public bool GameStarted { get => gameStarted; set => gameStarted = value; }

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

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void GameOver(int playerIndex)
    {
        gameOverEvent?.Invoke(playerIndex);
    }
}
