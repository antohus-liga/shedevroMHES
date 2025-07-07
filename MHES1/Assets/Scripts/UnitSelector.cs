using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    private Unit selectedUnit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Unit unit = hit.collider.GetComponent<Unit>();

                if (unit != null)
                {
                    // Deselect current unit (if different)
                    if (selectedUnit != null && selectedUnit != unit)
                        selectedUnit.SetSelected(false);

                    // Select new unit
                    selectedUnit = unit;
                    selectedUnit.SetSelected(true);
                }
                else
                {
                    // Clicked something else — deselect
                    DeselectCurrent();
                }
            }
            else
            {
                // Clicked empty space — deselect
                DeselectCurrent();
            }
        }

        // Right-click to move
        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                selectedUnit.MoveTo(hit.point);
            }
        }
    }

    void DeselectCurrent()
    {
        if (selectedUnit != null)
        {
            selectedUnit.SetSelected(false);
            selectedUnit = null;
        }
    }
}