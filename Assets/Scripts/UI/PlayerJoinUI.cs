using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private GameObject player1Image;
    [SerializeField] private Animator player1Animation;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private GameObject player2Image;
    [SerializeField] private Animator player2Animation;
    [SerializeField] private TextMeshProUGUI player3Text;
    [SerializeField] private GameObject player3Image;
    [SerializeField] private Animator player3Animation;
    [SerializeField] private TextMeshProUGUI player4Text;
    [SerializeField] private GameObject player4Image;
    [SerializeField] private Animator player4Animation;

    private void Start()
    {
        GameManager.Instance.showPlayerEvent += ShowPlayer;
    }

    private void OnDestroy()
    {
        GameManager.Instance.showPlayerEvent -= ShowPlayer;
    }

    public void OnPlayerJoined()
    {
        GameManager.Instance.PlayerCount++;
        switch (GameManager.Instance.PlayerCount)
        {
            case 1:
                player1Text.SetText("Player 1 ready");
                player1Image.SetActive(true);
                break;
            case 2:
                player2Text.SetText("Player 2 ready");
                player2Image.SetActive(true);
                break;
            case 3:
                player3Text.SetText("Player 3 ready");
                player3Image.SetActive(true);
                break;
            case 4:
                player4Text.SetText("Player 4 ready");
                player4Image.SetActive(true);
                break;
        }

        if (GameManager.Instance.PlayerCount >= 2)
        {
            startText.enabled = true;
        }
    }

    public void ShowPlayer(int playerIndex)
    {
        switch (playerIndex)
        {
            case 1:
                player1Animation.Play("Player1");
                break;
            case 2:
                player2Animation.Play("Player2");
                break;
            case 3:
                player3Animation.Play("Player3");
                break;
            case 4:
                player4Animation.Play("Player4");
                break;
        }
    }
}
