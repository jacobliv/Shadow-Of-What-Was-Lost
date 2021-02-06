using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInOut : MonoBehaviour {
    private RectTransform rectTransform;

    public float maxX = 40;

    public float minX = -32;

    [SerializeField]
    [Range(1,100)]
    public float slideSpeed;
    // Start is called before the first frame update
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 position = transform.position;
        Vector3 mousePosition = Input.mousePosition;
        if (inside(mousePosition) && transform.position.x < maxX) {
            position.x+=slideSpeed;
            transform.position = position;
        }
        else if(!inside(mousePosition) && transform.position.x > minX) {
            position.x-=slideSpeed;
            transform.position = position;
        }

        if (position.x > maxX) {
            position.x = maxX;
            transform.position = position;

        } else if (position.x < minX) {
            position.x = minX;
            transform.position = position;

        }
    }

    private bool inside(Vector3 mousePosition) {
        var position = transform.position;
        var size = rectTransform.rect;
        return mousePosition.x >= position.x - size.width/2f &&
               mousePosition.x <= position.x + size.width/2f &&
               mousePosition.y >= position.y - size.height/2f &&
               mousePosition.y <= position.y + size.height/2f;
    }
}
