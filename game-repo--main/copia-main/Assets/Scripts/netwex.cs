using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class netwex : NetworkBehaviour
{
    //[SerializeField]  public CinemachineVirtualCamera vc;
    [SerializeField] public CinemachineFreeLook vcc; 
    public Transform[] spawnPoints; // Assign these with two spawn points in the editor
    //public PlayerRespawner py;
     //public Ball ball;
    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            vcc.Priority = 1;
           
       
         if (IsServer && IsClient)
            {
                transform.position = spawnPoints[0].position;
                transform.rotation = spawnPoints[0].rotation;
                
            }
            else if (IsClient)
            {
               
                transform.position = spawnPoints[1].position;
                transform.rotation = spawnPoints[1].rotation;
                //GetComponent<Rigidbody>().isKinematic = false;
                //py.pi();

            }
        }
        else
        {
            vcc.Priority = 0;
        }
        
    }
}
