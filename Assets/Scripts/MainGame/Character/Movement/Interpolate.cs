using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolate : MonoBehaviour {
    public GameObject testObject;

    public Vector2 startPosition;

    public Vector2 endPosition;

    public float speed;

    public float currentSpeed;
    
    public InterpolationType interpolationType;
    public enum InterpolationType {
        Linear,
        Overshoot,
        SlowDown
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(startPosition.x-50,startPosition.y,0), new Vector3(startPosition.x+50,startPosition.y,0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(endPosition.x-50,endPosition.y,0), new Vector3(endPosition.x+50,endPosition.y,0));
        
    }

    // Start is called before the first frame update
    void Start() {
        testObject.transform.position = startPosition;
        this.currentSpeed = speed;
        testObject.GetComponent<Rigidbody>().AddForce(Vector3.MoveTowards(testObject.transform.position,new Vector3(endPosition.x,endPosition.y,0),currentSpeed));
        // Grapher.sharedVerticalResolution = 10;
    }

    // Update is called once per frame
    void Update() {
        // Grapher.Log(testObject.transform.position.y,"Y Position",Color.blue);
        currentSpeed = speed;
        switch (interpolationType) {
            case InterpolationType.Linear:
                linearInterpolate();
                break;
            case InterpolationType.Overshoot:
                overshoot();
                break;
            case InterpolationType.SlowDown:
                float totalDistance = startPosition.y - endPosition.y;
                float currentDistance = testObject.transform.position.y - endPosition.y;
                float percent = currentDistance / totalDistance;
                currentSpeed = -(Mathf.Pow(currentDistance,2)) +speed;
                // Debug.Log(VAR);
                linearInterpolate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        

        if (Input.GetKeyDown("r")) {
            testObject.transform.position = startPosition;
            this.currentSpeed = speed;
        }
    }

    private void overshoot() {

        currentSpeed = currentSpeed*1.03f;    
    }

    private void linearInterpolate() {
        Debug.Log(currentSpeed);
        testObject.transform.position = Vector3.MoveTowards(testObject.transform.position,
            new Vector3(endPosition.x, endPosition.y, 0),
            currentSpeed);
    }
}
