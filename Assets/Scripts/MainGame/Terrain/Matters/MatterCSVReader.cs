using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MainGame.Terrain.Generation;
using UnityEngine;

public class MatterCSVReader : MonoBehaviour {
    public TerrainMatter terrainMatter;
    private Stack<char> arrayStack;
    void Awake() {
        arrayStack = new Stack<char>();
        int i = 0;
        using (StreamReader sr = new StreamReader("./assets/modularCsv/Matters.csv")) {
            string currentLine;
            // currentLine will be null when the StreamReader reaches the end of file
            while((currentLine = sr.ReadLine()) != null) {
                if (i == 0) { 
                    i++;
                    continue;
                }

                List<string> line = parseLine(currentLine);
                Matter matter = matterCreator(line,i);
                terrainMatter.addMatter(matter,i);
                i++;
            }
        }
    }

    private Matter matterCreator(List<string> line, int i) {
        Color color = getColor(line[2]);
        TerrainType terrainType = getTerrainType(line[3]);

        return new Matter(line[0],
                          i,
                        float.Parse(line[1]),
                        color,
                        terrainType,
                        Boolean.Parse(line[4]),
                        float.Parse(line[5]),
                        float.Parse(line[6]),
                        Boolean.Parse(line[7]),
                        getVeinSize(line[8]),
                        getItemsDropped(line[9]),
                        getDroppedItems(line[10])
                        );
    }
    //1,1,1,5
    private int[,] getDroppedItems(string s) {
        Stack<char> bracketStack = new Stack<char>();
        char[] charArray = s.ToCharArray();
        List<string> outerArray = new List<string>();
        string val = "";
        int i = 0;
        foreach (char c in charArray) {
            if (c == '[') {
                bracketStack.Push('[');
                i++;
                continue;
            } else if (c == ']') {
                bracketStack.Pop();
                if (i == charArray.Length-1) {
                    outerArray.Add(val);
                    val = "";    
                }
                i++;
                continue;
            }
            
            if (bracketStack.Count == 1 && c == ',') {
                outerArray.Add(val);
                val = "";
                i++;
                continue;
            }

            val += c;
            i++;
        }
        int[,] returnVals = new int[outerArray.Count,2];
        i = 0;
        foreach (string inner in outerArray) {
            string[] strings = inner.Split(',');
            for (int j = 0; j < 2; j++) {
                returnVals[i,j] = int.Parse(strings[j]);

            }
        }
        return returnVals;
    }

    private string[] getItemsDropped(string s) {
        char[] charArray = s.ToCharArray();
        string val = "";
        foreach (char c in charArray) {
            if(c == '[' || c == ']') continue;
            val+= c +"";
        }

        return val.Split(',');
    }

    private int[] getVeinSize(string veinSize) {
        if (veinSize == "Empty") return new int[0];
        char[] charArray = veinSize.ToCharArray();
        string[] values = new string[2];
        int i = 0;
        foreach (char c in charArray) {
            if(c == '[' || c == ']' || c == ',') continue;
            values[i] = c +"";
            i++;
        }
        int[] ints = Array.ConvertAll(values, s => int.Parse(s));
        return ints;
    }

    private TerrainType getTerrainType(string tType) {
        Enum.TryParse(tType, out TerrainType terrainType);
        return terrainType;
    }

    private Color getColor(string hex) {
        Color newCol;

        if (ColorUtility.TryParseHtmlString(hex, out newCol))
            return newCol;
        return Color.cyan;
    }

    // Iron-Ore,0.6,#e3c29fff,SOLID,FALSE,0.7,0.01,TRUE,"[1,4]",[Iron-Ore],"[[1,1]]"
    private List<string> parseLine(string line) {
        // Debug.Log(line);
        line += ",";
        char[] chars = line.ToArray();
        List<string> parsedValues = new List<string>();
        string currentValue = "";
        for (int i = 0; i < chars.Length; i+=1) {
            if (chars[i] == '"') {
                continue;
            }
            if (chars[i] == '[') {
                arrayStack.Push('[');
            } else if (chars[i] == ']') {
                arrayStack.Pop();
            }

            if (chars[i] != ',' || arrayStack.Count != 0) {
                currentValue += chars[i];
            }

            if ((chars[i] == ',' /*|| chars[i] == ']'*/) && arrayStack.Count == 0) {
                // Debug.Log("Value added: " + currentValue);
                if (currentValue == "") currentValue = "Empty";
                parsedValues.Add(currentValue);
                currentValue = "";
                continue;
            }
        }

        return parsedValues;
    }
}
