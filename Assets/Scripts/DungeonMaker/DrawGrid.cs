using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DrawGrid : MonoBehaviour, IDragHandler{
    public GameManager GameManager;
    private RawImage rawImage;
    private Color clear;
   

    private Size size;
    private Texture2D texture;
    private Dictionary<Vector2, Sqr> squares;
    private bool buttonDown;
    private Vector2? previousPoint;
    private Vector2? current;

    private void OnDrawGizmos() {
        if (current != null && previousPoint != null) {
            Gizmos.color = Color.cyan;
            
            Gizmos.DrawLine(new Vector3(previousPoint.Value.x,previousPoint.Value.y,5), new Vector3(current.Value.x,current.Value.y,5));
            Gizmos.DrawLine(new Vector3(previousPoint.Value.x-5,previousPoint.Value.y,5), new Vector3(current.Value.x-5,current.Value.y,5));

        }
    }

    // Start is called before the first frame update
    void Start() {
        rawImage = GetComponent<RawImage>();

        texture = new Texture2D(Screen.width, Screen.height);
        rawImage.texture = texture;
        int widthSquares = Screen.width / GameManager.SQUARE_SIZE;
        int heightSquares = Screen.height / GameManager.SQUARE_SIZE;
        squares = new Dictionary<Vector2, Sqr>();
        for (int x = 0; x < widthSquares; x++) {
            for (int y = 0; y < heightSquares; y++) {
                Vector2Int pos = new Vector2Int(x*GameManager.SQUARE_SIZE,y*GameManager.SQUARE_SIZE);
                Sqr sqr = new Sqr(pos,new Size(GameManager.SQUARE_SIZE,GameManager.SQUARE_SIZE),Color.clear);
                squares.Add(pos,sqr);
            }
        }
    }
    

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            buttonDown = true;
            previousPoint = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0)) {
            buttonDown = false;
            // previousPoint = null;
        }

        if (buttonDown) {
            


        }
        foreach (Sqr sqr in squares.Values) {
            bool mouseInside = checkInside(sqr);
            drawSquare(sqr.pos.x,sqr.pos.y,sqr.size.Width,sqr.size.Height,sqr.color != Color.clear?sqr.color:Color.black,mouseInside || sqr.color!=Color.clear);
        }

        texture.Apply();
    }

    private bool checkInside(Sqr sqr) {
        Vector3 pos = Input.mousePosition;
        double x = Math.Floor(pos.x / GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
        double y = Math.Floor(pos.y/GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
        return sqr.pos.x == (int)x && sqr.pos.y == (int)y;
    }

    void drawSquare(int x, int y, int dx, int dy, Color color, bool fill = false) {
        for (int x1 = x; x1 <= x + dx; x1++) {
            for (int y1 = y; y1 <= y + dy; y1++) {
                if (!fill) {
                    if(x1 == x || y1 == y || x1 == (x+dx) || y1 == (y+dy)) 
                        texture.SetPixel(x1,y1,color);
                    else {
                        texture.SetPixel(x1, y1, Color.clear);
                    }
                    continue;
                }
                texture.SetPixel(x1,y1,color);

            }
        }
    }

    HashSet<Vector2> getPoints(int x1, int y1, int x2, int y2) {
        float angle = Mathf.Atan2(y2-y1,x2-x1);
        int distance = (int)Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2))/(GameManager.SQUARE_SIZE/6);
        HashSet<Vector2> points = new HashSet<Vector2>();
        for (int i = 0; i < distance; i++) {
            float x = Mathf.Cos(angle)*i + x1;
            float y = Mathf.Sin(angle)*i + y1;
            x = (float)Math.Floor(x / GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
            y = (float)Math.Floor(y/GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
            Vector2 p = new Vector2((float)x,(float)y);
            points.Add(new Vector2(x, y));
            Debug.Log(new Vector2(x,y));
        }

        Debug.Log(points.Count);
        return points;
    }

    public void setTool(ToolType type) {
        
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("VAR");
        Vector2 pos = eventData.position;
        float x = (float)Math.Floor(pos.x / GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
        float y = (float)Math.Floor(pos.y/GameManager.SQUARE_SIZE) * GameManager.SQUARE_SIZE;
        current = pos;
        Debug.Log(pos);
        // Vector2 p = new Vector2(x,y);
        HashSet<Vector2> hashSet = getPoints((int)pos.x, (int)pos.y, (int)previousPoint.Value.x, (int)previousPoint.Value.y);
        foreach (Vector2 p in hashSet) {
           if (squares.ContainsKey(p)) {
                Sqr sqr = squares[p];
                sqr.color=Color.red;
                squares[p]= sqr;
         }
         else {
            
        }

        }
        previousPoint = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
    }
}


struct Sqr {
    public Vector2Int pos { get; set; }
    public Size size { get; set; }
    public Color color { get; set; }
    public Sqr(Vector2Int pos, Size size, Color color) {
        this.pos = pos;
        this.size = size;
        this.color = color;
    }
}