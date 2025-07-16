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
    private Tile reservedTile = null;

    void Start()
    {

        currentTile = GridManager.Instance.GetTileFromWorldPosition(transform.position);

        if (currentTile != null)
        {
            currentTile.SetOccupiedUnit(this);

            Vector3 tilePos = currentTile.transform.position;
            transform.position = new Vector3(tilePos.x, tilePos.y + 1f, tilePos.z);
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
        // Stop any movement in progress
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }

        // Start new movement
        movementCoroutine = StartCoroutine(MoveStepByStep(path));
    }

    private IEnumerator MoveStepByStep(List<Tile> path)
    {
        // Cancel previous reservation if any
        if (reservedTile != null && reservedTile != currentTile)
        {
            reservedTile.ClearOccupiedUnit();
            reservedTile = null;
        }

        Tile finalTile;
        // Reserve the destination tile
        if (path.Count > 0)
        {
            finalTile = path[^1];
        }
        else
        {
            finalTile = currentTile;
        }

        finalTile.SetOccupiedUnit(this);
        reservedTile = finalTile;

        // Move through the path
        foreach (Tile tile in path)
        {
            Vector3 targetPos = tile.transform.position;
            targetPos.y = 1f;

            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;

            // Update current tile only after arrival
            if (currentTile != null)
                currentTile.ClearOccupiedUnit();

            currentTile = tile;
        }

        // Arrived at final destination, so reservation is fulfilled
        reservedTile = null;
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