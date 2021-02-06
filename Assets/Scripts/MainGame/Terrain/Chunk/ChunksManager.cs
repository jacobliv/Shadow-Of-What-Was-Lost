using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksManager : MonoBehaviour {
   private Dictionary<Vector2, GenerationData> chunkData;


   private void Start() {
      chunkData = new Dictionary<Vector2, GenerationData>();
   }

   public void addChunkData(Vector2 pos, GenerationData data) {
      chunkData.Add(pos,data);
   }

   public Optional<GenerationData> get(Vector2 pos) {
      if(chunkData == null) Start();
      if (chunkData.ContainsKey(pos)) {
         return Optional<GenerationData>.of(chunkData[pos]);
      }
      return Optional<GenerationData>.empty();
   }

   public void removeChunkData(Vector2 pos, GenerationData data) {
      if (chunkData.ContainsKey(pos)) {
         chunkData.Remove(pos);
      }
   }
   
}
