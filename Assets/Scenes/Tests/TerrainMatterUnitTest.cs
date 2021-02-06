using System.Collections;
using System.Collections.Generic;
using MainGame.Terrain.Generation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class TerrainMatterUnitTest {
        public static IEnumerable<TestCaseData> addMatterSuccess
        {
            get
            {
                yield return new TestCaseData(createMatter("Dirt",true)).SetName("Success -- addMatter() -- add base matter -- matter is added to all associated lists");
                yield return new TestCaseData(createMatter("Gold",false)).SetName("Success -- addMatter() -- add ore matter -- matter is added to all associated lists");
            }
        }
        [TestCaseSource("addMatterSuccess")]

        public void addMatter(Matter matter) {
            TerrainMatter terrainMatter = new GameObject().AddComponent<TerrainMatter>().GetComponent<TerrainMatter>();
            
            terrainMatter.addMatter(matter,matter.index);
            Assert.AreEqual(terrainMatter.getMatter(matter.name),matter);
            Assert.AreEqual(terrainMatter.getIndexedMatter(matter.index),matter);
            if(matter.baseMaterial) {
                Assert.AreEqual(terrainMatter.getBaseMatter(matter.name),matter);
                Assert.Throws(typeof(KeyNotFoundException), delegate { terrainMatter.getOreMatter(matter.name); });
            } else if (matter.isOre) {
                Assert.AreEqual(terrainMatter.getOreMatter(matter.name),matter);
                Assert.Throws(typeof(KeyNotFoundException), delegate { terrainMatter.getBaseMatter(matter.name); });

            }
        }

        private static Matter createMatter(string name, bool baseMatter) {
            return new Matter(name,
                0,
                0.0f,
                Color.black,
                TerrainType.SOLID,
                baseMatter,
                0.0f,
                0.0f,
                !baseMatter,
                new int[0],
                new string[0],
                new int[0,0]);
        }
        
    }
}
