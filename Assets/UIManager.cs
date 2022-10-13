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
        SetProgress(1, 100);
    }

    // Update is called once per frame
    void Update() {
        
    }


    private void SetProgress(int player, float progress) {
        _root.Q<ProgressBar>("Player"+player+"Progress").lowValue = progress;
    }
}
