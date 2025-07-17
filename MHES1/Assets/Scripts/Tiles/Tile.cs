using UnityEngine;

public class Tile : MonoBehaviour
{
    public TerrainType Terrain;
    public float Temperature;
    public float Moisture;
    public float Elevation;
    public bool isWalkable;
    public Vector2Int GridCoordinates = new Vector2Int(0, 0);
    public Unit OccupierUnit { get; private set; }
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
    public Tile Parent;

    void Awake()
    {
        GridManager.Instance.RegisterTile(this);

        if (Terrain == TerrainType.Water) isWalkable = false;
        else isWalkable = true; 
    }

    public bool IsOccupied()
    {
        return OccupierUnit != null;
    }

    public void SetOccupiedUnit(Unit unit)
    {
        OccupierUnit = unit;
    }

    public void ClearOccupiedUnit()
    {
        OccupierUnit = null;
    }
}