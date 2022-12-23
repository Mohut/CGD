using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject StartUI;

    public void StartGame()
    {
        StartUI.SetActive(false);
        GameManager.Instance.GameStarted = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
