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
<<<<<<< HEAD:Assets/Scripts/Instanse/GameManager.cs
    public int count = 0;
=======

    public bool IsPlaying { get; private set; }
>>>>>>> 814045f06f204a8d8a3bf9fa46775648899fd227:Assets/BasicScript/GameManager.cs

    private void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
<<<<<<< HEAD:Assets/Scripts/Instanse/GameManager.cs
=======
        if (IsPlaying) return;
        IsPlaying = true;
>>>>>>> 814045f06f204a8d8a3bf9fa46775648899fd227:Assets/BasicScript/GameManager.cs
        GameStart.Invoke();
        Player.Instance.transform.position = new Vector2(0, 0);
        Canvas.transform.Find("UI").Find("HP").GetComponent<Slider>().value = 1f;
        Canvas.transform.Find("UI").Find("Exp").GetComponent<Slider>().value = 0f;
    }
    public void EndGame()
    {
<<<<<<< HEAD:Assets/Scripts/Instanse/GameManager.cs
=======
        if (!IsPlaying) return;
        IsPlaying = false;
>>>>>>> 814045f06f204a8d8a3bf9fa46775648899fd227:Assets/BasicScript/GameManager.cs
        GameOver.Invoke();
    }
}
