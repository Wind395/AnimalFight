using Unity.VisualScripting;
using UnityEngine;
using Fusion;
using NUnit.Framework;

public abstract class WeaponBase : NetworkBehaviour
{
    public int damage;
    [Networked] public NetworkBool IsPickedUp { get; set; }
    private NetworkTransform networkTransform;
    public Rigidbody rb;
    private BoxCollider boxCollider;

    void Start()
    {
        networkTransform = GetComponent<NetworkTransform>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void SetPosition(Transform weaponPos)
    {
        IsPickedUp = true;
        rb.isKinematic = true;
        boxCollider.enabled = false;
        transform.SetParent(weaponPos);
        transform.position = weaponPos.position;
    }

    public void DropWeapon(float dropOffset, Transform target)
    {
        IsPickedUp = false;
        rb.isKinematic = false;
        boxCollider.enabled = true;
        transform.SetParent(null);
        transform.position = target.position + target.forward * dropOffset;
        transform.rotation = Quaternion.identity;
    }

    public void SetPickedUp(bool pickedUp)
    {
        IsPickedUp = pickedUp;
    }
}