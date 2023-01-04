using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PathDrawer;

/* Prototype : GetFocusedOnTile
 *   This function uses a raycast to detect which tile the mouse is currently hovering over.
 *   If a tile is found, the function returns the RaycastHit2D object for that tile. 
 *   Otherwise, it returns null
 * 
 * Prototype : PositionCharacterOnLine
 *   This function positions the character on top of a specified tile.
 *   It sets the character's standingOnTile property to the tile that it is standing on.
 * 
 * Protoype : MoveAlongPath
 *   This function moves the character along a specified path. 
 *   It calculates the character's position at each step along the path and
 *   updates the character's position accordingly.
 * 
 * Prototype : GetInRangeTiles
 *   This function uses the RangeFinder to find all the tiles within the character's range, 
 *   and sets the rangeFinderTiles variable to the list of tiles that were found.
 * 
 * */



public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float speed;
    public GameObject characterPrefab;
    private Character character;
    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private List<VisualizeTile> path;
    private List<VisualizeTile> rangeFinderTiles;
    private bool isMoving;
    private PathDrawer pathDrawer;
    public GameObject mechant;
    public GameObject gentil;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mechant = GameObject.FindGameObjectWithTag("Ennemies");
        GameObject gentil = GameObject.FindGameObjectWithTag("Heroes");
        pathFinder = new PathFinder();
        path = new List<VisualizeTile>();
        rangeFinder = new RangeFinder();
        pathDrawer = new PathDrawer();
        isMoving = false;
        rangeFinderTiles = new List<VisualizeTile>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            VisualizeTile tile = hit.Value.collider.gameObject.GetComponent<VisualizeTile>();
            cursor.transform.position = tile.transform.position;
            cursor.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (rangeFinderTiles.Contains(tile) && !isMoving)
            {
                path = pathFinder.FindPath(character.standingOnTile, tile, rangeFinderTiles);

                foreach (var item in rangeFinderTiles)
                {
                    MapContainer.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                }

                for (int i = 0; i < path.Count; i++)
                {
                    var previousTile = i > 0 ? path[i - 1] : character.standingOnTile;
                    var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                    var arrow = pathDrawer.TranslateDirection(previousTile, path[i], futureTile);
                    path[i].SetSprite(arrow); //***
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //tile.ShowTile();

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<Character>();
                    PositionCharacterOnLine(tile);
                    GetInRangeTiles();
                }
                else if (path.Count == 0)
                {
                    isMoving = false;
                    if (mechant.activeSelf && !gentil.activeSelf)
                    {
                        gentil.SetActive(true);
                        mechant.SetActive(false);
                    }
                    else if (gentil.activeSelf && !mechant.activeSelf)
                    {
                        gentil.SetActive(false);
                        mechant.SetActive(true);
                    }
                }
                else
                {
                    isMoving = true;
                    tile.gameObject.GetComponent<VisualizeTile>().HideTile();
                }
            }
        }

        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }

        /*if (path.Count == 0 && Input.GetMouseButtonDown(0))
        {
            isMoving = false;
            if (mechant.activeSelf && !gentil.activeSelf)
            {
                gentil.SetActive(true);
                mechant.SetActive(false);
                GetInRangeTiles();
            }
            else if (gentil.activeSelf && !mechant.activeSelf)
            {
                gentil.SetActive(false);
                mechant.SetActive(true);
                GetInRangeTiles();
            }
        }

        if (path.Count == 0)
        {

        }*/


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
        /*var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);*/
        var step = speed * Time.deltaTime;
        float zIndex = path[0].transform.position.z;
        Vector3 targetPos = path[0].transform.position;
        // Only move in the horizontal or vertical direction, depending on which brings the character closer to the target tile
        if (Mathf.Abs(character.transform.position.x - targetPos.x) > Mathf.Abs(character.transform.position.y - targetPos.y))
        {
            targetPos.y = character.transform.position.y;
        }
        else
        {
            targetPos.x = character.transform.position.x;
        }
        character.transform.position = Vector2.MoveTowards(character.transform.position, targetPos, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);
        /*Makes the guy giggle giggle, add some ugly anim when going up and down | UP: doesn't function properly when the project is build and creates poor perf*/
        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
            isMoving = false;
        }
    }

    private void GetInRangeTiles()
    {
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), 3);//here is the range

        foreach (var item in rangeFinderTiles)
        {
            item.ShowTile();
        }
    }
}