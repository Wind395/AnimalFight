using System.Collections;
using Fusion;
using UnityEngine;

public class Ammo : NetworkBehaviour
{
    public int speed = 2;

    public override void Render()
    {
        StartCoroutine(ReturnToPool());
    }

    IEnumerator ReturnToPool()
    {
        transform.position += transform.forward * speed * Runner.DeltaTime;
        yield return new WaitForSeconds(2f);
        if (Object.HasStateAuthority)
        {
            ObjectAmmoPool.Instance.ReturnObject(gameObject);
        }
    }
}
