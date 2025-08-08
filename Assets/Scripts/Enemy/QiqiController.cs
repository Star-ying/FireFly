using System.Collections;
using UnityEngine;

public class QiqiController : MonoBehaviour
{
    [Header("基础属性")]
    public float health = 100f;
    public float speed = 2f;
    public int damage = 10;

    [Header("难度系数")]
    public float healthMultiplier = 1.1f; 
    public float speedMultiplier = 1.05f; 

    private Transform player; 

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("找不到玩家！请确保玩家有'Player'标签");
            enabled = false;
        }
    }

    void Update()
    {
        if (player != null)
        {
            
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }

    
    public void SetDifficulty(int waveNumber)
    {
        health *= Mathf.Pow(healthMultiplier, waveNumber - 1);
        speed *= Mathf.Pow(speedMultiplier, waveNumber - 1);
    }

    
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

        Destroy(gameObject);


    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}