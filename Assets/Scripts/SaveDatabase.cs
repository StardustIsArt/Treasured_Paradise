using UnityEngine;
using SQLite;
using System.IO;

public class Player
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; } = 1;
    public float posX { get; set; }
    public float posY { get; set; }
    public float posZ { get; set; }
}
    
public class InventoryItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int itemId { get; set; }
    public int Quantity { get; set; }
}
public class SaveDatabase : MonoBehaviour
{
    SQLiteConnection db;

    void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.db");
        Debug.Log("Database path: " + path);
        db = new SQLiteConnection(path);
        
        db.CreateTable<Player>();
        db.CreateTable<InventoryItem>();
    }
    public void SavePlayer(Player player)
    {
        db.InsertOrReplace(player);
    }

    public Player LoadPlayer(int id)
    {
        return db.Find<Player>(id);
    }
    void OnDestroy()
    {
        db?.Close();
    }
}


