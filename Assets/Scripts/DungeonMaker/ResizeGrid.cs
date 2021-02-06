using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ResizeGrid : MonoBehaviour {
    private RectTransform rectTransform;
    private Size size;
    [SerializeField]
    public Margin margin;
    // Start is called before the first frame update
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        // rectTransform.rect.width = Screen.width;
        rectTransform.sizeDelta = new Vector2(Screen.width-(margin.left+margin.right),Screen.height - (margin.top + margin.bottom));
        Vector3 transformPosition = rectTransform.transform.position;
        size = new Size(Screen.width-(margin.left+margin.right),Screen.height - (margin.top + margin.bottom));
        int sizeWidth = Screen.width - size.Width;
        transformPosition.x = sizeWidth;
    }

    // Update is called once per frame
    void Update() {
        resize();

    }

    private void resize() {
        // if (Screen.width != size.Width) {
        //     rectTransform.sizeDelta = new Vector2(Screen.width,rectTransform.sizeDelta.y);
        //     size.Width = Screen.width;
        // }
        // if (Screen.height != size.Height) {
        //     rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y,Screen.height);
        //     size.Height = Screen.height;
        // }
    }
}
