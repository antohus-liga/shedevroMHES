using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Tile> FindPath(Tile start, Tile target)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Tile current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < current.FCost ||
                   (openSet[i].FCost == current.FCost && openSet[i].HCost < current.HCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == target)
                return RetracePath(start, target);

            foreach (Tile neighbour in GridManager.Instance.GetNeighbours(current))
            {
                if (neighbour.IsOccupied() || closedSet.Contains(neighbour))
                    continue;

                int newGCost = current.GCost + GetDistance(current, neighbour);
                if (newGCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newGCost;
                    neighbour.HCost = GetDistance(neighbour, target);
                    neighbour.Parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null; // no path
    }

    private List<Tile> RetracePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Tile a, Tile b)
    {
        int dstX = Mathf.Abs((int)(a.GridCoordinates.x - b.GridCoordinates.x));
        int dstY = Mathf.Abs((int)(a.GridCoordinates.y - b.GridCoordinates.y));
        return dstX + dstY;
    }
}