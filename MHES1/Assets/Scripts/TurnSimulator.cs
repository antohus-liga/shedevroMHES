using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("Turn Control")]
    public int currentPlayerID = 0;
    public int totalPlayers = 2;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void NextTurn()
    {
        currentPlayerID = (currentPlayerID + 1) % totalPlayers;
        Debug.Log($"Turn switched to player {currentPlayerID}");
    }

    public bool IsCurrentPlayersTurn(int playerID)
    {
        return currentPlayerID == playerID;
    }
}