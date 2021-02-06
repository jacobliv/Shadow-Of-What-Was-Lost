
using UnityEngine;

public struct Matter {
    public string name;
    public int index;
    public float hardness;
    public Color color;
    public TerrainType terrainType;
    public bool baseMaterial;
    public float friction;
    public float frequency;
    public bool isOre;
    public int[] veinSize;
    public string[] itemsDropped;
    public int[,] droppedItems;
    
    public Matter(string name,int index, float hardness, Color color, TerrainType terrainType, bool baseMaterial, float friction, float frequency, bool isOre, int[] veinSize, string[] itemsDropped, int[,] droppedItems) {
        this.name = name;
        this.index = index;
        this.hardness = hardness;
        this.color = color;
        this.terrainType = terrainType;
        this.baseMaterial = baseMaterial;
        this.friction = friction;
        this.frequency = frequency;
        this.isOre = isOre;
        this.veinSize = veinSize;
        this.itemsDropped = itemsDropped;
        this.droppedItems = droppedItems;
    }

    public override string ToString() {
        return "Name: " + name +
               ", Hardness: " + hardness +
               ", Color: " + color +
               ", TerrainType: " + terrainType +
               ", BaseMaterial: " + baseMaterial +
               ", Friction: " + friction +
               ", Frequency: " + frequency +
               ", Ore: " + isOre +
               ", VeinSize: " + string.Join(",",veinSize) +
               ", Items Dropped: " + string.Join(",",itemsDropped) +
               ", Dropped Items: " + getNestedList(droppedItems);
    }

    private string getNestedList(int[,] ints) {
        string val = "";
        foreach (int i in ints) {
            val += i+",";
        }
        return val;
    }
}
