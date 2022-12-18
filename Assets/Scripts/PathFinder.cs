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

/*This is a C# script that defines a PathFinder class, which contains a method for finding a path between two tiles in a grid-based map. The FindPath method uses the A* pathfinding algorithm to find the shortest path between two tiles, represented by the start and end parameters.
 * 
 * The FindPath method begins by initializing two lists, openList and closedList, which will be used to keep track of which tiles have been visited or are still being considered as part of the path. The openList starts off containing only the start tile, and the algorithm iterates through each tile in the openList until it finds the end tile or the openList is empty.
 * 
 * For each tile in the openList, the FindPath method calculates its G, H, and F values, which are used by the A* algorithm to determine the cost of moving to that tile and the estimated distance to the end tile. The G value is the actual distance from the start tile to the current tile, while the H value is the estimated distance from the current tile to the end tile. The F value is the sum of the G and H values, and it is used to determine which tile in the openList is the most promising to visit next.
 * 
 * The FindPath method then adds the current tile to the closedList and removes it from the openList. If the current tile is the end tile, the method returns a list of tiles representing the path from the start tile to the end tile by calling the GetFinishedList method. If the end tile is not found, the method loops through each of the current tile's neighbors (tiles that are adjacent to it on the map) and adds them to the openList if they are not blocked or already in the closedList.
 * 
 * The GetFinishedList method takes the start and end tiles as arguments and returns a list of tiles representing the path from the start tile to the end tile by following the Previous property of each tile, which was set by the FindPath method. 
 * 
 * The GetManhattenDistance method calculates the distance between two tiles using the Manhattan distance formula, which is the sum of the absolute differences of their x and y coordinates. 
 * 
 * The GetNeightbourOverlayTiles method returns a list of the current tile's neighbors by checking the positions of the tiles in the map.
*/

/*The A* algorithm works by maintaining two lists of tiles: an "open" list of tiles that are still being considered as part of the path,
 * and a "closed" list of tiles that have already been visited. 
 * The algorithm begins by adding the start tile to the open list and iterates through each tile in the open list until it finds the end tile or the open list is empty. 
 * For each tile in the open list, the A* algorithm calculates the cost of moving to that tile and the estimated distance to the end tile,
 * and adds the tile to the closed list and removes it from the open list. If the current tile is the end tile, the algorithm returns the path by following the "previous" property of each tile.
 * If the end tile is not found, the algorithm loops through each of the current tile's neighbors and adds them to the open list if they are not blocked or already in the closed list.*/