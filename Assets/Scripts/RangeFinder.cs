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
