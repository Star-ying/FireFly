using System.Collections;
using UnityEngine;

public class QiqiController : LivingEntity
{
    [Header("基础属性")]
    public float health = 100f;
    public float speed = 2f;
    public int damage = 10;

    private Transform player;
    public string poolTag = "Qiqi_Normal";

    protected override void Start()
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
            transform.position = Vector2.MoveTowards(transform.position,player.position,speed * Time.deltaTime);
        }
    }

    
    //public void SetDifficulty(int waveNumber)
    //{
    //    health *= Mathf.Pow(EnemySpawner.healthMultiplier, waveNumber - 1);
    //    speed *= Mathf.Pow(speedMultiplier, waveNumber - 1);
    //}

    
    //public void TakeDamage(float damageAmount)
    //{
    //    health -= damageAmount;

    //    if (health <= 0)
    //    {
    //        Die();
    //    }
    //}
    //public void Die()
    //{
    //    EnemyPool.Instance.ReturnToPool(poolTag, gameObject);
    //}

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