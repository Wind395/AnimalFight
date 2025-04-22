using UnityEngine;

public class Zweihander : WeaponBase
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Debug.Log("OnTriggerEnter called with: " + other.tag);
            var animal = other.gameObject.GetComponent<AnimalProperties>(); // Làm sử dụng cơ sở hữu vũ khí>
            if (animal != null)
            {
                // Gửi RPC tới client có quyền trên mục tiêu để áp dụng sát thương
                animal.Rpc_TakeDamage(damage);
                animal.Rpc_UpdatePoint(damage);
            }
        }
    }
}
