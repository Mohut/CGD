using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapZoneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tilemap pathMap;

    [SerializeField] private Tilemap wallMap;

    [SerializeField] private PlayerSpawnManager _spawnManager;
    
    public List<Vector3> ZoneMiddlePoints = new List<Vector3>();
    public List<Vector3> ZoneSizes = new List<Vector3>();

    private Dictionary<int, Bounds> ZoneBounds = new Dictionary<int, Bounds>();
    public Dictionary<int, Dictionary<int, int>> Points = new Dictionary<int, Dictionary<int, int>>();
    private Dictionary<int, int> ZoneTileCounts = new Dictionary<int, int>();
    private Dictionary<int, int> ZoneOwner = new Dictionary<int, int>();

    private Action<int, int> onNewZoneOwner;

    void Start()
    {
        if (ZoneMiddlePoints.Count != ZoneSizes.Count)
        {
            Debug.LogWarning("Zone Middlepoints and Sizes do not have the same Count");
        }
        InitZones();
        // attach to player events
        _spawnManager.onPlayerSpawn += RegisterPlayer;
    }

    private void OnDestroy()
    {
        _spawnManager.onPlayerSpawn -= RegisterPlayer;
    }

    private void RegisterPlayer(int id, PlayerController controller)
    {
        controller.onTileColored += UpdateTileZones;
    }

    private void InitZones()
    {
        for (int i = 0; i < ZoneSizes.Count; i++)
        {
            Bounds bounds = new Bounds(ZoneMiddlePoints[i], ZoneSizes[i]);
            ZoneTileCounts[i] = GetTileCount(pathMap,
                new BoundsInt(Vector3Int.FloorToInt(bounds.center), Vector3Int.FloorToInt(bounds.size)));
            Vector3 middle = ZoneMiddlePoints[i];
            ZoneBounds[i] = new Bounds(new Vector3(middle.x + ZoneSizes[i].x / 2, middle.y + ZoneSizes[i].y / 2, 0),
                ZoneSizes[i]);
            Points[i] = new Dictionary<int, int>(){{1,0},{2,0},{3,0},{4,0}};
            ZoneOwner[i] = 0;
        }
    }

    public Bounds ConvertBoundsToTilemapBounds(Bounds bounds)
    {
        // tilemapbounds are computed different, the "center" is actually the bottom left corner
        
        return new Bounds(new Vector3(bounds.center.x - Mathf.Floor(bounds.size.x/2),bounds.center.y-Mathf.Floor(bounds.size.y/2),0), bounds.size);
    }


    public void UpdateTileZones(int id, Vector3 tilepos, Color previous)
    {
        for (int i = 0; i < ZoneBounds.Count; i++)
        {
            if (ZoneBounds[i].Contains(tilepos+new Vector3(0.5f,0.5f,0)))
            {
                // tile is in zone i
                Points[i][id] ++;
                
                // check if previous color is from a player => reduce points
                if (previous != Color.white)
                {
                    Points[i][_spawnManager.PlayerColorDictionary[previous]]--;
                }

                if (ZoneOwner[i] != 0)
                {
                    if (Points[i][ZoneOwner[i]] < ZoneTileCounts[i] / 2)
                    {
                        ResetZoneOwner(i);
                        ZoneOwner[i] = 0;
                    }
                }
                
                // update the zone
                if (Points[i][id] > ZoneTileCounts[i] / 2)
                {
                    if (ZoneOwner[i] != id)
                    {
                        SetZoneOwner(id, i);
                    }
                }
            }
        }
    }

    private void ColorWalls(int zoneid, int playerid, Color? color=null)
    {
        Color playercolor = color!=null ? color.Value : _spawnManager.PlayerColorDictionary.FirstOrDefault(x => x.Value == playerid).Key;
        Vector3 middle = ZoneBounds[zoneid].center;
        Bounds BoundsForTilemap =
            new Bounds(new Vector3(middle.x - ZoneSizes[zoneid].x / 2, middle.y - ZoneSizes[zoneid].y / 2, 0),
                ZoneSizes[zoneid]);
        ColorAllTiles(wallMap, BoundsForTilemap, playercolor );
    }

    private void SetZoneOwner(int playerid, int zone)
    {
        ZoneOwner[zone] = playerid;
        ColorWalls(zone, playerid);
        SoundManager.Instance.PlayZoneTakenSound();
        CheckZoneProgress();
        Logger.Instance.WriteToFile(LogId.ZoneTakenTime, Time.time.ToString());
    }

    private void ResetZoneOwner(int zone)
    {
        ColorWalls(zone, 0, Color.white);
        CheckZoneProgress();
    }


    private void CheckZoneProgress()
    {
        Dictionary<int, int> occurences = new Dictionary<int, int>();
        foreach(int i in ZoneOwner.Values)
            if(occurences.ContainsKey(i))
                occurences[i]++;
            else
                occurences[i] = 1;

        int leadingPlayer = occurences.OrderByDescending(x => x.Value).Where(x=>x.Key!=0).First().Key;
        int leadingZones = occurences[leadingPlayer];

        foreach (KeyValuePair<int,int> pair in occurences)
        {
            if (pair.Key != leadingPlayer && pair.Value == leadingZones)
            {
                ScoreManager.Instance.ResetLead();
                return;
            }
        }

        // add points and visualize in progresss bar
        ScoreManager.Instance.SetLead(leadingPlayer);


    }
    
    private int GetTileCount(Tilemap map, BoundsInt bounds)
    {
        int counter = 0;   
        TileBase[] allTiles = map.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    counter++;
                }
            }
        }

        return counter;
    }
    
    public void ColorAllTiles(Tilemap map, Bounds bounds, Color color, Action<Color, Vector3Int> callback = null)
    {
        BoundsInt boundsInt = new BoundsInt(Vector3Int.FloorToInt(bounds.center), Vector3Int.FloorToInt(bounds.size));
        TileBase[] allTiles = map.GetTilesBlock(boundsInt);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];

                if (tile != null)
                {
                    // you may not need to find gridPlace, in which case cut next line
                    Vector3Int gridPlace = new Vector3Int(
                        x + boundsInt.xMin, y + boundsInt.yMin, boundsInt.z);
                    Vector3 worldPlace = map.CellToWorld(gridPlace);
                    
                    // do something
                    Vector3Int worldCoords = Vector3Int.FloorToInt(worldPlace);
                    map.SetTileFlags(worldCoords, TileFlags.None);
                    map.SetColor(worldCoords, color);
                    map.RefreshTile(worldCoords);
                    if (callback != null)
                    {
                        callback(color, worldCoords);
                    }
                }
            }
        }
    }
    
}
