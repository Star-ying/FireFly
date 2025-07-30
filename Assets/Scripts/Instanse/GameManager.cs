using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public static bool Exists => Instance != null;

    public UnityEvent GameStart = new();
    public UnityEvent GameOver = new();

    public GameObject Canvas;
    public int count = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
        GameStart.Invoke();
        Player.Instance.transform.position = new Vector2(0, 0);
        Canvas.transform.Find("UI").Find("HP").GetComponent<Slider>().value = 1f;
        Canvas.transform.Find("UI").Find("Exp").GetComponent<Slider>().value = 0f;
    }
    public void EndGame()
    {
        GameOver.Invoke();
    }
}
