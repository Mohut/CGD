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

    private float[,] heatmap = new float[18, 26];

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
                heatmap[y,x] = 0;
            }
        }
    }

    public void GameOver(int playerIndex)
    {
        Debug.Log("Game Over");

        gameOverEvent?.Invoke(playerIndex);
        LogResult(playerIndex);
        // log heatmap
        Logger.Instance.WriteToFile(LogId.Heatmap, "Heatmap");
        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                row += heatmap[i,j] + " ";
            }
            Logger.Instance.WriteToFile(LogId.Heatmap, row);
        }
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
        heatmap[ (int)(pos.y)*-1 + 5, (int) (pos.x + 8)] += 1;
    }

    private void LogResult(int winnerId)
    {
        Logger.Instance.WriteToFile(LogId.Other, playerCount + " players played");
        Logger.Instance.WriteToFile(LogId.Other, "player" + winnerId + " won");
    }
}
