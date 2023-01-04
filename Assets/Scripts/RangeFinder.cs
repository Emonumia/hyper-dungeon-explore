using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangeFinder
{
    public List<VisualizeTile> GetTilesInRange(Vector2Int location, int range)
        {
            var startingTile = MapContainer.Instance.map[location];
            var inRangeTiles = new List<VisualizeTile>();
            int stepCount = 0;

            inRangeTiles.Add(startingTile);

            //Should contain the surroundingTiles of the previous step. 
            var tilesForPreviousStep = new List<VisualizeTile>();
            tilesForPreviousStep.Add(startingTile);
            while (stepCount < range)
            {
                var surroundingTiles = new List<VisualizeTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(MapContainer.Instance.GetSurroundingTiles(new Vector2Int(item.gridLocation.x, item.gridLocation.y)));
                }

                inRangeTiles.AddRange(surroundingTiles);
                tilesForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return inRangeTiles.Distinct().ToList();
        }
}
/*The GetTilesInRange function takes in a location parameter, 
 * which is a Vector2Int representing the location to find tiles around, 
 * and a range parameter, which is an integer representing the number of steps away from the location to search for tiles.
 * 
 * The function starts by adding the tile at the specified location to a list of tiles within range, 
 * and setting a stepCount variable to 0. It then enters a loop that runs until the stepCount reaches the specified range.
 * 
 * Inside the loop, the function gets a list of tiles that are adjacent to the tiles found in the previous step, 
 * and adds them to the list of tiles within range. It then sets the tilesForPreviousStep variable to the list of tiles found in this step, 
 * and increments the stepCount variable. 
 * 
 * The function then returns the list of tiles within range, after removing any duplicates.*/