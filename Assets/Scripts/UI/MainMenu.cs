using System;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject inGameUI;
    private void Start()
    {
        GameManager.Instance.gameStartEvent += StartGame;
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameStartEvent -= StartGame;
    }

    public void StartGame()
    {
        startUI.SetActive(false);
        inGameUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
