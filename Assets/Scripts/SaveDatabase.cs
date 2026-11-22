using UnityEngine;
using SQLite;

public class SaveDatabase
{
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
    
    public class Inventory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int itemId { get; set; }
        public int Quantity { get; set; }
    }
}
