using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Prototype : GetSurroundingTiles
 *  This function takes in a Vector2Int representing the location of a tile, and returns a list of tiles that are adjacent to the specified tile.
 *  The function checks each tile within a 3x3 grid centered on the specified tile, 
 *  and adds any tiles that are adjacent to the specified tile (i.e., tiles that share a side with the specified tile) to the list of surrounding tiles.
 * 
 * */

public class MapContainer : MonoBehaviour
{
    public static MapContainer Instance { get; private set; }

    public GameObject overlayPrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, VisualizeTile> map;
    public bool ignoreBottomTiles;

    private void Awake()
    {
        if (Instance == null || Instance == this)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        map = new Dictionary<Vector2Int, VisualizeTile>();

        foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;

            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        if (z == 0 && ignoreBottomTiles)
                            return;

                        if (tm.HasTile(new Vector3Int(x, y, z)))
                        {
                            if (!map.ContainsKey(new Vector2Int(x, y)))
                            {
                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder;
                                overlayTile.GetComponent<VisualizeTile>().gridLocation = new Vector3Int(x, y, z);

                                map.Add(new Vector2Int(x, y), overlayTile.GetComponent<VisualizeTile>());
                            }
                        }
                    }
                }
            }
        }
    }
  
    public List<VisualizeTile> GetSurroundingTiles(Vector2Int originTile)
    {
        var surroundingTiles = new List<VisualizeTile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == y || x == -y)
                    continue;

                Vector2Int tileToCheck = new Vector2Int(originTile.x + x, originTile.y + y);
                if (map.ContainsKey(tileToCheck) && Mathf.Abs(map[tileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(map[tileToCheck]);
            }
        }

        return surroundingTiles;
    }
  
}

