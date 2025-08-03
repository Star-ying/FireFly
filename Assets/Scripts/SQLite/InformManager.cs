using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;

public class InformManager : MonoBehaviour
{
    public static InformManager Instance { get; private set; }

    private SQLiteHelper sql;
    private SqliteDataReader reader;
    public InputField Name_l;
    public InputField Password_l;
    public InputField Name_r;
    public InputField Password_r;
    public InputField cir_Password;
    public GameObject UI_Massage;
    public GameObject Canvas1;
    public GameObject Canvas2;
    public GameObject Role_Select;
    public bool isLogin = true;
    private string Name;
    private string P_Name;
    private long id;

    private void Awake()
    {
        Instance = this;
        sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/SQLite.db");
    }
    private void OnApplicationQuit()
    {
        ProgressManager.Instanse.ResyscleProgress();
        UpdateBag();
        UpdateEquipment();
        UpdatePlayer();
        UpdateTalk();
        sql.CloseConnection();
    }
    public void Turn()
    {
        if (!isLogin)
        {
            isLogin = true;
            return;
        }
        isLogin = false;
    }
    public void Send()
    {
        reader = sql.ReadFullTable("Player");
        if (isLogin)
        {
            if (!Name_l.text.Equals("") && !Password_l.text.Equals(""))
            {
                while (reader.Read())
                {
                    if (reader.GetValue(reader.GetOrdinal("Name")).Equals(Name_l.text))
                    {
                        if (reader.GetValue(reader.GetOrdinal("Password")).Equals(Password_l.text))
                        {
                            StartCoroutine(massage("登录成功"));
                            reader = sql.ReadTable("Archive", new string[] {"ID","Name","Level","Exp" }, new string[] { "P_Name" }, new string[] { "=" }, new string[] { $"{Name_l.text}" },new string[] { });
                            int i = 0;
                            Transform roles = Canvas2.transform.Find("archive");
                            while (reader.Read() && i < 12)
                            {
                                Transform archive = roles.Find($"Archive{i}");
                                Sprite role = Resources.Load<Sprite>($"{reader.GetValue(reader.GetOrdinal("Name"))}");
                                archive.Find("Image").GetComponent<Image>().sprite = role;
                                archive.Find("Level").GetComponent<Text>().text = $"Level:{reader.GetValue(reader.GetOrdinal("Level"))}";
                                archive.Find("Exp").GetComponent<Text>().text = $"Exp:{reader.GetValue(reader.GetOrdinal("Exp"))}";
                                archive.Find("ID").GetComponent<Text>().text = $"{reader.GetValue(reader.GetOrdinal("ID"))}";
                                archive.Find("Image").gameObject.SetActive(true);
                                archive.Find("Level").gameObject.SetActive(true);
                                archive.Find("Exp").gameObject.SetActive(true);
                                i++;
                            }
                            P_Name = Name_l.text;
                            return;
                        }
                        StartCoroutine(massage("密码错误"));
                        return;
                    }
                }
                StartCoroutine(massage("你还没有注册"));
                return;
            }
            else
            {
                StartCoroutine(massage("请输入账号密码"));
            }
        }
        else
        {
            if (!Name_r.text.Equals("") && !Password_r.text.Equals("") && !cir_Password.text.Equals(""))
            {
                while (reader.Read())
                {
                    if (reader.GetValue(reader.GetOrdinal("Name")).Equals(Name_r.text))
                    {
                        StartCoroutine(massage("用户名已存在"));
                        return;
                    }
                    else if (!Password_r.text.Equals(cir_Password.text))
                    {
                        StartCoroutine(massage("两次输入密码不一致"));
                        return;
                    }
                }
                StartCoroutine(massage("注册成功"));
                sql.InsertValues("Player",new Dictionary<string, object> {["Name"] = Name_r.text,["Password"] = Password_r.text });
                P_Name = Name_r.text;
                return;
            }
            else
            {
                StartCoroutine(massage("请填写完整"));
                return;
            }
        }
    }
    IEnumerator massage(string str)
    {
        UI_Massage.transform.Find("Text").GetComponent<Text>().text = str;
        UI_Massage.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (str.Equals("登录成功") || str.Equals("注册成功"))
        {
            Canvas1.SetActive(false);
            Canvas2.SetActive(true);
        }
        UI_Massage.SetActive(false);
    }
    public void InsertRole()
    {
        id = sql.Count("Archive") + 1;
        sql.InsertValues("Archive", new Dictionary<string, object> { ["ID"] = id, ["Name"] = Name, ["P_Name"] = P_Name });
        sql.CreateTable($"Bag{id}", new string[] { "Name", "Class"}, new string[] { "STRING", "STRING"});
        sql.CreateTable($"Talk{id}", new string[] { "NPC", "Progress" }, new string[] { "STRING", "INTEGER" });
        ProgressManager.Instanse.SetProgress(new int[] { });
        sql.InsertValues("Equipment", new Dictionary<string, object> {["A_ID"] = id});
        Player.Instance.Level = 1;
    }
    public void GetEquipments()
    {
        reader = sql.ReadTable("Equipment", new string[] { "*" }, new string[] { "A_ID" }, new string[] { "=" }, new string[] { $"{id}" }, new string[] { });
        SqliteDataReader reader1 = sql.ReadItem("Equipment");
        reader1.Read();
        for (int i = 1; i < 4; i++)
        {
            reader1.Read();
            Player.Instance.SetEquipment($"{reader.GetValue(i)}", $"{ reader1.GetValue(1)}");
        }
    }
    public void GetBag()
    {
        reader = sql.ReadFullTable($"Bag{id}");
        while (reader.Read())
        {
            BagManager.Instanse.AddTool(reader.GetValue(1).ToString(), reader.GetValue(0).ToString());
        }
    }
    public void GetTalk()
    {
        reader = sql.ReadFullTable($"Talk{id}");
        List<int> progress = new();
        while (reader.Read())
        {
            progress.Add(Convert.ToInt32(reader.GetValue(1)));
        }
        ProgressManager.Instanse.SetProgress(progress.ToArray());
    }
    public void UpdateBag()
    {
        sql.ExecuteQuery($"Delete From Bag{id}");
        foreach (var e in BagManager.Instanse.tool)
        {
            foreach (var i in e.Value)
            {
                sql.InsertValues($"Bag{id}", new Dictionary<string, object> { ["Name"] = i, ["Class"] = e.Key });
            }
        }
    }
    public void UpdateEquipment()
    {
        List<string> equipment = new();
        List<string> name = new();
        Transform equip = Player.Instance.transform.Find("Equipment");
        for (int i = 0;i < equip.childCount;i++)
        {
            if (equip.GetChild(i).gameObject.activeSelf)
            {
                if (equip.name.Contains("41"))
                {
                    equipment.Insert(0, equip.name);
                    name.Insert(0, equip.name);
                }
                else if (equip.name.Contains("42"))
                {
                    equipment.Insert(1, equip.name);
                    name.Insert(1, equip.name);
                }
                else
                {
                    equipment.Insert(2, equip.name);
                    name.Insert(2, equip.name);
                }
            }
        }
        if(equipment.Count != 0)
        {
            sql.UpdateValues("Equipment", name.ToArray(), equipment.ToArray(), "A_ID", "=", $"{id}");
        }
    }
    public void UpdatePlayer()
    {
        sql.UpdateValues("Archive", new string[] { "Exp", "Level" }, new string[] { $"{Player.Instance.Property["Exp"]}", $"{Player.Instance.Level}" }, "ID","=",$"{id}");
    }
    public void UpdateTalk()
    {
        sql.ExecuteQuery($"Delete From Talk{id}");
        foreach(var e in ProgressManager.Instanse.Progress)
        {
            sql.InsertValues($"Talk{id}", new Dictionary<string, object> { ["NPC"] = e.Key, ["Progress"] = e.Value });
        }
    }
    public void AddArchive()
    {
        Canvas2.transform.Find("Button1").gameObject.SetActive(false);
        Canvas2.transform.Find("Button2").gameObject.SetActive(false);
        ButtonEvent.Archive += ArchiveEvent;
    }
    public void DeleteArchive()
    {
        // 取消订阅防止内存泄漏
        ButtonEvent.Archive -= ArchiveEvent;
    }
    private void ArchiveEvent(Button clickedButton)
    {
        // 事件处理方法
        if (!clickedButton.transform.Find("Image").gameObject.activeSelf)
        {
            Role_Select.SetActive(true);
        }
        else
        {
            Transform archive = clickedButton.transform;
            id = Convert.ToInt64(archive.Find("ID").GetComponent<Text>().text);
            Name = archive.Find("Image").GetComponent<Image>().sprite.name;
            Player.Instance.Property["Exp"] = Convert.ToInt32(archive.Find("Exp").GetComponent<Text>().text.Split("Exp:")[1]);
            Player.Instance.Level = Convert.ToInt32(archive.Find("Level").GetComponent<Text>().text.Split("Level:")[1]);
            Player.Instance.transform.Find("Role").Find($"{Name}").gameObject.SetActive(true);
            GetBag();
            GetEquipments();
            GetTalk();
            Canvas2.transform.Find("Button1").gameObject.SetActive(true);
            Canvas2.transform.Find("Button2").gameObject.SetActive(true);
        }
    }
    public void AddRole()
    {
        Canvas2.transform.Find("Button3").gameObject.SetActive(false);
        Canvas2.transform.Find("Button4").gameObject.SetActive(false);
        // 订阅按钮点击事件
        ButtonEvent.Role += RoleEvent;
    }
    public void DeleteRole()
    {
        // 取消订阅防止内存泄漏
        ButtonEvent.Role -= RoleEvent;
    }
    private void RoleEvent(Button clickedButton)
    {
        Name = clickedButton.name;
        Player.Instance.transform.Find("Role").Find($"{Name}").gameObject.SetActive(true);
        Canvas2.transform.Find("Button3").gameObject.SetActive(true);
        Canvas2.transform.Find("Button4").gameObject.SetActive(true);
    }
}
