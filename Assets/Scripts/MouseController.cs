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

public enum CursorClickType
{
    Move,
    Attack,
    None
}

public class MouseController : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private GameplayHelper gameplayHelper;

    private Character character;
    private PathFinder pathFinder;
    private List<VisualizeTile> path;
    
    void OnEnable()
    {
        pathFinder = new PathFinder();
        if (path == null) path = new List<VisualizeTile>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        VisualizeTile tile = GetFocusedOnTile(mousePos2d);

        if (tile != null)
        {
            transform.position = tile.transform.position;
            GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
            if (Input.GetMouseButtonDown(0))
            {
                switch (gameplayHelper.CursorClick())
                {
                    case CursorClickType.Move:
                        path = pathFinder.FindPath(gameplayHelper.GetCurrentCharacter().standingOnTile, tile);
                        break;
                    case CursorClickType.Attack:
                        gameplayHelper.Attack(tile);
                        break;
                    case CursorClickType.None:
                        break;
                }
            }
        }

        if (path.Count > 0)
        {
            MoveAlongPath(gameplayHelper.GetCurrentCharacter());
        }
    }

    public void MoveTo(Character character, Vector2 position)
    {
        path = pathFinder.FindPath(GetFocusedOnTile(position-Vector2.down), GetFocusedOnTile(position));
        PositionCharacterOnLine(character, GetFocusedOnTile(position));
    }

    private VisualizeTile GetFocusedOnTile(Vector2 mousePos2D)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        Debug.DrawRay(mousePos2D, Vector2.zero, Color.red);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First().collider.gameObject.GetComponent<VisualizeTile>();
        }

        return null;
    }

    private void PositionCharacterOnLine(Character character, VisualizeTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        // character.GetComponentInChildren<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.standingOnTile = tile;
    }

    private void MoveAlongPath(Character character)
    {
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(character, path[0]);
            path.RemoveAt(0);
        }
    }
}