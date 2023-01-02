using System;
using System.Collections;
using System.Collections.Generic;

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
        
        while (!controller.borderTilemap.HasTile(gridPosition))
        {
            if(controller.borderTilemap.HasTile(gridPosition))
                break;
            controller.pathTilemap.SetTileFlags(gridPosition, TileFlags.None);
            Color before = controller.pathTilemap.GetColor(gridPosition);
            
            // Set the colour.
            controller.pathTilemap.SetColor(gridPosition, playerDetails.Color);
        
            controller.onTileColored?.Invoke(playerDetails.PlayerID, gridPosition, before);
          
            Vector3Int dir = new Vector3Int((int) player.transform.right.x, (int) player.transform.right.y, (int) player.transform.right.z);
            gridPosition += dir;
        }

        playerDetails.HasGun = false;
    }

}
