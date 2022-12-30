using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    [SerializeField] private Slider slider3;
    [SerializeField] private Slider slider4;
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

        slider3.gameObject.SetActive(true);

        if (GameManager.Instance.PlayerCount == 3)
            return;

        slider4.gameObject.SetActive(true);
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
                slider1.value = progress;
                break;
            case 2:
                slider2.value = progress;
                break;
            case 3:
                slider3.value = progress;
                break;
            case 4:
                slider4.value = progress;
                break;
        }
    }

    private void LogPlayerProgression(int noInt)
    {
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player1 progress: " + slider1.value);
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player2 progress: " + slider2.value);

        if (GameManager.Instance.PlayerCount == 2)
            return;
        
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player3 progress: " + slider3.value);

        if (GameManager.Instance.PlayerCount == 3)
            return;
        
        Logger.Instance.WriteToFile(LogId.PlayerAheadTime, "Player4 progress: " + slider4.value);
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
