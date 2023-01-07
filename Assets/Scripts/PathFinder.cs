using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*  Prototype : FindPath
 *  
 *  
 *  Prototype : GetFinishedList
 *  
 *  
 *  Prototype : GetManhattenDistance
 *  
 *  
 *  Prototype : GetNeightbourOverlayTiles
 *  
 *  
 * 
 * */

public class PathFinder
{
    public List<VisualizeTile> FindPath(VisualizeTile start, VisualizeTile end)
    {
        List<VisualizeTile> openList = new List<VisualizeTile>();
        List<VisualizeTile> closedList = new List<VisualizeTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            VisualizeTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end);
            }

            foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
            {
                if (tile.isBlocked || closedList.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                {
                    continue;
                }

                tile.G = GetManhattenDistance(start, tile);
                tile.H = GetManhattenDistance(end, tile);

                tile.Previous = currentOverlayTile;


                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<VisualizeTile>();
    }

    private List<VisualizeTile> GetFinishedList(VisualizeTile start, VisualizeTile end)
    {
        List<VisualizeTile> finishedList = new List<VisualizeTile>();
        VisualizeTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(VisualizeTile start, VisualizeTile tile)
    {
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
    }

    private List<VisualizeTile> GetNeightbourOverlayTiles(VisualizeTile currentOverlayTile)
    {
        var map = MapContainer.Singleton.map;

        List<VisualizeTile> neighbours = new List<VisualizeTile>();

        //right
        Vector2Int locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //top
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y + 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y - 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
}
