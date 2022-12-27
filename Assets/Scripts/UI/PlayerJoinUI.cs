using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoinUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private GameObject player1Image;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private GameObject player2Image;
    [SerializeField] private TextMeshProUGUI player3Text;
    [SerializeField] private GameObject player3Image;
    [SerializeField] private TextMeshProUGUI player4Text;
    [SerializeField] private GameObject player4Image;

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
            startButton.interactable = true;
        }
    }
}
