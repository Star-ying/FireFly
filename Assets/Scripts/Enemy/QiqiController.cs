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

    void OnEnable()
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
        if (Player.Instance != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                Player.Instance.transform.position,
                speed * Time.deltaTime
            );
        }
    }

    public void SetDifficulty(int waveNumber)
    {
        health *= Mathf.Pow(healthMultiplier, waveNumber - 1);
        speed *= Mathf.Pow(speedMultiplier, waveNumber - 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attacking"))
        {
            int temp = Player.Instance.Damage(defense, collision.gameObject.name);
            if (health > temp) health -= temp;
            else Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}