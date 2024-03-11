
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;



public class PlayerRespawner : NetworkBehaviour
{
    public List<GameObject> Respawns = new List<GameObject>();

    public Transform playerOneSpawnPoint; // Assign in the editor
    public Transform playerTwoSpawnPoint; // Assign in the editor
    public Ball ball;
    
    public gol lol;
    public gol2 lol2;

    public void RespawnPlayersAfterGoal()
    {
        RespawnPlayersServerRpc();
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.K) && IsOwner)
            RespawnPlayersServerRpc();
    }

    public void pi()
    {
    
       lol.ResetScoreServerRpc();
       lol2.ResetScoreServerRpc();
       RespawnPlayersServerRpc();
       ball.Respawn();
    }    

   // [ServerRpc]
    [ServerRpc(RequireOwnership = false)]
    private void RespawnPlayersServerRpc()
    {
        // Respawn the host player
        RespawnPlayer(NetworkManager.Singleton.LocalClientId, true);

        // Respawn the client player
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (client.Key != NetworkManager.Singleton.LocalClientId)
            {
                RespawnPlayer(client.Key, false);
                break; // Since it's 1v1, we can break after finding the one other client
            }
        }
    }

    private void RespawnPlayer(ulong clientId, bool isHost)
    {
        // Get the NetworkObject for the player
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var networkClient))
        {
            // Assuming the player component is attached to the network object
            var player = networkClient.PlayerObject.GetComponent<CubeController>();
            if (player)
            {
                // Move the player to the correct spawn point
                Vector3 spawnPosition = isHost ? playerOneSpawnPoint.position : playerTwoSpawnPoint.position;
                Quaternion spawnRotation = isHost ? playerOneSpawnPoint.rotation : playerTwoSpawnPoint.rotation;

                player.transform.position = spawnPosition;
                player.transform.rotation = spawnRotation;

                // Reset other player states as needed

                // Inform the client of their new position
                MovePlayerClientRpc(spawnPosition, spawnRotation, clientId);
            }
        }
    }

    [ClientRpc(RequireOwnership = false)]
    private void MovePlayerClientRpc(Vector3 position, Quaternion rotation, ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            var player = Respawns[(int)clientId] ;

            //NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var networkClient);
            //var player = networkClient.PlayerObject.GetComponent<CubeController>();
            // The client knows this is their player and can move them accordingly
            player.transform.position = position;
            player.transform.rotation = rotation;

            // Reset other necessary states here
        }
    }


    private NetworkObject GetClientNetworkObject(ulong clientId)
    {

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var networkClient))
        {

            return networkClient.PlayerObject;
        }

        return null;
    }
}


