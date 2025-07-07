using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    [SerializeField] private GameObject selectionCircle;

    private NavMeshAgent agent;
    private bool isSelected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats.currentHealth = stats.maxHealth;

        if (selectionCircle != null)
            selectionCircle.SetActive(false);

        agent.speed = stats.moveSpeed;
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
