using System.Collections.Generic;
using UnityEngine;
using static PathDrawer;


/* Prototype : HideTile
     * 
     * 
     * */
// Update is called once per frame


public class VisualizeTile : MonoBehaviour
{
    public int G;
    public int H;
    public int F { get { return G + H; } }

    public bool isBlocked = false;
    public VisualizeTile Previous;
    public Vector3Int gridLocation;

    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
    public List<Sprite> arrows;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void HideTile() => gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

    public void ShowTile() => gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

    public void SetSprite(ArrowDirection d)
    {
        if (d == ArrowDirection.None)
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        else
        {
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
            GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)d];
            GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }

}

