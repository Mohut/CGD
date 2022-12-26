using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    [SerializeField] private Slider slider3;
    [SerializeField] private Slider slider4;
    void Start()
    {
        ScoreManager.Instance.onPlayerScoreChange += SetProgress;
    }
    private void OnDestroy()
    {
        ScoreManager.Instance.onPlayerScoreChange -= SetProgress;
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
}
