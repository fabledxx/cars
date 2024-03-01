using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;


public class gol : NetworkBehaviour
{
    public int score;
    public Ball ball;
    //public PlayerRespawner playerRespawner;
     public TextMeshProUGUI scoreText; // Use public Text scoreText; for standard UI Text

     private void Start()
    {
        UpdateScoreText(); // Initial update to display the starting score
    }
    private void OnTriggerEnter(Collider other)
{
 
    if (other.CompareTag("Ball"))
    {
       
       
            // goal scored
            score++;
            ball.Respawn();
            //playerRespawner.RespawnPlayersAfterGoal();
        UpdateScoreText(); 
    }
    

}
public void UpdateScoreText()
    {
        scoreText.text = "playerOne:-" + score.ToString();
    }

public void reset()
    {
        score = 0;
        UpdateScoreText(); 
    }


}
