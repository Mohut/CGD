using UnityEngine;

public class ColorBucket : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") == false)
            return;
        SoundManager.Instance.PlayColorRefillSound();
        PlayerDetails playerDetails = col.gameObject.GetComponentInChildren<PlayerDetails>();
        playerDetails.CurrentFields = playerDetails.MaxFields;
    }
}