
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameTimer : NetworkBehaviour
{
    public TMP_Text timerText;
    private float timeRemaining; // No need to set here, as we will set it when the second player joins
    private bool timerIsRunning = false;
    public GameObject ball; // Assign the ball GameObject in the inspector
    public PlayerRespawner playerRespawner;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        // Initialize timer for 3 minutes when the second player joins
        timeRemaining = 180f;
        playerRespawner.pi();
        
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer){
        // Check if this is the second player joining
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            timerIsRunning = true; // Start the timer
        }
        }
    }

    void Update()
    {
        if (timerIsRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else if (timerIsRunning)
        {
            EndGame();
            timerIsRunning = false; // Stop the timer
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndGame()
{
    // Log out players and remove the ball
    NetworkManager.Singleton.Shutdown();

    if (ball != null)
    {
        Destroy(ball); // This will remove the ball from the scene
    }

    // Optionally, you can also load a different scene or show a game over screen here.


        // Optionally, you can also load a different scene or show a game over screen here.
    }
}
