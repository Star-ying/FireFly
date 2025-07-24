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
<<<<<<< HEAD
    public GameObject Canvas1;
    public GameObject Canvas2;
=======
    public GameObject Carvas1;
    public GameObject Carvas2;
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
    public GameObject Role_Select;
    public bool isLogin = true;
    private string Name;
    private string P_Name;
    private int id;

    public void Turn()
    {
        if (!isLogin)
        {
            isLogin = true;
            return;
        }
        isLogin = false;
    }
    private void Awake()
    {
        Instance = this;
        sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/SQLite.db");
    }
    public void AddArchive()
    {
<<<<<<< HEAD
        Canvas2.transform.Find("Button1").gameObject.SetActive(false);
        Canvas2.transform.Find("Button2").gameObject.SetActive(false);
=======
        Carvas2.transform.Find("Button1").gameObject.SetActive(false);
        Carvas2.transform.Find("Button2").gameObject.SetActive(false);
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
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
            id = Convert.ToInt32(archive.Find("ID").GetComponent<Text>().text);
            Name = archive.Find("Image").GetComponent<Image>().sprite.name;
            Player.Instance.Exp = Convert.ToInt32(archive.Find("Exp").GetComponent<Text>().text.Split("Exp:")[1]);
            Player.Instance.Level = Convert.ToInt32(archive.Find("Level").GetComponent<Text>().text.Split("Level:")[1]);
            Player.Instance.transform.Find("Role").Find($"{Name}").gameObject.SetActive(true);
<<<<<<< HEAD
            Canvas2.transform.Find("Button1").gameObject.SetActive(true);
            Canvas2.transform.Find("Button2").gameObject.SetActive(true);
=======
            SqliteDataReader reader = sql.ReadTable("Role", new string[] { "Health", "Attack", "Defense" }, new string[] { "Name" }, new string[] { "=" }, new string[] { Name }, new string[] { });
            Player.Instance.health = Convert.ToInt32(reader.GetValue(0));
            Player.Instance.attack = Convert.ToInt32(reader.GetValue(1));
            Player.Instance.defense = Convert.ToInt32(reader.GetValue(2));
            Carvas2.transform.Find("Button1").gameObject.SetActive(true);
            Carvas2.transform.Find("Button2").gameObject.SetActive(true);
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
        }
    }
    public void AddRole()
    {
<<<<<<< HEAD
        Canvas2.transform.Find("Button3").gameObject.SetActive(false);
        Canvas2.transform.Find("Button4").gameObject.SetActive(false);
=======
        Carvas2.transform.Find("Button3").gameObject.SetActive(false);
        Carvas2.transform.Find("Button4").gameObject.SetActive(false);
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
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
<<<<<<< HEAD
        Canvas2.transform.Find("Button3").gameObject.SetActive(true);
        Canvas2.transform.Find("Button4").gameObject.SetActive(true);
=======
        SqliteDataReader reader = sql.ReadTable("Role", new string[] { "Health", "Attack", "Defense" }, new string[] { "Name" }, new string[] { "=" }, new string[] { $"{Name}" }, new string[] { });
        while (reader.Read())
        {
            Player.Instance.health = Convert.ToInt32(reader.GetValue(0));
            Player.Instance.attack = Convert.ToInt32(reader.GetValue(1));
            Player.Instance.defense = Convert.ToInt32(reader.GetValue(2));
        }
        Carvas2.transform.Find("Button3").gameObject.SetActive(true);
        Carvas2.transform.Find("Button4").gameObject.SetActive(true);
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
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
<<<<<<< HEAD
                            Transform roles = Canvas2.transform.Find("archive");
=======
                            Transform roles = Carvas2.transform.Find("archive");
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
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
<<<<<<< HEAD
            Canvas1.SetActive(false);
            Canvas2.SetActive(true);
=======
            Carvas1.SetActive(false);
            Carvas2.SetActive(true);
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
        }
        UI_Massage.SetActive(false);
    }
    public void InsertRole()
    {
<<<<<<< HEAD
        long id = sql.Count("Archive") + 1;
=======
        long id = Convert.ToInt64(sql.Count("Archive")) + 1;
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
        sql.InsertValues("Archive", new Dictionary<string, object> { ["ID"] = id, ["Name"] = Name, ["P_Name"] = P_Name });
        Player.Instance.Exp = 0;
        Player.Instance.Level = 1;
    }
    private void OnApplicationQuit()
    {
        sql.CloseConnection();
    }
<<<<<<< HEAD
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
=======
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
}
