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

    public Tile GetTileFromWorldPosition(Vector3 worldPos)
    {
        Vector2Int coords = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));
        return GetTileAt(coords);
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

    public List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        };

        foreach (var dir in directions)
        {
            Vector2Int checkPos = Vector2Int.RoundToInt(tile.GridCoordinates + dir);
            Tile neighbour = GetTileAt(checkPos);
            if (neighbour != null)
                neighbours.Add(neighbour);
        }

        return neighbours;
    }
}