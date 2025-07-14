using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public static bool Exists => Instance != null;

    public UnityEvent GameStart = new();
    public UnityEvent GameOver = new();

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        Instance = this;
        IsPlaying = true;
    }
    public void StartGame()
    {
        if (IsPlaying) return;
        IsPlaying = true;
        GameStart.Invoke();
    }
    public void EndGame()
    {
        if (!IsPlaying) return;
        IsPlaying = false;
        GameOver.Invoke();
    }
    public void SetBullet()
    {
        
    }
}
