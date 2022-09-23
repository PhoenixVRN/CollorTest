using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pictures : MonoBehaviour
{
    public Color color;
    public int brushSize;
    private Texture2D t2D;
    private RectTransform rect;
    private Vector2 mousePos;
    private int width = 0;
    private int height = 0;
    
   
    void Start()
    {
        mousePos = new Vector2();
        var rawImage = GetComponent<RawImage>();
        rect = rawImage.GetComponent<RectTransform>();
        t2D = GetComponent<RawImage>().texture as Texture2D;
        width = (int) rect.rect.width;
        height = (int) rect.rect.height;
        var pixelData = t2D.GetPixels();
        Debug.Log("всего пикселей "+pixelData.Length);
    }
    
    void Update()
    {
//        brushSize += (int)Input.mouseScrollDelta.y;
        
        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main,
                out mousePos);
//            mousePos.x = width - (width / 2 - mousePos.x);
            mousePos.x = Mathf.Abs((width / 2 - mousePos.x) - width);
            mousePos.y = Mathf.Abs((height / 2 - mousePos.y) - height);
            Debug.Log((int)mousePos.x +" "+ (int)mousePos.y);
//            t2D.SetPixel((int)mousePos.x, (int) mousePos.y, color);
            DrawCircle((int)mousePos.x, (int)mousePos.y);
            t2D.Apply();
        }
    }
    
    void DrawCircle(int rayX, int rayY) 
    {
        for (int y = 0; y < brushSize; y++) {
            for (int x = 0; x < brushSize; x++) {

                float x2 = Mathf.Pow(x - brushSize / 2, 2);
                float y2 = Mathf.Pow(y - brushSize / 2, 2);
                float r2 = Mathf.Pow(brushSize / 2 , 2);

                if (x2 + y2 < r2) {
//                    int pixelX = rayX + x - width / 2;
//                    int pixelY = rayY + y - height / 2;

                    int pixelX = rayX + x;
                    int pixelY = rayY + y;

                    if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height) {
                        Color oldColor = t2D.GetPixel(pixelX, pixelY);
                        // if (eraser && ColorValuesEqual(oldColor, Color.black))
                        // {
                        //     break;
                        // }
                        // else
                        // {
                            Color resultColor = Color.Lerp(oldColor, color, color.a);
                            t2D.SetPixel(pixelX, pixelY, resultColor); 
//                        }
                    }
                }
            }
        }
    }
}
