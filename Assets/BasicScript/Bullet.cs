using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject liuying;
    public Vector2 movement;
    public float speed;
    public float destory_max_X;
    public float destory_min_X;
    public float destory_max_Y;
    public float destory_min_Y;

    public void SetSpawner(GameObject liuying) => this.liuying = liuying;

    void Stop()
    {
        if(liuying != null)
        {
            Player.Instance.ReturnPooledObject(gameObject,Player.Instance.Bullet);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") &&
            gameObject.activeSelf &&
            other.gameObject.activeSelf &&
            GameManager.Exists &&
            GameManager.Instance.IsPlaying)
        {
            Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Exists && GameManager.Instance.IsPlaying)
        {
            transform.Translate(speed * Time.deltaTime * movement);
            if(transform.position.x > destory_max_X || 
                transform.position.x < destory_min_X |
                transform.position.y > destory_max_Y ||
                transform.position.y < destory_min_Y ||
                Player.Instance.isSaMu)
            {
                Stop();
            }
        }
    }
}
