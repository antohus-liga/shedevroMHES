using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int GridCoordinates = new Vector2Int(0, 0);
    public Unit OccupierUnit { get; private set; }
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
    public Tile Parent;

    void Awake()
    {
        GridManager.Instance.RegisterTile(this);
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