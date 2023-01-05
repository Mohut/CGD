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

    private float[,] heatmap;

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

        for (int x = 0; x < 24; x++)
        {
            for (int y = 0; y < 18; y++)
            {
                heatmap[x, y] = 0;
            }
        }
    }

    public void GameOver(int playerIndex)
    {
        // log heatmap
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

    public void RegisterField(Vector3 pos)
    {
        heatmap[(int) (pos.x + 12), (int) (pos.y + 9)] += 1;
    }

    private void LogResult(int winnerId)
    {
        Logger.Instance.WriteToFile(LogId.Other, playerCount + " players played");
        Logger.Instance.WriteToFile(LogId.Other, "player" + winnerId + " won");
    }
}
