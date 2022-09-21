using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [Range(2, 512)]
    [SerializeField] private int _textureSize = 128;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Material _material;

    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 8;
    private int _oldRayX, _oldRayY;

    void OnValidate()
    {
        // if (_texture == null) {
        //     _texture = new Texture2D(_textureSize, _textureSize);
        // }
        // if (_texture.width != _textureSize) {
        //     _texture.Resize(_textureSize, _textureSize);
        // }
        _textureSize = _texture.width;
        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _material.mainTexture = _texture;
        _texture.Apply();
    }

    private void Update() 
    {

        _brushSize += (int)Input.mouseScrollDelta.y;

        if (Input.GetMouseButton(0)) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 100f)) {

                int rayX = (int)(hit.textureCoord.x * _textureSize);
                int rayY = (int)(hit.textureCoord.y * _textureSize);
                
                if (_oldRayX != rayX || _oldRayY != rayY) {
                    //DrawQuad(rayX, rayY);
                 //                    DrawCircle(rayX, rayY);
                 //                    Shading(rayX,rayY);
//                    Seed(rayX, rayY, Color.black, _color, _texture);
                    _oldRayX = rayX;
                    _oldRayY = rayY;
                }
                _texture.Apply();
            }
        }
    }



   


       void DrawQuad(int rayX, int rayY)
           {
               for (int y = 0; y < _brushSize; y++) {
                   for (int x = 0; x < _brushSize; x++) {
                       _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
                   }
               }
           }

           void DrawCircle(int rayX, int rayY) 
           {
               for (int y = 0; y < _brushSize; y++) {
                   for (int x = 0; x < _brushSize; x++) {

                       float x2 = Mathf.Pow(x - _brushSize / 2, 2);
                       float y2 = Mathf.Pow(y - _brushSize / 2, 2);
                       float r2 = Mathf.Pow(_brushSize / 2 - 0.5f, 2);

                       if (x2 + y2 < r2) {
                           int pixelX = rayX + x - _brushSize / 2;
                           int pixelY = rayY + y - _brushSize / 2;

                           if (pixelX >= 0 && pixelX < _textureSize && pixelY >= 0 && pixelY < _textureSize) {
                               Color oldColor = _texture.GetPixel(pixelX, pixelY);
                               //                        Float d = Mathf.InverseLerp(oldColor, _color, _color.a);
                               Color resultColor = Color.Lerp(oldColor, _color, _color.a);
                               //                        Color resultColor = new Color(0,0,0,0.5f);
                               if (oldColor.r >0.5f&& oldColor.g > 0.5f&& oldColor.b >0.5f)
                                   //Debug.Log(oldColor);
                                   _texture.SetPixel(pixelX, pixelY, resultColor);
                           }

                       }
                       
                   }
               }
           }
}

