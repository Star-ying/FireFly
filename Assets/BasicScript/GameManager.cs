<<<<<<< HEAD
using Mono.Data.Sqlite;
=======
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
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

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
        if (IsPlaying) return;
        IsPlaying = true;
        GameStart.Invoke();
        Player.Instance.transform.position = new Vector2(0, 0);
        Canvas.transform.Find("UI").Find("HP").GetComponent<Slider>().value = 1f;
        Canvas.transform.Find("UI").Find("Exp").GetComponent<Slider>().value = 0f;
    }
    public void EndGame()
    {
        if (!IsPlaying) return;
        IsPlaying = false;
        GameOver.Invoke();
    }
}
