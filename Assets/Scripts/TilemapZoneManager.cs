using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapZoneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tilemap tm;

    [SerializeField] private PlayerSpawnManager _spawnManager;

    public List<Vector3> ZoneMiddlePoints = new List<Vector3>();
    public List<Vector3> ZoneSizes = new List<Vector3>();

    private Dictionary<int, Bounds> ZoneBounds = new Dictionary<int, Bounds>();
    public Dictionary<int, Dictionary<int, int>> Points = new Dictionary<int, Dictionary<int, int>>();

    void Start()
    {
        if (ZoneMiddlePoints.Count != ZoneSizes.Count)
        {
            Debug.LogWarning("Zone Middlepoints and Sizes do not have the same Count");
        }
        InitZones();
        // attach to player events
        _spawnManager.onPlayerSpawn += RegisterPlayer;



        //GetTiles(tm);
        // Debug.Log(grid);
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
            GetTiles(tm,
                new BoundsInt(Vector3Int.FloorToInt(bounds.center), Vector3Int.FloorToInt(bounds.size)));
            Vector3 middle = ZoneMiddlePoints[i];
            ZoneBounds[i] = new Bounds(new Vector3(middle.x + ZoneSizes[i].x / 2, middle.y + ZoneSizes[i].y / 2, 0),
                ZoneSizes[i]);
            Points[i] = new Dictionary<int, int>(){{1,0},{2,0},{3,0},{4,0}};
            
        }
    }

    public void UpdateTileZones(int id, Vector3 tilepos, Color previous)
    {
        for (int i = 0; i < ZoneBounds.Count; i++)
        {
            if (ZoneBounds[i].Contains(tilepos))
            {
                // tile is in zone i
                Points[i][id] += 1;
                Debug.Log(i);
                
                // check if previous color is from a player => reduce points
                if (previous != Color.white)
                {
                    
                }
                // update the zone
                
            }
            
        }
    }
    
    private void GetTiles(Tilemap map, BoundsInt bounds)
    {
        
        TileBase[] allTiles = tm.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    // you may not need to find gridPlace, in which case cut next line
                    Vector3Int gridPlace = new Vector3Int(
                        x + bounds.xMin, y + bounds.yMin, bounds.z);
                    Vector3 worldPlace = map.CellToWorld(gridPlace);
                    // do something
                    Vector3Int worldCoords = Vector3Int.FloorToInt(worldPlace);
                    tm.SetTileFlags(worldCoords, TileFlags.None);
                    tm.SetColor(worldCoords, Color.red);
                    tm.RefreshTile(worldCoords);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
