using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument ingameUI;

    private VisualElement _root;

    // Start is called before the first frame update
    void Start() {
        _root = ingameUI.rootVisualElement;
        
        SetProgress(1, 0);
        SetProgress(2, 0);
        SetPhase(3);
        
        ScoreManager.Instance.onPlayerScoreChange += SetProgress;

        _root.Q<ProgressBar>("Player1Progress").Q<Label>().text = "Red";
        _root.Q<ProgressBar>("Player2Progress").Q<Label>().text = "Green";
        _root.Q<ProgressBar>("Player3Progress").Q<Label>().text = "Blue";
        _root.Q<ProgressBar>("Player4Progress").Q<Label>().text = "Yellow";
    }
    
    void Update()
    {
        if (GameManager.Instance.GameStarted == false)
            return;
        
        if (_root.Q("GameStartScreen").style.display.value == DisplayStyle.Flex)
            _root.Q("GameStartScreen").style.display = DisplayStyle.None;
    }


    public void SetProgress(int player, float progress) {
        _root.Q<ProgressBar>("Player"+player+"Progress").lowValue = progress;
    }

    private void SetPhase(int phase)
    {
        
        VisualElement phases = _root.Q<VisualElement>("Phases");
        if (phase > phases.childCount)
        {
            Debug.LogError("Phase is higher than childcount (Code: 30476323)");
            return;
        }
        
        int currentChild = 0;
        foreach (VisualElement child in phases.Children())
        {
            child.SetEnabled(true);
            if (currentChild >= phase) {
                child.SetEnabled(false);
            }

            currentChild++;
        }
    }
    
    
}
