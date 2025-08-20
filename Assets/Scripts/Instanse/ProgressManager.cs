using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instanse { get; private set; }
    public bool space = true;
    public Dictionary<string, int> Progress = new();
    public Canvas canvas3;
    public Canvas canvas4;
    public Text text;
    private void Awake()
    {
        Instanse = this;
    }
    public void GetReward_Property(Dictionary<string, int> reward)
    {
        foreach (var e in reward)
        {
            Player.Instance.Property[e.Key] += e.Value;
        }
    }
    public void GetReward_Tool(Dictionary<string, int> reward)
    {
        foreach (var e in reward)
        {
            for (int i = 0; i < e.Value; i++)
            {
                BagManager.Instanse.AddTool(e.Key[0].ToString(), e.Key);
            }
        }
    }
    public void ShowReward(string[] reward)
    {
        canvas4.transform.Find("Reward").Find("reward").GetComponent<Text>().text = $"{string.Join("\n", reward)}";
        canvas4.transform.Find("Reward").gameObject.SetActive(true);
    }
    public void BiginSelect(UnityEngine.Events.UnityAction yes, UnityEngine.Events.UnityAction no)
    {
        canvas4.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(yes);
        canvas4.transform.Find("No").GetComponent<Button>().onClick.AddListener(no);
    }
    public void Select()
    {
        canvas4.transform.Find("Yes").gameObject.SetActive(true);
        canvas4.transform.Find("No").gameObject.SetActive(true);
    }
    public void EndSelect()
    {
        canvas4.transform.Find("Yes").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas4.transform.Find("No").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas4.transform.Find("Yes").gameObject.SetActive(false);
        canvas4.transform.Find("No").gameObject.SetActive(false);
    }
    public void EndTalk()
    {
        canvas3.gameObject.SetActive(true);
        Player.Instance.isTalk = false;
        canvas4.gameObject.SetActive(false);
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        space = true;
    }
    public void SetProgress(int[] progress)
    {
        for(int i = 0;i< transform.childCount; i++)
        {
            try
            {
                Progress.Add(transform.GetChild(i).gameObject.name, i < progress.Length ? progress[i] : 0);
            }
            catch (Exception)
            {
                Progress[transform.GetChild(i).gameObject.name] = i < progress.Length ? progress[i] : 0;
            }
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void ResyscleProgress()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
