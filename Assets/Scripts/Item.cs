using UnityEngine;

namespace Player
{
    public class Item
    {
        public string uniqueID;     //for items in game that are found and inside subsequent attributes of each item.
        public string itemName;
        public string itemType;    //"tool"  "resource"  "buildMaterial"  "artifact"
        public float itemWeight;
        public float monetaryValue;
        public bool isDiscovered;
    }

    public class GearItem : Item
    {
        public string slot;     //"tank"  "fins"  "suit"  
        public int tier;
        public float depthLimit;
        
    }

    public class Artifact : Item
    {
        public string artifactType;        //"carved stone"   "fragment"   "inscription" 
        public int loreIndex;              //the mapping to the LoreEntry in GameState
        public bool isCivilizationTied;    //flags the artifact to not be sold
        public string civilizationOrigin;  //is it from "Atlantis" or "Lemuria" or "unknownOrigin"
        

    }
}