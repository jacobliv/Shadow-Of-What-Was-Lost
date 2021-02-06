using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerCamera : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Vector3 transformPosition = transform.position;
        transformPosition.x = (GameManager.SQUARE_SIZE * GameManager.CHUNK_SIZE)/2f;
        transformPosition.y = (GameManager.SQUARE_SIZE * GameManager.CHUNK_SIZE)/2f;
        transform.position = transformPosition;
    }

}
