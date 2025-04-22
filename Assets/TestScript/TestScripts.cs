using UnityEngine;

public class TestScripts : MonoBehaviour
{

    public int health;
    public int score;
    public int damage;

    public void MovementTesting(string input)
    {
        Vector3 moveDirection = Vector3.zero;
        
        switch (input)
        {
            case "W":
                moveDirection = Vector3.forward;
                break;
            case "S":
                moveDirection = Vector3.back;
                break;
            case "A":
                moveDirection = Vector3.left;
                break;
            case "D":
                moveDirection = Vector3.right;
                break;
            case "Space":
                moveDirection = Vector3.up;
                break;
            case "Q":
                transform.rotation = Quaternion.Euler(0, -10, 0);
                break;
        }

        //transform.position += moveDirection * 10f * Time.deltaTime;
    }

    public void HealthTest(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
    }

    public void AddScore(int addScore)
    {
        score += addScore;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Attack(EnemyTest enemyTest, int damage)
    {
        enemyTest.health -= damage;
    }

    public void HealBuff(int healAmount)
    {
        health += healAmount;
        if (health > 100)
        {
            health = 100;
        }
    }
}
