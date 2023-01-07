using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapContainer : MonoBehaviour
{
    public static MapContainer Singleton { get; private set; }

    public GameObject overlayPrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, VisualizeTile> map;
    public bool ignoreBottomTiles;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
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

                        if (tm.HasTile(new Vector3Int(x, y, z)) && !map.ContainsKey(new Vector2Int(x, y)))
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