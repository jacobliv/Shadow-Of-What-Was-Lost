using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEdge {
   public Vector2 point1 { get; }
   public Vector2 point2 { get; }
   
   public ExternalEdge(Vector2 point1, Vector2 point2) {
      this.point1 = point1;
      this.point2 = point2;
   }
}
