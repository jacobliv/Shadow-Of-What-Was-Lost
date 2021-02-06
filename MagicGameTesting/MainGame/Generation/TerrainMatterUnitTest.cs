﻿using System.Drawing;
using MainGame.Terrain.Generation;
using NUnit.Framework;
using Color = UnityEngine.Color;
using FluentAssertions;

namespace MagicGameTesting.MainGame.Generation {
    public class TerrainMatterUnitTest {
        [TestCase(TestName = "Success -- addMatter() -- add base Matter -- adds matter to associated list")]
        public void addMatter() {
            TerrainMatter terrainMatter = new TerrainMatter();
            Matter matter = new Matter("Dirt", 0,
                0.0f,
                Color.black, 
                TerrainType.SOLID,
                true,
                0.0f,
                0.0f,
                false,
                new int[0],
                new string[0],
                new int[0,0]);
            terrainMatter.addMatter(matter,0);
            terrainMatter.getMatter(matter.name).Should().Be(matter);
        }
    }
}