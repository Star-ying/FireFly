using System;
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
    public bool isPlaying = false;
    public int count = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
        GameStart.Invoke();
        Player.Instance.transform.position = new Vector2(0, 0);
        isPlaying = true;
        Canvas.transform.Find("UI").Find("HP").GetComponent<Slider>().value = 1f;
        Canvas.transform.Find("UI").Find("Exp").GetComponent<Slider>().value = 0f;
    }
    public void EndGame()
    {
        GameOver.Invoke();
    }
    private void Update()
    {
        try
        {
            Canvas.transform.Find("UI").Find("Level").Find("level").GetComponent<Text>().text = Player.Instance.Property["Level"].ToString();
            Canvas.transform.Find("UI").Find("HP").GetComponent<Slider>().value = (float)(Player.Instance.Property["health"] - Player.Instance.loss_health) / Player.Instance.Property["health"];
            Canvas.transform.Find("UI").Find("HP").Find("hp").Find("health").GetComponent<Text>().text = $"{Player.Instance.Property["health"] - Player.Instance.loss_health}/{Player.Instance.Property["health"]}";
            Canvas.transform.Find("UI").Find("Exp").GetComponent<Slider>().value = (float)Player.Instance.Property["Exp"] / Player.Instance.Property["Max_Exp"];
            Canvas.transform.Find("UI").Find("Exp").Find("exp").Find("exprision").GetComponent<Text>().text = $"{Player.Instance.Property["Exp"]}/{Player.Instance.Property["Max_Exp"]}";
        }
        catch (Exception)
        {

        }
    }
}
