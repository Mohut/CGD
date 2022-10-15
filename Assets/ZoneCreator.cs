using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneCreator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;
    private TileBase[] tileBases;
    private List<Tile> tiles;

    private void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        tileBases = tilemap.GetTilesBlock(bounds);
        
        tilemap.SetTile(new Vector3Int(1,0,0), tile);
        //tilemap.GetTile<Tile>(new Vector3Int(7, 0, 0)).color = Color.magenta;
        tilemap.RefreshAllTiles();

        foreach (TileBase tileBase in tileBases)
        {
           
        }
    }
}
