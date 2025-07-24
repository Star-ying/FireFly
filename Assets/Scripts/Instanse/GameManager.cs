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
<<<<<<< HEAD:Assets/BasicScript/GameManager.cs

    public bool IsPlaying { get; private set; }
=======
    public int count = 0;
>>>>>>> b90eece9edc6d97b2467b25b1eedad96b3fdc822:Assets/Scripts/Instanse/GameManager.cs

    private void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
<<<<<<< HEAD:Assets/BasicScript/GameManager.cs
        if (IsPlaying) return;
        IsPlaying = true;
=======
>>>>>>> b90eece9edc6d97b2467b25b1eedad96b3fdc822:Assets/Scripts/Instanse/GameManager.cs
        GameStart.Invoke();
        Player.Instance.transform.position = new Vector2(0, 0);
        Canvas.transform.Find("UI").Find("HP").GetComponent<Slider>().value = 1f;
        Canvas.transform.Find("UI").Find("Exp").GetComponent<Slider>().value = 0f;
    }
    public void EndGame()
    {
<<<<<<< HEAD:Assets/BasicScript/GameManager.cs
        if (!IsPlaying) return;
        IsPlaying = false;
=======
>>>>>>> b90eece9edc6d97b2467b25b1eedad96b3fdc822:Assets/Scripts/Instanse/GameManager.cs
        GameOver.Invoke();
    }
}
