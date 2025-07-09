using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterTile(Tile tile)
    {
        Vector2Int coords = Vector2Int.RoundToInt(tile.GridCoordinates);
        if (!tiles.ContainsKey(coords))
            tiles.Add(coords, tile);
    }

    public Tile GetTileAt(Vector2Int coords)
    {
        tiles.TryGetValue(coords, out Tile tile);
        return tile;
    }

    public Tile GetTileAtWorldPosition(Vector3 worldPos)
    {
        Tile closest = null;
        float minDist = Mathf.Infinity;

        foreach (Tile tile in tiles.Values)
        {
            float dist = Vector3.Distance(tile.transform.position, worldPos);
            if (dist < minDist)
            {
                closest = tile;
                minDist = dist;
            }
        }

        return closest;
    }
}