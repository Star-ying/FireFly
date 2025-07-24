using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

public class SQLiteHelper
{
    /// <summary>
    /// 数据库连接定义
    /// </summary>
    private SqliteConnection dbConnection;

    /// <summary>
    /// SQL命令定义
    /// </summary>
    private SqliteCommand dbCommand;

    /// <summary>
    /// 数据读取定义
    /// </summary>
    private SqliteDataReader dataReader;

    /// <summary>
    /// 构造函数    
    /// </summary>
    /// <param name="connectionString">数据库连接字符串</param>
    public SQLiteHelper(string connectionString)
    {
        try
        {
            //构造数据库连接
            dbConnection = new SqliteConnection(connectionString);
            //打开数据库
            dbConnection.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 执行SQL命令
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL命令字符串</param>
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryString;
        dataReader = dbCommand.ExecuteReader();
        return dataReader;
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void CloseConnection()
    {
        //销毁Command
        if (dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;

        //销毁Reader
        if (dataReader != null)
        {
            dataReader.Close();
        }
        dataReader = null;

        //销毁Connection
        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
    }

    /// <summary>
    /// 读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public void InsertValues(string tableName, Dictionary<string, object> data)
    {
        // 输入验证
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("表名不能为空");
        if (data == null || data.Count == 0)
            throw new ArgumentException("插入数据不能为空");

        // 构建参数化SQL
        var(sql, parameters) = BuildParameterizedSql(tableName, data);
        dbCommand = dbConnection.CreateCommand();
        // 添加参数
        foreach (var param in parameters)
        {
            dbCommand.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
        }
        dbCommand.CommandText = sql;
        dbCommand.ExecuteNonQuery();
    }
    /// <summary>
    /// 构造参数化SQL语句
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="data">数据</param>
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
                throw new ArgumentException("请补充完整数据");
            }
            columns.Add(dataReader.GetString(1));

            // 生成参数名
            string paramName = $"@p{paramIndex++}";
            values.Add(paramName);
            parameters.Add(paramName, value);
        }

        string sql = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) " +
                     $"VALUES ({string.Join(", ", values)})";

        return (sql, parameters);
    }

    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
    {
        //当字段名称和字段数值不对应时引发异常
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
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValues(string tableName, string[] colNames, string[] operations, string[] colValues,string[] relation)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || relation.Length + 1 != operations.Length)
        {
            throw new SqliteException("字符不匹配");
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
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
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
    /// 查找表信息
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="items">搜索目标</param>
    /// <param name="colNames">属性名</param>
    /// <param name="operations">操作符</param>
    /// <param name="colValues">属性值</param>
    /// <param name="relation">语句关系</param>
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
    /// 计算数据总量
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public long Count(string tableName)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = $"SELECT COUNT(*) FROM {tableName}";
        dataReader = dbCommand.ExecuteReader();
        return (long)dataReader.GetValue(0);
    }
    /// <summary>
    /// 获取结构表
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public SqliteDataReader ReadItem(string tableName)
    {
        return ExecuteQuery($"PRAGMA table_info({tableName})");
    }
}
