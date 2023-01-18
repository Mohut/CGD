using UnityEngine;

public class FrontChecker : MonoBehaviour
{
    private bool behindPlayer;
    [SerializeField] public BoxCollider2D collider;

    public bool BehindPlayer
    {
        get => behindPlayer;
        set => behindPlayer = value;
    }
    
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
            behindPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
            behindPlayer = false;
    }
}
