using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class Login : MonoBehaviour
{
    private SQLiteHelper sql;
    private SqliteDataReader reader;
    public InputField Name;
    public InputField Password;
    public GameObject UI_Massage;
    public GameObject Carvas1;
    public GameObject Carvas2;
    private void OnEnable()
    {
        sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/Sqlist.db");
        reader = sql.ReadFullTable("Player");
        if (!Name.text.Equals("") || !Password.text.Equals(""))
        {
            Examine();
        }
        else
        {
            StartCoroutine(massage("«Î ‰»Î’À∫≈√‹¬Î"));
        }
    }
    private void Examine()
    {
        while (reader.Read())
        {
            if(reader.GetString(reader.GetOrdinal("Name")).Equals(Name.text))
            {
                if(reader.GetString(reader.GetOrdinal("Password")).Equals(Password.text))
                {
                    StartCoroutine(massage("µ«¬º≥…π¶"));
                    return;
                }
                StartCoroutine(massage("√‹¬Î¥ÌŒÛ"));
                return;
            }
        }
        StartCoroutine(massage("ƒ„ªπ√ª”–◊¢≤·"));
        return;
    }
    IEnumerator massage(string str)
    {
        UI_Massage.transform.Find("Text").GetComponent<Text>().text = str;
        UI_Massage.SetActive(true);
        if (str.Equals("µ«¬º≥…π¶"))
        {
            sql.CloseConnection();
            Carvas1.SetActive(false);
            Carvas2.SetActive(true);
            GameManager.Instance.StartGame();
        }
        yield return new WaitForSeconds(1f);
        UI_Massage.SetActive(false);
        gameObject.SetActive(false);
    }
}
