using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorTrigger : MonoBehaviour
{
    public PlayerController _playerController;
    public PlayerDetails _playerDetails;
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger");
        if (other.gameObject.name.Equals("LevelPaths"))
        {
            Color playerColor = _playerDetails.Color;
            //_playerController.SetTileColour(playerColor, Vector3Int.FloorToInt(transform.position), _playerController.pathTilemap);
        }
    }
}
