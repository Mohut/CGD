using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public Dictionary<int, float> PlayerScores = new Dictionary<int, float>();

    public PlayerSpawnManager SpawnManager;

    private Dictionary<int, bool> LeadPlayer = new Dictionary<int, bool>();

    public Action<int, float> onPlayerScoreChange;
    
    [SerializeField] private int pointsToWin;
    [SerializeField] private float _scoreIntervall;
    [SerializeField] private float _scorePerIntervall;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("No multiple Instance of a Singleton possible");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnManager.onPlayerSpawn += (int id, PlayerController _) =>
        {
            PlayerScores[id] = 0;
            LeadPlayer[id] = false;
        };
        StartCoroutine(ScoreLoop());
    }

    public void AddScore(int playerid, float score)
    {
        Color bgColor = new Color(SpawnManager.colors[playerid-1].r - 0.5f, SpawnManager.colors[playerid-1].g - 0.5f, SpawnManager.colors[playerid-1].b - 0.5f);
        Camera.main.backgroundColor = bgColor;
        PlayerScores[playerid] += score;
        onPlayerScoreChange?.Invoke(playerid, PlayerScores[playerid]);
        if(PlayerScores[playerid] >= pointsToWin)
            GameManager.Instance.GameOver(playerid);
    }

    public void SetLead(int playerid)
    {
        Dictionary<int, bool> temp = new Dictionary<int, bool>();
        foreach (var pair in LeadPlayer)
        {
            if (pair.Key != playerid)
            {
                temp[pair.Key] = false;
            }
            else
            {
                temp[pair.Key] = true;
            }

        }

        LeadPlayer = temp;
    }

    public void ResetLead()
    {
        Dictionary<int, bool> temp = new Dictionary<int, bool>();
        foreach (var pair in LeadPlayer)
        {
            temp[pair.Key] = false;
        }

        LeadPlayer = temp;
    }

    private IEnumerator ScoreLoop()
    {
        while (true)
        {
            foreach (var pair in LeadPlayer)
            {
                if (pair.Value)
                {
                    AddScore(pair.Key,_scorePerIntervall);
                }
            }
            
            yield return new WaitForSeconds(_scoreIntervall);
        }
    }
}
