using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using Random = System.Random;
=======
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21

public class SQLiteDemo : MonoBehaviour
{
    /// <summary>
    /// SQLite���ݿ⸨����
    /// </summary>
    private SQLiteHelper sql;

    void Start()
    {
<<<<<<< HEAD
        sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/SQLite.db");
        SqliteDataReader reader = sql.ReadTable("Equipment", new string[] { "*" }, new string[] { "R_ID" }, new string[] { "=" }, new string[] { $"{1}" }, new string[] { });
        /*
        //װ��Ч��ʹ�ù淶ʾ��
        //��#�Ŷ�������˵������ȼ��仯��û�о��ǹ̶���
        int hp = 0;
        int enemy_effect = 1;
        string str = "��������#1#��������#2#%���ʱ���(���˹̶�50%)����������#3#��ʵ�˺�@100@20@10";
        string[] s = str.Split("@");
        string[] c = s[0].Split("#");
        string str1 = "";
        for(int i = 0; i < c.Length; i++)
        {
            try
            {
                str1 += s[Convert.ToInt32(c[i])];
            }
            catch (Exception)
            {
                str1 += c[i];
            }
        }
        hp += Convert.ToInt32(c[1]);
        Random random = new Random();
        if(random.NextDouble() < Convert.ToSingle(c[2]) * 0.01)
        {
            enemy_effect = (int)(enemy_effect * 1.5);
        }
        enemy_effect += Convert.ToInt32(c[3]);
        Debug.Log(str1);
        */
        /*      //ԭHelper���޸ģ���Ҫ�Լ������ݽ����޸�
                //������Ϊsqlite4unity�����ݿ�
                sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/sqlite4unity.db");

                //������Ϊtable1�����ݱ�
                sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });

                //������������
                sql.InsertValues("table1", new string[] { "'1'", "'����'", "'22'", "'Zhang3@163.com'" });
                sql.InsertValues("table1", new string[] { "'2'", "'����'", "'25'", "'Li4@163.com'" });

                //�������ݣ���Name="����"�ļ�¼�е�Name��Ϊ"Zhang3"
                sql.UpdateValues("table1", new string[] { "Name" }, new string[] { "'Zhang3'" }, "Name", "=", "'����'");

                //����3������
                sql.InsertValues("table1", new string[] { "3", "'����'", "25", "'Wang5@163.com'" });
                sql.InsertValues("table1", new string[] { "4", "'����'", "26", "'Wang5@163.com'" });
                sql.InsertValues("table1", new string[] { "5", "'����'", "27", "'Wang5@163.com'" });

                //ɾ��Name="����"��Age=26�ļ�¼,DeleteValuesOR��������
                sql.DeleteValuesAND("table1", new string[] { "Name", "Age" }, new string[] { "=", "=" }, new string[] { "'����'", "'26'" });

                //��ȡ���ű�
                SqliteDataReader reader = sql.ReadFullTable("table1");
                while (reader.Read())
                {
                    //��ȡID
                    Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
                    //��ȡName
                    Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
                    //��ȡAge
                    Debug.Log(reader.GetInt32(reader.GetOrdinal("Age")));
                    //��ȡEmail
                    Debug.Log(reader.GetString(reader.GetOrdinal("Email")));
                }

                //��ȡ���ݱ���Age>=25�����м�¼��ID��Name
                reader = sql.ReadTable("table1", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
                while (reader.Read())
                {
                    //��ȡID
                    Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
                    //��ȡName
                    Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
                }

                //�Զ���SQL,ɾ�����ݱ�������Name="����"�ļ�¼
                sql.ExecuteQuery("DELETE FROM table1 WHERE NAME='����'");
        */
=======
        //������Ϊsqlite4unity�����ݿ�
        sql = new SQLiteHelper("data source=" + Application.dataPath + "/StreamingAssets/sqlite4unity.db");

        //������Ϊtable1�����ݱ�
        sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });

        //������������
        sql.InsertValues("table1", new string[] { "'1'", "'����'", "'22'", "'Zhang3@163.com'" });
        sql.InsertValues("table1", new string[] { "'2'", "'����'", "'25'", "'Li4@163.com'" });

        //�������ݣ���Name="����"�ļ�¼�е�Name��Ϊ"Zhang3"
        sql.UpdateValues("table1", new string[] { "Name" }, new string[] { "'Zhang3'" }, "Name", "=", "'����'");

        //����3������
        sql.InsertValues("table1", new string[] { "3", "'����'", "25", "'Wang5@163.com'" });
        sql.InsertValues("table1", new string[] { "4", "'����'", "26", "'Wang5@163.com'" });
        sql.InsertValues("table1", new string[] { "5", "'����'", "27", "'Wang5@163.com'" });

        //ɾ��Name="����"��Age=26�ļ�¼,DeleteValuesOR��������
        sql.DeleteValuesAND("table1", new string[] { "Name", "Age" }, new string[] { "=", "=" }, new string[] { "'����'", "'26'" });

        //��ȡ���ű�
        SqliteDataReader reader = sql.ReadFullTable("table1");
        while (reader.Read())
        {
            //��ȡID
            Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
            //��ȡName
            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
            //��ȡAge
            Debug.Log(reader.GetInt32(reader.GetOrdinal("Age")));
            //��ȡEmail
            Debug.Log(reader.GetString(reader.GetOrdinal("Email")));
        }

        //��ȡ���ݱ���Age>=25�����м�¼��ID��Name
        reader = sql.ReadTable("table1", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
        while (reader.Read())
        {
            //��ȡID
            Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
            //��ȡName
            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
        }

        //�Զ���SQL,ɾ�����ݱ�������Name="����"�ļ�¼
        sql.ExecuteQuery("DELETE FROM table1 WHERE NAME='����'");
>>>>>>> e680e54f127fbcafce6c9017202eedf364e17e21
    }
}
//��ƽ̨�����ݿ�洢�ľ���·��(ͨ��)
//PC��sql = new SQLiteHelper("data source=" + Application.dataPath + "/sqlite4unity.db");
//Mac��sql = new SQLiteHelper("data source=" + Application.dataPath + "/sqlite4unity.db");
//Android��sql = new SQLiteHelper("URI=file:" + Application.persistentDataPath + "/sqlite4unity.db");
//iOS��sql = new SQLiteHelper("data source=" + Application.persistentDataPath + "/sqlite4unity.db");

//PCƽ̨�µ����·��
//sql = new SQLiteHelper("data source="sqlite4unity.db");
//�༭����Assets/sqlite4unity.db
//����󣺺�AppName.exeͬ����Ŀ¼�£�����Ƚ�����
//��Ȼ�����ø�����ķ�ʽsql = new SQLiteHelper("data source="D://SQLite//sqlite4unity.db");
//ȷ��·�����ڼ��ɷ���ᷢ������

//��������ȴ�����һ�����ݿ�
//���Խ�������ݿ������StreamingAssetsĿ¼��Ȼ���ٿ�����
//Application.persistentDataPath + "/sqlite4unity.db"·������