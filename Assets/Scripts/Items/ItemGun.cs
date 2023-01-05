using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine.Tilemaps;
using Items;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Items/Gun")]
public class ItemGun : Item
{
    public override void TriggerEffect(GameObject player)
    {
        PlayerDetails playerDetails = player.GetComponent<PlayerDetails>();
        PlayerController controller = player.GetComponent<PlayerController>();

        
        Vector3 currentPos = player.transform.position;
        Vector3Int gridPosition = controller.pathTilemap.WorldToCell(currentPos);

        Vector3Int lastPos = new Vector3Int();
        while (!controller.borderTilemap.HasTile(gridPosition))
        {
            lastPos = gridPosition;
                
            controller.pathTilemap.SetTileFlags(gridPosition, TileFlags.None);
            Color before = controller.pathTilemap.GetColor(gridPosition);
            
            // Set the colour.
            controller.pathTilemap.SetColor(gridPosition, playerDetails.Color);
        
            controller.onTileColored?.Invoke(playerDetails.PlayerID, gridPosition, before);
          
            Vector3Int dir = new Vector3Int((int) player.transform.right.x, (int) player.transform.right.y, (int) player.transform.right.z);
            gridPosition += dir;
        }

        float x = 0;
        float y = 0;

        if (Mathf.Ceil(Mathf.Abs(currentPos.x)) == Mathf.Abs(lastPos.x))
        {
            y = 0.2f;
            x = Mathf.Max(currentPos.x, lastPos.x) - Mathf.Min(currentPos.x, lastPos.x);
        }
        else if(Mathf.Ceil(Mathf.Abs(currentPos.y)) == Mathf.Abs(lastPos.y))
        {
            x = 0.2f;
            y = Mathf.Max(currentPos.x, lastPos.x) - Mathf.Min(currentPos.x, lastPos.x);
        }

        RaycastHit2D[] objects = Physics2D.BoxCastAll((Vector2)(currentPos), new Vector2(x, y), 0f,
            direction: new Vector2(player.transform.right.x, player.transform.right.y));
        
        Debug.Log(objects);
        if (objects.Length > 0)
        {
            foreach (var t in objects)
            {
               
                if( t.transform.gameObject.layer != LayerMask.NameToLayer("Player")) continue;
                if (playerDetails.PlayerID == t.transform.gameObject.GetComponent<PlayerDetails>().PlayerID)
                    continue;
                Debug.Log(t.transform.gameObject.GetComponent<PlayerDetails>().PlayerID);
                PlayerController playerController =
                    t.transform.gameObject.GetComponent<PlayerController>();
                playerController.Respawn();
                    
                Debug.Log("YESSSS");
                SoundManager.Instance.PlayPlayerHitSound();
                Logger.Instance.WriteToFile(LogId.PlayerHit, "Player" + player.gameObject.GetComponent<PlayerDetails>().PlayerID + " hit Player" + objects[0].transform.gameObject.GetComponent<PlayerDetails>().PlayerID);
            }
        }
        playerDetails.HasGun = false;
    }

}
