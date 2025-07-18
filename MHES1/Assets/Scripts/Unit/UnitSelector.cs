using UnityEngine;
using System.Collections.Generic;

public class UnitSelector : MonoBehaviour
{
    public Unit selectedUnit;
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private UnitInfoUI infoUI;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TurnManager.Instance.NextTurn();
        }

        // Left-click to select unit
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, unitLayer))
            {
                Unit clickedUnit = hit.collider.GetComponent<Unit>();
                if (clickedUnit != null)
                {
                    // Deselect previous
                    if (selectedUnit != null)
                        selectedUnit.SetSelected(false);

                    selectedUnit = clickedUnit;
                    selectedUnit.SetSelected(true);

                    // Show info regardless of ownership
                    infoUI.Show(clickedUnit);
                }
            }
            else
            {
                // Clicked empty space
                if (selectedUnit != null)
                {
                    selectedUnit.SetSelected(false);
                    selectedUnit = null;
                }
                infoUI.Hide();
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int unitLayer = LayerMask.GetMask("Units");
            int tileLayer = ~unitLayer; // Invert the mask to hit everything *except* units

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null && selectedUnit != null)
                {
                    if (tile.IsOccupied())
                    {
                        selectedUnit.TryAttack(tile.OccupierUnit);
                    }
                    else
                    {
                        List<Tile> path = Pathfinding.Instance.FindPath(selectedUnit.currentTile, tile);
                        if (path != null && IsSelectedUnitMine())
                            selectedUnit.MoveAlongPath(path);
                    }
                }
            }
        }

    }
    public bool IsSelectedUnitMine()
    {
        return selectedUnit != null && TurnManager.Instance.IsCurrentPlayersTurn(selectedUnit.ownerPlayerID);
    }
}