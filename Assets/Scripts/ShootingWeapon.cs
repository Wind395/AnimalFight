using Fusion;
using UnityEngine;

public class ShootingWeapon : NetworkBehaviour
{

    [Networked] public NetworkBool IsPickedUp { get; set; }
    public GameObject firePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Render()
    {
        if (Input.GetButtonDown("Fire1") && Object.HasInputAuthority)
        {
            // Call the method to shoot or perform an action
            Rpc_Shoot();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void Rpc_Shoot()
    {
        ObjectAmmoPool.Instance.GetObject(firePosition.transform.position);
        // Implement shooting logic here
        Debug.Log("Shooting with weapon: " + gameObject.name);
    }

    public void SetPickedUp(bool pickedUp)
    {
        IsPickedUp = pickedUp;
    }
}
