using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour {
    public GameObject player;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x,playerPosition.y,-10);
    }
}
