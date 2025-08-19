using System.Collections;
using UnityEngine;

public class QiqiController : MonoBehaviour
{
    [Header("基础属性")]
    public float health = 100f;
    public float speed = 2f;
    public int damage = 10;
    public int defense = 10;

    [Header("难度系数")]
    public float healthMultiplier = 1.1f; 
    public float speedMultiplier = 1.05f; 

<<<<<<< HEAD
    void OnEnable()
=======
    private Transform player; 

    void Start()
>>>>>>> 0bab1afe548b004d26b66bbad25659e5f7fe0acc
    {
        health = 100f;
        speed = 2f;
        damage = 10;
        defense = 10;

        if (Player.Instance == null)
        {
            Debug.LogError("找不到玩家！请确保玩家有'Player'标签");
            enabled = false;
        }
    }

    void Update()
    {
<<<<<<< HEAD
        if (Player.Instance != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                Player.Instance.transform.position,
=======
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
>>>>>>> 0bab1afe548b004d26b66bbad25659e5f7fe0acc
                speed * Time.deltaTime
            );
        }
    }

<<<<<<< HEAD
=======
    
>>>>>>> 0bab1afe548b004d26b66bbad25659e5f7fe0acc
    public void SetDifficulty(int waveNumber)
    {
        health *= Mathf.Pow(healthMultiplier, waveNumber - 1);
        speed *= Mathf.Pow(speedMultiplier, waveNumber - 1);
    }
<<<<<<< HEAD
=======

    
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        gameObject.SetActive(false);
    }

>>>>>>> 0bab1afe548b004d26b66bbad25659e5f7fe0acc

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attacking"))
        {
<<<<<<< HEAD
            int temp = Player.Instance.Damage(defense, collision.gameObject.name);
            if (health > temp) health -= temp;
            else Die();
=======
            health -=Player.Instance.Damage(defense, collision.gameObject.name);
>>>>>>> 0bab1afe548b004d26b66bbad25659e5f7fe0acc
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}