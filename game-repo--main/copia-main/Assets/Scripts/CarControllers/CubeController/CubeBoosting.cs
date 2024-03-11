using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CubeController))]
public class CubeBoosting : NetworkBehaviour
{
    public float BoostForceMultiplier = 1f;
    const float BoostForce = 991 / 100;
    
    CubeController _c;
    Rigidbody _rb;
    public override void OnNetworkSpawn() {
       if(!IsOwner)Resources.FindObjectsOfTypeAll<CubeParticleSystem>()[0].gameObject.SetActive(false);
  ;
    }

    private void Start()
    {
        _c = GetComponent<CubeController>();
        _rb = GetComponentInParent<Rigidbody>();
        
        // Activate ParticleSystems GameObject
        if (Resources.FindObjectsOfTypeAll<CubeParticleSystem>()[0] != null && IsOwner)
            Resources.FindObjectsOfTypeAll<CubeParticleSystem>()[0].gameObject.SetActive(true);
    }




    void FixedUpdate()
    {
        
        Boosting();
    }
    
    void Boosting()
    {
        if (GameManager.InputManager.isBoost && _c.forwardSpeed < CubeController.MaxSpeedBoost)
        {
                _rb.AddForce(BoostForce * BoostForceMultiplier * transform.forward, ForceMode.Acceleration);
        }
    }
}