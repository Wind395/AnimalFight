using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AnimalsWeapon : NetworkBehaviour
{
    [SerializeField] private GameObject weaponPosition;
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask weaponMask;
    [SerializeField] private float dropOffset;
    
    [Networked] public NetworkObject currentWeapon { get; set; }
    [Networked] private NetworkBool IsWeaponObjectActive { get; set; }

    public override void FixedUpdateNetwork()
    {
    }

    public override void Render()
    {
        // Chỉ client có InputAuthority mới được phép nhặt vũ khí
        if (currentWeapon == null && Object.HasInputAuthority)
        {
            Rpc_GetWeapon();
        }

        // Đồng bộ trạng thái của weaponObject
        weaponObject.SetActive(IsWeaponObjectActive);

        //Debug.Log($"Current Weapon: {currentWeapon}, WeaponObject Active: {IsWeaponObjectActive}");

        if (Input.GetKeyDown(KeyCode.G) && Object.HasInputAuthority)
        {
            Drop();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void Rpc_GetWeapon()
    {
        if (currentWeapon != null) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, weaponMask);

        if (colliders.Length > 0)
        {
            ShootingWeapon nearestWeapon = null;
            float minDistance = Mathf.Infinity;

            foreach (var col in colliders)
            {
                ShootingWeapon weapon = col.GetComponent<ShootingWeapon>();
                if (weapon != null && weapon.Object != null && !weapon.IsPickedUp)
                {
                    float distance = Vector3.Distance(transform.position, weapon.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestWeapon = weapon;
                    }
                }
            }

            if (nearestWeapon != null && nearestWeapon.Object != null)
            {
                if (Object.HasStateAuthority)
                {
                    currentWeapon = nearestWeapon.Object;
                    ObjectPooling.Instance.ReturnObject(currentWeapon.gameObject); // Trả vũ khí về pool
                    IsWeaponObjectActive = true;
                    nearestWeapon.SetPickedUp(true);
                    // Gọi RPC để thông báo cho tất cả client cập nhật trạng thái
                    RPC_SyncWeaponPickup(nearestWeapon.Object.Id);
                }
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SyncWeaponPickup(NetworkId weaponId)
    {
        var weaponObject = Runner.FindObject(weaponId);
        if (weaponObject != null)
        {
            ShootingWeapon weapon = weaponObject.GetComponent<ShootingWeapon>();
            if (weapon != null)
            {
                weapon.SetPickedUp(true);
                ObjectPooling.Instance.ReturnObject(weaponObject.gameObject);
            }
        }
    }

    void Drop()
    {
        if (currentWeapon == null) return;

        if (Object.HasStateAuthority)
        {
            IsWeaponObjectActive = false;
            weaponObject.GetComponent<ShootingWeapon>().SetPickedUp(false);
            currentWeapon = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}