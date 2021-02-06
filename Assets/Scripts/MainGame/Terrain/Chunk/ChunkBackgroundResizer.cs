using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBackgroundResizer : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Vector3 transformLocalScale = transform.localScale;
        transformLocalScale.x = GameManager.CHUNK_SIZE * GameManager.SQUARE_SIZE;
        transformLocalScale.y = GameManager.CHUNK_SIZE * GameManager.SQUARE_SIZE;
        transform.localScale = transformLocalScale;
        
        Vector3 pos = transform.localPosition;
        pos.x = (GameManager.CHUNK_SIZE * GameManager.SQUARE_SIZE)/2f;
        pos.y = (GameManager.CHUNK_SIZE * GameManager.SQUARE_SIZE)/2f;
        transform.localPosition = pos;
    }
    
}
