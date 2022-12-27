using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void gameOverHandler(int playerIndex);
    public event gameOverHandler gameOverEvent;
    private bool gameStarted;
    private int playerCount = 0;

    public bool GameStarted { get => gameStarted; set => gameStarted = value; }
    public int PlayerCount { get => playerCount; set => playerCount = value; }

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
        LogResult(playerIndex);
    }

    private void LogResult(int winnerId)
    {
        Logger.Instance.WriteToFile(LogId.Other, playerCount + " players played");
        Logger.Instance.WriteToFile(LogId.Other, "player" + winnerId + " won");
    }
}
