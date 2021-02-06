using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Terrain.Generation {
     public class TerrainMatter : MonoBehaviour {
          public Dictionary<String, Matter> allMatters { get; set; }
          public Dictionary<String, Matter> baseMatters { get; set; }
          public Dictionary<String, Matter> oreMatters { get; set; }
          public Dictionary<int, Matter> indexedMatters { get; set; }

          public TerrainMatter() {
               instantiate();
          }

          private void Awake() {
               instantiate();
          }

          private void instantiate() {
               if (allMatters == null) {
                    allMatters = new Dictionary<string, Matter>();
                    baseMatters = new Dictionary<string, Matter>();
                    oreMatters = new Dictionary<string, Matter>();
                    indexedMatters = new Dictionary<int, Matter>();
               }
          }

          public Matter getMatter(string name) {
               return allMatters[name];
          }

          public Matter getBaseMatter(string name) {
               return baseMatters[name];
          }

          public Matter getOreMatter(string name) {
               return oreMatters[name];
          }

          public Matter getIndexedMatter(int i) {
               return indexedMatters[i];
          }
     
     
          /// <summary>
          /// <param name="matter">The matter to be added to the TerrainMatters lists</param>
          /// <param name="i">The index of the matter, to allow for quick indexing</param>
          /// <returns></returns>
          /// </summary>
          public void addMatter(Matter matter, int i) {
               if(allMatters == null) instantiate();
               allMatters.Add(matter.name,matter);
               indexedMatters.Add(i,matter);
               if (matter.baseMaterial) {
                    baseMatters.Add(matter.name, matter);
               } else if (matter.isOre) {
                    oreMatters.Add(matter.name,matter);
               }
          
          }
     }
}
