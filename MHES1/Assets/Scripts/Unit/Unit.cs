using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject selectionCircle;

    public int ownerPlayerID;
    public UnitStats stats;
    public Tile currentTile;

    private float moveSpeed = 5f;
    private bool isSelected = false;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Coroutine movementCoroutine;

    void Start()
    {

        currentTile = GridManager.Instance.GetTileFromWorldPosition(transform.position);

        if (currentTile != null)
        {
            currentTile.SetOccupiedUnit(this);

            Vector3 tilePos = currentTile.transform.position;
            transform.position = new Vector3(tilePos.x, tilePos.y + 1.5f, tilePos.z);
        }
        else
        {
            Debug.LogWarning($"No tile found under {gameObject.name}, check unit position.");
        }

        targetPosition = transform.position;
        stats.currentHealth = stats.maxHealth;

        if (selectionCircle != null)
            selectionCircle.SetActive(false);

        Renderer r = GetComponentInChildren<Renderer>();
        switch (ownerPlayerID)
        {
            case 0: r.material.color = Color.red; break;
            case 1: r.material.color = Color.blue; break;
        }
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

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (selectionCircle != null)
            selectionCircle.SetActive(value);
    }

    public void MoveAlongPath(List<Tile> path)
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(MoveStepByStep(path));
    }

    private IEnumerator MoveStepByStep(List<Tile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Tile nextTile = path[i];

            Vector3 targetPos = nextTile.transform.position;
            targetPos.y = 1.5f;

            // Move toward the tile
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;

            // Only after arriving: update occupancy
            if (currentTile != null && currentTile != nextTile)
                currentTile.ClearOccupiedUnit();

            currentTile = nextTile;
            currentTile.SetOccupiedUnit(this);
        }

        movementCoroutine = null;
    }

    private bool IsEnemyTo(Unit other)
    {
        return ownerPlayerID != other.ownerPlayerID;
    }

    public bool IsAdjacentTo(Unit other)
    {
        Vector2Int myPos = currentTile.GridCoordinates;
        Vector2Int otherPos = other.currentTile.GridCoordinates;

        return Mathf.Abs(myPos.x - otherPos.x) + Mathf.Abs(myPos.y - otherPos.y) == 1;
    }

    private void AttackEnemyUnit(Unit enemy)
    {
        enemy.stats.currentHealth -= Mathf.RoundToInt(stats.damage - stats.damage * enemy.stats.defense * 0.3f);

        if (enemy.stats.currentHealth < 0)
        {
            enemy.currentTile.ClearOccupiedUnit();
            Destroy(enemy.gameObject);
        }
    }

    public void TryAttack(Unit target)
    {
        if (target == null) return;

        if (IsEnemyTo(target) && IsAdjacentTo(target))
        {
            Debug.Log($"{gameObject.name} attacks {target.name}!");

            AttackEnemyUnit(target); // Example method
        }
        else
        {
            Debug.Log("Target is out of range or not an enemy.");
        }
    }
}