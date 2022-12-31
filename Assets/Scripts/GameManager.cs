using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerSpawnManager SpawnManager;
    public TilemapZoneManager ZoneManager;
    public delegate void gameOverHandler(int playerIndex);
    public delegate void gameStartHandler();
    public delegate void showPlayerHandler(int playerIndex);
    public delegate void ShowPlayerAheadHandler(int playerIndex);
    public event gameOverHandler gameOverEvent;
    public event gameStartHandler gameStartEvent;
    public event showPlayerHandler showPlayerEvent;
    public event ShowPlayerAheadHandler showPlayerAhead;
    private int playerAhead = -1;
    private bool gameStarted;
    private int playerCount = 0;

    public bool GameStarted { get => gameStarted; set => gameStarted = value; }
    public int PlayerCount { get => playerCount; set => playerCount = value; }
    public int PlayerAhead { get => playerAhead; set => playerAhead = value; }

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

    public void StartGame()
    {
        gameStartEvent?.Invoke();
    }

    public void ShowPlayer(int playerIndex)
    {
        showPlayerEvent?.Invoke(playerIndex);
    }

    public void ChangePlayerAhead(int playerIndex)
    {
        playerAhead = playerIndex;
        showPlayerAhead?.Invoke(playerAhead);
    }

    private void LogResult(int winnerId)
    {
        Logger.Instance.WriteToFile(LogId.Other, playerCount + " players played");
        Logger.Instance.WriteToFile(LogId.Other, "player" + winnerId + " won");
    }
}
