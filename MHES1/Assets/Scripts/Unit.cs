using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject selectionCircle;
    public UnitStats stats;
    public float moveSpeed = 5f;
    private bool isSelected = false;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;
        stats.currentHealth = stats.maxHealth;

        if (selectionCircle != null)
            selectionCircle.SetActive(false);
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                isMoving = false;
        }
    }

    public void MoveTo(Tile tile)
    {
        if (tile == null) return;

        targetPosition = tile.transform.position;
        targetPosition.y = 1;
        isMoving = true;
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (selectionCircle != null)
            selectionCircle.SetActive(value);
    }
}
