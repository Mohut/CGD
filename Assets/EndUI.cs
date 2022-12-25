using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Color[] playerColors;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        GameManager.Instance.gameOverEvent += SetWinnerText;
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameOverEvent -= SetWinnerText;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void SetWinnerText(int playerIndex)
    {
        canvas.enabled = true;
        Time.timeScale = 0;
        winnerText.text = "Player " + playerIndex + " Wins!!!";
        winnerText.color = playerColors[playerIndex-1];
    }
}
