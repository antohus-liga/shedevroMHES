using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 GridCoordinates = new Vector2(0f, 0f);
    //public float moveCost;

    private void Start()
    {
        GridManager.Instance?.RegisterTile(this);
    }
}