using UnityEngine;


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


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void HideTile() => gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
}
