using UnityEngine;

public class PathDrawer
{
    public enum ArrowDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopLeft = 5,
        BottomLeft = 6,
        TopRight = 7,
        BottomRight = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12
    }
    
    public ArrowDirection TranslateDirection(VisualizeTile previousTile, VisualizeTile currentTile, VisualizeTile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null ? (Vector2Int)(currentTile.gridLocation - previousTile.gridLocation) : new Vector2Int(0, 0);
        Vector2Int futureDirection = futureTile != null ? (Vector2Int)(futureTile.gridLocation - currentTile.gridLocation) : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if (direction == new Vector2Int(0, 1))
        {
            return isFinal ? ArrowDirection.UpFinished : ArrowDirection.Up;
        }

        if (direction == new Vector2Int(0, -1))
        {
            return isFinal ? ArrowDirection.DownFinished : ArrowDirection.Down;
        }

        if (direction == new Vector2Int(1, 0))
        {
            return isFinal ? ArrowDirection.RightFinished : ArrowDirection.Right;
        }

        if (direction == new Vector2Int(-1, 0))
        {
            return isFinal ? ArrowDirection.LeftFinished : ArrowDirection.Left;
        }

        if (direction == new Vector2Int(1, 1))
        {
            return pastDirection.y < futureDirection.y ? ArrowDirection.BottomLeft : ArrowDirection.TopRight;
        }

        if (direction == new Vector2Int(-1, 1))
        {
            return pastDirection.y < futureDirection.y ? ArrowDirection.BottomRight : ArrowDirection.TopLeft;
        }

        if (direction == new Vector2Int(1, -1))
        {
            return pastDirection.y > futureDirection.y ? ArrowDirection.TopLeft : ArrowDirection.BottomRight;
        }

        if (direction == new Vector2Int(-1, -1))
        {
            return pastDirection.y > futureDirection.y ? ArrowDirection.TopRight : ArrowDirection.BottomLeft;
        }

        return ArrowDirection.None;
    }
    
}

