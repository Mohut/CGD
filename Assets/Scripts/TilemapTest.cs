using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tilemap tm;
    void Start()
    {
        StartCoroutine(colos());
    }

    IEnumerator colos()
    {
        yield return new WaitForSeconds(2);
        BoundsInt bounds = new BoundsInt(new Vector3Int(3, 3, 0), new Vector3Int(3, 3, 1));
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
                    Vector3 worldPlace = tm.CellToWorld(gridPlace);
                    // do something
                    Vector3Int worldCoords = Vector3Int.FloorToInt(worldPlace);
                    
                    Debug.Log(worldCoords);
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
