using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*  Prototype : FindPath
     *  This method finds a path between the start and end points, using the A* pathfinding algorithm. 
     *  The path can only pass through tiles in the inRangeTiles list, if it is not empty. 
     *  The method returns a list of VisualizeTile objects representing the path from the start point to the end point.
 *  
 *  Prototype : GetFinishedList
    *  This method traces back the path from the end point to the start point by following the Previous references of each tile and returns the path in reverse order. 
    *  The method takes in the start and end points as VisualizeTile objects and returns a list of VisualizeTile objects representing the path.
 *  
 *  Prototype : GetManhattenDistance
    *  This method calculates the Manhattan distance between two tiles. 
    *  It takes in the start and end points as VisualizeTile objects and returns the Manhattan distance as an integer.
 *  
 *  Prototype : GetNeightbourOverlayTiles
    * This method returns a list of the current tile's neighbors by considering the tiles in the four cardinal directions of the current tile. 
    * It takes in the current tile as a VisualizeTile object and returns a list of VisualizeTile objects representing the neighbors. 
 *  
 * 
 * */

public class PathFinder
{
    private Dictionary<Vector2Int, VisualizeTile> searchableTiles;

    public List<VisualizeTile> FindPath(VisualizeTile start, VisualizeTile end, List<VisualizeTile> inRangeTiles)
    {
        searchableTiles = inRangeTiles.Count > 0 ? inRangeTiles.ToDictionary(tile => tile.grid2DLocation, tile => MapContainer.Instance.map[tile.grid2DLocation]) : MapContainer.Instance.map; /*
                                                                                                                                                                                                */

        List<VisualizeTile> openList = new List<VisualizeTile> { start };
        HashSet<VisualizeTile> closedList = new HashSet<VisualizeTile>();

        while (openList.Count > 0)
        {
            VisualizeTile currentOverlayTile = openList.OrderBy(tile => tile.F).First();

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

    private static readonly int[][] directions = {
    new int[] { 1, 0 }, // right
    new int[] { -1, 0 }, // left
    new int[] { 0, 1 }, // top
    new int[] { 0, -1 } // bottom
};

    private List<VisualizeTile> GetNeightbourOverlayTiles(VisualizeTile currentOverlayTile)
    {
        var map = MapContainer.Instance.map;

        // Pre-allocate a list to store the neighbours
        var neighbours = new List<VisualizeTile>(4);

        // Use a single locationToCheck variable and update its values
        var locationToCheck = new Vector2Int();

        // Iterate over the directions using a for loop
        for (int i = 0; i < directions.Length; i++)
        {
            locationToCheck.x = currentOverlayTile.gridLocation.x + directions[i][0];
            locationToCheck.y = currentOverlayTile.gridLocation.y + directions[i][1];

            // Check if the map contains the locationToCheck
            if (map.ContainsKey(locationToCheck))
            {
                neighbours.Add(map[locationToCheck]);
            }
        }

        return neighbours;
    }
}


/*The A* algorithm works by maintaining two lists of tiles: an "open" list of tiles that are still being considered as part of the path,
 * and a "closed" list of tiles that have already been visited. 
 * The algorithm begins by adding the start tile to the open list and iterates through each tile in the open list until it finds the end tile or the open list is empty. 
 * For each tile in the open list, the A* algorithm calculates the cost of moving to that tile and the estimated distance to the end tile,
 * and adds the tile to the closed list and removes it from the open list. If the current tile is the end tile, 
 * the algorithm returns the path by following the "previous" property of each tile.
 * If the end tile is not found, 
 * the algorithm loops through each of the current tile's neighbors and adds them to the open list if they are not blocked or already in the closed list.*/