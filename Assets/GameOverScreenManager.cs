using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverScreenManager : MonoBehaviour
{
    [SerializeField] private UIDocument gameOverDocument;
    private VisualElement root;

    private void Start()
    {
        root = gameOverDocument.rootVisualElement;
        root.Q<Button>("PlayAgainButton").clicked += Restart;
        GameManager.Instance.gameOverEvent += ShowScreen;
        root.Q("GameOverScreen").style.display = DisplayStyle.None;
    }

    private void OnDestroy()
    {
        root.Q<Button>("PlayAgainButton").clicked -= Restart;
        GameManager.Instance.gameOverEvent -= ShowScreen;
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowScreen(int playerIndex)
    {
        root.Q("GameOverScreen").style.display = DisplayStyle.Flex;
        root.Q<Label>("WinText").text = "Player " + playerIndex + " Wins!";
    }
}