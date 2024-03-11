using System;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class gol2 : NetworkBehaviour
{
    private NetworkVariable<int> score = new NetworkVariable<int>();
    public Ball ball;
    public PlayerRespawner playerRespawner;
    public TextMeshProUGUI scoreText;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            score.Value = 0;
            UpdateScoreClientRpc(score.Value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //ball.ResetOwnershipToHostServerRpc();
            
            IncrementScoreServerRpc();
            ball.Respawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncrementScoreServerRpc()
    {
       if (ball.Respawn()){
        score.Value += 1;
       
        }
        //playerRespawner.RespawnPlayersAfterGoal();
    
        UpdateScoreClientRpc(score.Value);
        
    } 

    [ClientRpc]
    private void UpdateScoreClientRpc(int newScore)
    {
        scoreText.text = "player2:-" + newScore.ToString();
      
       // ball.Respawn();
        playerRespawner.RespawnPlayersAfterGoal();


    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetScoreServerRpc()
    {
        score.Value = 0;

        UpdateScoreClientRpc(score.Value);
    }
   
}
