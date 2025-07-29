using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

public class SQLiteHelper
{
    /// <summary>
    /// ���ݿ����Ӷ���
    /// </summary>
    private SqliteConnection dbConnection;

    /// <summary>
    /// SQL�����
    /// </summary>
    private SqliteCommand dbCommand;

    /// <summary>
    /// ���ݶ�ȡ����
    /// </summary>
    private SqliteDataReader dataReader;

    /// <summary>
    /// ���캯��    
    /// </summary>
    /// <param name="connectionString">���ݿ������ַ���</param>
    public SQLiteHelper(string connectionString)
    {
        try
        {
            //�������ݿ�����
            dbConnection = new SqliteConnection(connectionString);
            //�����ݿ�
            dbConnection.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// ִ��SQL����
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL�����ַ���</param>
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryString;
        dataReader = dbCommand.ExecuteReader();
        return dataReader;
    }

    /// <summary>
    /// �ر����ݿ�����
    /// </summary>
    public void CloseConnection()
    {
        //����Command
        if (dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;

        //����Reader
        if (dataReader != null)
        {
            dataReader.Close();
        }
        dataReader = null;

        //����Connection
        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
    }

    /// <summary>
    /// ��ȡ�������ݱ�
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">���ݱ�����</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">���ݱ�����</param>
    /// <param name="values">�������ֵ</param>
    public void InsertValues(string tableName, Dictionary<string, object> data)
    {
        // ������֤
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("��������Ϊ��");
        if (data == null || data.Count == 0)
            throw new ArgumentException("�������ݲ���Ϊ��");

        // ����������SQL
        var(sql, parameters) = BuildParameterizedSql(tableName, data);
        dbCommand = dbConnection.CreateCommand();
        // ��Ӳ���
        foreach (var param in parameters)
        {
            dbCommand.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
        }
        dbCommand.CommandText = sql;
        dbCommand.ExecuteNonQuery();
    }
    /// <summary>
    /// ���������SQL���
    /// </summary>
    /// <param name="tableName">����</param>
    /// <param name="data">����</param>
    /// <returns></returns>
    private (string sql, Dictionary<string, object> parameters)
        BuildParameterizedSql(string tableName, Dictionary<string, object> data)
    {
        var columns = new List<string>();
        var values = new List<string>();
        var parameters = new Dictionary<string, object>();
        int paramIndex = 0;
        dataReader = ReadItem(tableName);
        while(dataReader.Read())
        {
            object value = DBNull.Value;
            if(data.ContainsKey(dataReader.GetString(1)))
            {
                value = data[dataReader.GetString(1)];
            }
            else if (Convert.ToString(dataReader.GetValue(4)) != "")
            {
                value = dataReader.GetValue(4);
            }
            else if (Convert.ToString(dataReader.GetValue(5)) == "1")
            {
                throw new ArgumentException("�벹����������");
            }
            columns.Add(dataReader.GetString(1));

            // ���ɲ�����
            string paramName = $"@p{paramIndex++}";
            values.Add(paramName);
            parameters.Add(paramName, value);
        }

        string sql = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) " +
                     $"VALUES ({string.Join(", ", values)})";

        return (sql, parameters);
    }

    /// <summary>
    /// ����ָ�����ݱ��ڵ�����
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">���ݱ�����</param>
    /// <param name="colNames">�ֶ���</param>
    /// <param name="colValues">�ֶ�����Ӧ������</param>
    /// <param name="key">�ؼ���</param>
    /// <param name="value">�ؼ��ֶ�Ӧ��ֵ</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
    {
        //���ֶ����ƺ��ֶ���ֵ����Ӧʱ�����쳣
        if (colNames.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length");
        }
        List<string> str = new List<string>();
        for (int i = 0; i < colValues.Length; i++)
        {
            str.Add($"{colNames[i]} = {colValues[i]}");
        }
        string queryString = $"UPDATE {tableName} " + $"SET {string.Join(" ", str)}" + $"{key} {operation} {value}";

        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// ɾ��ָ�����ݱ��ڵ�����
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">���ݱ�����</param>
    /// <param name="colNames">�ֶ���</param>
    /// <param name="colValues">�ֶ�����Ӧ������</param>
    public SqliteDataReader DeleteValues(string tableName, string[] colNames, string[] operations, string[] colValues,string[] relation)
    {
        //���ֶ����ƺ��ֶ���ֵ����Ӧʱ�����쳣
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || relation.Length + 1 != operations.Length)
        {
            throw new SqliteException("�ַ���ƥ��");
        }

        List<string> str = new List<string>();
        str.Add($"{colNames[0]} {operations[0]} {colValues[0]}");
        for (int i = 1; i < colNames.Length; i++)
        {
            str.Add($"{relation[i - 1]} {colNames[i]} {operations[i]} {colValues[i]}");
        }
        string queryString = $"DELETE FROM {tableName} " + $"WHERE {string.Join(" ", str)}";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// �������ݱ�
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">���ݱ���</param>
    /// <param name="colNames">�ֶ���</param>
    /// <param name="colTypes">�ֶ�������</param>
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        try
        {
            string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }

    /// <summary>
    /// ���ұ���Ϣ
    /// </summary>
    /// <param name="tableName">����</param>
    /// <param name="items">����Ŀ��</param>
    /// <param name="colNames">������</param>
    /// <param name="operations">������</param>
    /// <param name="colValues">����ֵ</param>
    /// <param name="relation">����ϵ</param>
    /// <returns></returns>
    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues,string[] relation)
    {
        List<string> str = new List<string>();
        str.Add($"{colNames[0]} {operations[0]} '{colValues[0]}'");
        for (int i = 1; i < colNames.Length; i++)
        {
             str.Add($"{relation[i - 1]} {colNames[i]} {operations[i]} '{colValues[i]}'");
        }
        string queryString = $"SELECT {string.Join(", ", items)} FROM {tableName} " + $"WHERE {string.Join(" ", str)}";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="tableName">����</param>
    /// <returns></returns>
    public long Count(string tableName)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = $"SELECT COUNT(*) FROM {tableName}";
        dataReader = dbCommand.ExecuteReader();
        return (long)dataReader.GetValue(0);
    }
    /// <summary>
    /// ��ȡ�ṹ��
    /// </summary>
    /// <param name="tableName">����</param>
    /// <returns></returns>
    public SqliteDataReader ReadItem(string tableName)
    {
        return ExecuteQuery($"PRAGMA table_info({tableName})");
    }
}
