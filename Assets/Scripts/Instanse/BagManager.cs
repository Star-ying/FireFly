using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{
    public static BagManager Instanse;
    public Button tool_Button;
    public Canvas canvas3;
    public InputField Page;
    private int page = 0;
    private string Tool_name = "";
    private string Class = "1";
    public Dictionary<string, List<string>> tool { get; private set; } = new();
    private void Awake()
    {
        Instanse = this;
        tool.Add("2", new List<string>());
        tool.Add("3", new List<string>());
        tool.Add("4", new List<string>());
        tool.Add("5", new List<string>());
    }
    public void ShowAllBag()
    {
        int i;
        for(i = 0;i < 10; i++)
        {
            canvas3.transform.Find("UI").Find("Bag").Find($"tool{i}").Find("Image").GetComponent<Image>().sprite = null;
        }
        int length = 0;
        foreach (List<string> name in tool.Values)
        {
            length += name.Count;
        }
        canvas3.transform.Find("UI").Find("Bag").Find("Page").GetComponent<InputField>().text = $"{page + 1}";
        canvas3.transform.Find("UI").Find("Bag").Find("Last").gameObject.SetActive(!(page == 0));
        canvas3.transform.Find("UI").Find("Bag").Find("Next").gameObject.SetActive(page * 10 < length - 10);
        i = 0;
        foreach(var e in tool)
        {
            foreach(string name in e.Value)
            {
                if(i >= page * 10 && i < page * 10 + 10)
                {
                    canvas3.transform.Find("UI").Find("Bag").Find($"tool{i - page * 10}").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(name);
                }
                i++;
            }
        }
        DestroyClass();
    }
    public void ShowBag()
    {
        for(int i = 0; i < 10; i++)
        {
            canvas3.transform.Find("UI").Find("Bag").Find($"tool{i}").Find("Image").GetComponent<Image>().sprite = null;
        }
        canvas3.transform.Find("UI").Find("Bag").Find("Page").GetComponent<InputField>().text = $"{page + 1}";
        canvas3.transform.Find("UI").Find("Bag").Find("Last").gameObject.SetActive(!(page == 0));
        canvas3.transform.Find("UI").Find("Bag").Find("Next").gameObject.SetActive(page * 10 < tool[Class].Count - 10);
        for(int i = page * 10; i < 10 + 10 * page;i++)
        {
            if (i < tool[Class].Count)
            {
                canvas3.transform.Find("UI").Find("Bag").Find($"tool{i - page * 10}").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(tool[Class][i]);
            }
        }
        DestroyClass();
    }
    public void Check()
    {
        if (Class.Equals("1"))
        {
            ShowAllBag();
        }
        else
        {
            ShowBag();
        }
    }
    public void AddClass()
    {
        ButtonEvent.Class += ClassEvent;
    }
    public void DestroyClass()
    {
        ButtonEvent.Class -= ClassEvent;
    }
    private void ClassEvent(Button btn)
    {
        page = 0;
        Class = btn.name.Replace("Class", "");
        Check();
    }
    public void AddToolEvent()
    {
        ButtonEvent.Tool += ToolEvent;
    }
    public void DestroyToolEvent()
    {
        ButtonEvent.Tool -= ToolEvent;
    }
    public void ToolEvent(Button btn)
    {
        Transform use = canvas3.transform.Find("UI").Find("Bag").Find("Select").Find("Use");
        tool_Button = btn;
        try
        {
            Tool_name = btn.transform.Find("Image").GetComponent<Image>().sprite.name;
        }
        catch (Exception)
        {
            return;
        }
        if (Tool_name.StartsWith("2"))
        {
            use.Find("Text").GetComponent<Text>().text = "使用";
        }
        else if (Tool_name.StartsWith("3"))
        {
            use.Find("Text").GetComponent<Text>().text = "使用";
            Select();
        }
        else if (Tool_name.StartsWith("4"))
        {
            use.Find("Text").GetComponent<Text>().text = "装备";
            Select();
        }
        DestroyToolEvent();
    }
    public void Equip()
    {
        Transform Position = canvas3.transform.Find("UI").Find("Equipment").GetChild(Tool_name[1] - 48);
        try
        {
            Player.Instance.transform.Find("Equipment").Find(Position.GetComponent<Image>().sprite.name).gameObject.SetActive(false);
            Player.Instance.transform.Find("Role").gameObject.SetActive(false);
            Player.Instance.transform.Find("Equipment").gameObject.SetActive(false);
            Player.Instance.transform.Find("Role").gameObject.SetActive(true);
            Player.Instance.transform.Find("Equipment").gameObject.SetActive(true);
        }
        catch (Exception)
        {

        }
        Position.GetComponent<Image>().sprite = Resources.Load<Sprite>(Tool_name);
        Player.Instance.transform.Find("Equipment").Find($"WPN_{Tool_name}").gameObject.SetActive(true);
    }
    public void Select()
    {
        canvas3.transform.Find("UI").Find("Bag").Find("Select").transform.position = Input.mousePosition;
        canvas3.transform.Find("UI").Find("Bag").Find("Select").gameObject.SetActive(true);
    }
    public void CancelSelect()
    {
        canvas3.transform.Find("UI").Find("Bag").Find("Select").gameObject.SetActive(false);
    }
    public void AddTool(string Class, string Tool)
    {
        tool[Class].Add(Tool);
    }
    public void NextPage()
    {
        page++;
        Check();
    }
    public void LastPage()
    {
        page--;
        Check();
    }
    public void ChangePage()
    {
        try
        {
            page = Convert.ToInt32(Page.text) + 1;
            Check();
        }
        catch (Exception)
        {
            Debug.Log("输入格式不正确");
        }
    }
    public void CancelCheck()
    {
        page = 0;
    }
}
