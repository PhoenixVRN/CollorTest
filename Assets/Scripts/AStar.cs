using UnityEngine;
using UnityEngine.UI;

public class AStar : MonoBehaviour
{
    public Toggle IsToggleShading;
    public bool eraser;
    
    [SerializeField] private Texture2D _texture;
    [Range(2, 512)] [SerializeField] private int _textureSize;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Material _material;
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 8;
    
    private CollorManager _collorManager;
    private int _oldRayX, _oldRayY;
    private Coordinates[] _arr;
    private bool IsShading;
    
    void Start()
    {
        CollorManager.СolorСhange += CollorChange;
        _collorManager = CollorManager.Instance;
        _textureSize = _texture.width;
        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _material.mainTexture = _texture;
        _texture.Apply();
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 100f)) {

                int rayX = (int)(hit.textureCoord.x * _textureSize);
                int rayY = (int)(hit.textureCoord.y * _textureSize);

                if (IsToggleShading.isOn && !IsShading && !eraser)
                {
                    Shading(rayX,rayY);
                    IsShading = false;
                    _texture.Apply();
                    return;
                }
               
                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    DrawCircle(rayX, rayY);

                    _oldRayX = rayX;
                    _oldRayY = rayY;
                }
                _texture.Apply();
            }
        }
    }

    public void SetEraser()
    {
        eraser = true;
        _color = Color.white;
    }
    private void CollorChange()
    {
        eraser = false;
        _color = _collorManager.color;
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
                        if (eraser && ColorValuesEqual(oldColor, Color.black))
                        {
                           break;
                        }
                        else
                        {
                            Color resultColor = Color.Lerp(oldColor, _color, _color.a);
                            _texture.SetPixel(pixelX, pixelY, resultColor); 
                        }
                    }
                }
            }
        }
    }
    
    public void Shading(int currentX, int currentY)
    {
        _arr = new Coordinates[_texture.width * _texture.height];
        if (!ColorValuesEqual(_texture.GetPixel(currentX,currentY))) return;
        IsShading = true;
       _texture.SetPixel(currentX,currentY,_color);
       _arr[0] = new Coordinates(currentX, currentY);
       int countArr = 0;
       int currentArr = 0;
       foreach (var coordin in _arr)
       {
           if(coordin==null) return;
           if ((coordin.x >= 1) && (coordin.x < _texture.width))
           {
               int x = coordin.x - 1;
               if (ColorValuesEqual(_texture.GetPixel(x, coordin.y)))
               {
                   _texture.SetPixel(x, coordin.y, _color);
                   countArr++;
                   _arr[countArr] = new Coordinates(x, coordin.y);
               }

               x = coordin.x + 1;
               if (ColorValuesEqual(_texture.GetPixel(x, coordin.y)))
               {
                   _texture.SetPixel(x, coordin.y, _color);
                   countArr++;
                   _arr[countArr] = new Coordinates(x, coordin.y);
               }
           }

           if ((coordin.y >= 1) && (coordin.y < _texture.width))
           {
               int y = coordin.y - 1;
               if (ColorValuesEqual(_texture.GetPixel(coordin.x, y)))
               {
                   _texture.SetPixel(coordin.x, y, _color);
                   countArr++;
                   _arr[countArr] = new Coordinates(coordin.x, y);
               }

               y = coordin.y + 1;
               if (ColorValuesEqual(_texture.GetPixel(coordin.x, y)))
               {
                   _texture.SetPixel(coordin.x, y, _color);
                   countArr++;
                   _arr[countArr] = new Coordinates(coordin.x, y);
               }
           }
       }
    }

    private bool ColorValuesEqual(Color clr1, Color clr2)
    {
        var result = (clr1.r - clr2.r) + (clr1.g - clr2.g) + (clr1.b - clr2.b);
        return Mathf.Abs(result) < 1;
    }
    
    private bool ColorValuesEqual(Color clr)
    {
        var result = (clr.r - Color.white.r) + (clr.g - Color.white.g) + (clr.b - Color.white.b);
        return Mathf.Abs(result) < 0.5f;
    }
}

public class Coordinates
{
    public int x;
    public int y;

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}