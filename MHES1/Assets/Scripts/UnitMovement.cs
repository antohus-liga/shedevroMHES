using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovement : MonoBehaviour
{
    [SerializeField] private GameObject selectionCircle;

    private NavMeshAgent agent;
    private UnitStatsComponent unitStats;
    private bool isSelected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitStats = GetComponent<UnitStatsComponent>();

        agent.speed = unitStats.stats.moveSpeed;

        if (selectionCircle != null)
            selectionCircle.SetActive(false);
    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (selectionCircle != null)
            selectionCircle.SetActive(value);
    }
}