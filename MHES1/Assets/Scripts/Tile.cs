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
        ApplyBiomeColor();

        if (Terrain == TerrainType.Water) isWalkable = false;
        else isWalkable = true; 
    }

    void ApplyBiomeColor()
    {
        Renderer rend = GetComponent<Renderer>();
        if (!rend) return;

        Color color = Color.gray;

        switch (Terrain)
        {
            case TerrainType.Water: color = Color.blue; break;
            case TerrainType.Forest: color = Color.green; break;
            case TerrainType.Desert: color = Color.yellow; break;
            case TerrainType.Swamp: color = new Color(0.2f, 0.4f, 0.2f); break;
            case TerrainType.Mountain: color = Color.grey; break;
            case TerrainType.Plains: color = new Color(0.5f, 1f, 0.5f); break;
            case TerrainType.Savanna: color = new Color(0.8f, 0.7f, 0.2f); break;
            case TerrainType.Tundra: color = new Color(0.9f, 1f, 1f); break;
            case TerrainType.Taiga: color = new Color(0.3f, 0.6f, 0.3f); break;
            case TerrainType.SnowForest: color = Color.white; break;
            case TerrainType.Rainforest: color = new Color(0.1f, 0.5f, 0.1f); break;
        }

        rend.material.color = color;
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

    void OnDrawGizmos()
    {
        Gizmos.color = IsOccupied() ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.1f, Vector3.one * 0.9f);

        #if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, GridCoordinates.ToString());
        #endif
    }
}