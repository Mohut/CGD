using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField]
    private Tilemap pathTilemap;
    [SerializeField]
    private Tilemap borderTilemap;
    [SerializeField]
    private Transform[] spawnLocations;
    
    [SerializeField]
    private Color[] colors;

    public Dictionary<Color, int> PlayerColorDictionary = new Dictionary<Color, int>();

    public Action<int, PlayerController> onPlayerSpawn;
    
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerDetails newPlayer = playerInput.gameObject.GetComponent<PlayerDetails>();
        PlayerController playerController = playerInput.gameObject.GetComponent<PlayerController>();
        // Set the player ID, add one to the index to start at Player 1
        newPlayer.PlayerID = playerInput.playerIndex + 1;

        // Set the start spawn position of the player using the location at the associated element into the array.
        newPlayer.StartPos = spawnLocations[playerInput.playerIndex].position;

        // Set color of player
        newPlayer.Color = colors[playerInput.playerIndex];
        PlayerColorDictionary[newPlayer.Color] = newPlayer.PlayerID;

        playerController.borderTilemap = borderTilemap;
        playerController.pathTilemap = pathTilemap;
        onPlayerSpawn?.Invoke(newPlayer.PlayerID, playerController);
        _playerManager.OnPlayerJoined();
    }
}
