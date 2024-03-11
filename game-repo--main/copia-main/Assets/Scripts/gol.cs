using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;


public class gol : NetworkBehaviour
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
        UpdateScoreClientRpc(score.Value);
       
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int newScore)
    {
        scoreText.text = "playerOne:-" + newScore.ToString();
        
        //ball.Respawn();
        playerRespawner.RespawnPlayersAfterGoal();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetScoreServerRpc()
    {
        score.Value = 0;
        UpdateScoreClientRpc(score.Value);
    }
   
}



    /*
    private void OnTriggerEnter(Collider other)
{
 
    if (other.CompareTag("Ball"))
    {
       
       
            // goal scored
            //score++;
            score.Value = score.Value + 1;
            ball.Respawn();
            playerRespawner.RespawnPlayersAfterGoal();
        UpdateScoreText(); 
    }
    

}
public void UpdateScoreText()
    {
        scoreText.text = "playerOne:-" + score.Value.ToString();
    }

public void reset()
    {
        score.Value = 0;
        UpdateScoreText(); 
    }

*/

