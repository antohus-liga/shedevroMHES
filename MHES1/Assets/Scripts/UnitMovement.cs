using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool isSelected = false;
    public GameObject selectionCircle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

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