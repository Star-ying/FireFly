using SQLite4Unity3d;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    void Start()
    {
        string path = Application.streamingAssetsPath + "/data.db";
        SQLiteConnection connection = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // 创建表
        connection.CreateTable<TestTable>();

        // 插入数据
        TestTable table = new TestTable
        {
            Name = "Example"
        };
        connection.Insert(table);

        // 查询数据
        var query = connection.Table<TestTable>().Where(v => v.Name == "Example");
        foreach (var item in query)
        {
            Debug.Log(item.Name);
        }
    }
}

public class TestTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
}