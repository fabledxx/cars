using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class test : MonoBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    void RequestOwnershipServerRpc(ulong newOwnerClientId)
    {
        var networkObject = GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.ChangeOwnership(newOwnerClientId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var dir = transform.position - other.transform.position;
        if (other.gameObject.CompareTag("Player"))
        {
            var networkObject = other.gameObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                RequestOwnershipServerRpc(networkObject.OwnerClientId);
                print(networkObject.OwnerClientId);
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        var dir = transform.position - other.transform.position;
        if (other.gameObject.CompareTag("Player"))
        {
            var networkObject = other.gameObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                RequestOwnershipServerRpc(networkObject.OwnerClientId);
                print(networkObject.OwnerClientId);
            }

        }
    }
}
