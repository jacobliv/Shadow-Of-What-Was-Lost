using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class SetCanvasDefaults : MonoBehaviour {
    // Start is called before the first frame update
    private Size size;
    private RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        // rectTransform.rect.width = Screen.width;
        rectTransform.sizeDelta = new Vector2(Screen.width,Screen.height);
        size = new Size(Screen.width, Screen.height);
        GetComponent<RawImage>().color = Color.gray;
        // Screen.width
    }

    // Update is called once per frame
    void Update() {
        resize();

    }

    private void resize() {
        if (Screen.width != size.Width) {
            rectTransform.sizeDelta = new Vector2(Screen.width,rectTransform.sizeDelta.y);
            size.Width = Screen.width;
        }
        if (Screen.height != size.Height) {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y,Screen.height);
            size.Height = Screen.height;
        }
    }
}
