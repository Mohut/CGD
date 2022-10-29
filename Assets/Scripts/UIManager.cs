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
    }

    // Update is called once per frame
    void Update() {
        
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
