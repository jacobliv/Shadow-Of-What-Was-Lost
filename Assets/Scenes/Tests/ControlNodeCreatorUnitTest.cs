using System.Drawing;
using MainGame.Terrain.Generation;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace Tests {
    public class ControlNodeCreatorUnitTest {
        [Test]
        public void generate() {
            Bounds bounds = new Bounds(new Vector2(0,0), new Size(GameManager.SQUARE_SIZE *3,GameManager.SQUARE_SIZE *3) );
            
        }
    }
}