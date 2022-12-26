using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject inGameUI;

    public void StartGame()
    {
        startUI.SetActive(false);
        inGameUI.SetActive(true);
        GameManager.Instance.GameStarted = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
