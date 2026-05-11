using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerData
    {
        public string PlayerName;
        
        //Inventory
        public List<Item> Inventory = new List<Item>();
        public Dictionary<string, GearItem> EquippedGear = new Dictionary<string, GearItem>();

        //Ecomony
        public float currencyBalance;
        
        //Oxygen
        public float OxygenMax;
        public float OxygenCurrent;
        public readonly OxygenTier CurrentOxygenTier = OxygenTier.Snorkel;
        
        //Health Level
        public float healthMeter;


    }
    
    public enum OxygenTier { Snorkel, BasicScuba, Rebreather }
}