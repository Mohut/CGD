using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private List<Slider> slider;
    [SerializeField] private GameObject startShadow;
    [SerializeField] private TextMeshProUGUI oneText;
    [SerializeField] private TextMeshProUGUI twoText;
    [SerializeField] private TextMeshProUGUI threeText;
    void Start()
    {
        ScoreManager.Instance.onPlayerScoreChange += SetProgress;
        GameManager.Instance.gameOverEvent += LogPlayerProgression;
        GameManager.Instance.gameStartEvent += CountDown;

        if (GameManager.Instance.PlayerCount == 2)
            return;

        slider[2].gameObject.SetActive(true);

        if (GameManager.Instance.PlayerCount == 3)
            return;

        slider[3].gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        ScoreManager.Instance.onPlayerScoreChange -= SetProgress;
        GameManager.Instance.gameOverEvent -= LogPlayerProgression;
        GameManager.Instance.gameStartEvent -= CountDown;
    }

    private void SetProgress(int player, float progress)
    {
        switch (player)
        {
            case 1:
                slider[0].value = progress;
                break;
            case 2:
                slider[1].value = progress;
                break;
            case 3:
                slider[2].value = progress;
                break;
            case 4:
                slider[3].value = progress;
                break;
        }
        
        CheckIfPlayerAhead();
    }

    public void CheckIfPlayerAhead()
    {
        int playerAhead = -1;
        float highestValue = slider[0].value;

        if (slider[0].value > 0)
            playerAhead = 1;

        for (int i = 0; i < slider.Count - 1; i++)
        {
            if (highestValue < slider[i].value)
            {
                highestValue = slider[i].value;
                playerAhead = i+1;
            }
        }
        
        GameManager.Instance.ChangePlayerAhead(playerAhead);
    }

    private void LogPlayerProgression(int noInt)
    {
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player1 progress: " + slider[0].value);
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player2 progress: " + slider[1].value);

        if (GameManager.Instance.PlayerCount == 2)
            return;
        
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player3 progress: " + slider[2].value);

        if (GameManager.Instance.PlayerCount == 3)
            return;
        
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player4 progress: " + slider[3].value);
    }

    private void CountDown()
    {
        StartCoroutine(Co_CountDown());
    }

    IEnumerator Co_CountDown()
    {
        yield return new WaitForSeconds(1);
        threeText.enabled = false;
        twoText.enabled = true;
        yield return new WaitForSeconds(1);
        twoText.enabled = false;
        oneText.enabled = true;
        yield return new WaitForSeconds(1);
        startShadow.SetActive(false);
        GameManager.Instance.GameStarted = true;
    }
}
