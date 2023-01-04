using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Prototype : GetFocusedOnTile
 * 
 * 
 * Prototype : PositionCharacterOnLine
 * 
 * 
 * Protoype : private void MoveAlongPath
 * 
 * 
 * */


public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float speed;
    public GameObject characterPrefab;

    private Character character;
    private PathFinder pathFinder;
    private List<VisualizeTile> path;
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new PathFinder();
        path = new List<VisualizeTile>();
    }

// Update is called once per frame
void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            VisualizeTile tile = hit.Value.collider.gameObject.GetComponent<VisualizeTile>();
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
            if (Input.GetMouseButtonDown(0))
            {
                tile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<Character>();
                    PositionCharacterOnLine(tile);
                    character.standingOnTile = tile;
                }
                else
                {
                    path = pathFinder.FindPath(character.standingOnTile, tile);

                    tile.gameObject.GetComponent<VisualizeTile>().HideTile();
                }
            }
        }

        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

        private void PositionCharacterOnLine(VisualizeTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.standingOnTile = tile;
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }
    }
}