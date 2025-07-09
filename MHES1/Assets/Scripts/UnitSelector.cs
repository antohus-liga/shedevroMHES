using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public Unit selectedUnit;

    void Update()
    {
        // Left-click to select unit
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Unit unit = hit.collider.GetComponent<Unit>();

                if (unit != null)
                {
                    // Deselect previously selected unit
                    if (selectedUnit != null && selectedUnit != unit)
                        selectedUnit.SetSelected(false);

                    selectedUnit = unit;
                    selectedUnit.SetSelected(true);
                }
                else
                {
                    // Clicked on non-unit: deselect
                    if (selectedUnit != null)
                    {
                        selectedUnit.SetSelected(false);
                        selectedUnit = null;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                    selectedUnit.MoveTo(tile);
            }
        }
    }
}