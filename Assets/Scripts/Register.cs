using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class Register : MonoBehaviour
{
    private SQLiteHelper sql;
    private SqliteDataReader reader;
    public InputField Name;
    public InputField Password;
    public InputField cir_Password;
    public GameObject UI_Massage;
    public GameObject Carvas1;
    public GameObject Carvas2;
    private void OnEnable()
    {
        sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/Sqlist.db");
        reader = sql.ReadFullTable("Player");
        if (!Name.text.Equals("") && !Password.text.Equals("") && !cir_Password.text.Equals(""))
        {
            while (reader.Read())
            {
                if (reader.GetString(reader.GetOrdinal("Name")).Equals(Name.text))
                {
                    StartCoroutine(massage("用户名已存在"));
                    return;
                }
                else if (!Password.text.Equals(cir_Password.text))
                {
                    StartCoroutine(massage("两次输入密码不一致"));
                    return;
                }
            }
            StartCoroutine(massage("注册成功"));
            return;
        }
        else
        {
            StartCoroutine(massage("请填写完整"));
            return;
        }
    }
    IEnumerator massage(string str)
    {
        UI_Massage.transform.Find("Text").GetComponent<Text>().text = str;
        UI_Massage.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (str.Equals("注册成功"))
        {
            sql.InsertValues("Player", new string[] { Name.text, Password.text});
            sql.CloseConnection();
            Carvas1.SetActive(false);
            Carvas2.SetActive(true);
            GameManager.Instance.StartGame();
        }
        UI_Massage.SetActive(false);
        gameObject.SetActive(false);
    }
}
