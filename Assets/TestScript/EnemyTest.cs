using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public int health;
    public int damage;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
