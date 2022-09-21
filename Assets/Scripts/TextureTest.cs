using UnityEngine;

public class TextureTest : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;
    [Range(2, 512)]
    [SerializeField] private int _resolution = 128;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private float _radiusOut = 0.5f;
    [SerializeField] private float _radiusIn = 0.3f;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Gradient _gradient;

    private void OnValidate()
    {
        if (_texture == null)
        {
            _texture = new Texture2D(_resolution, _resolution);
            GetComponent<Renderer>().material.mainTexture = _texture;
        }

        if (_texture.width != _resolution)
            _texture.Resize(_resolution, _resolution);

        _texture.filterMode = _filterMode;
        _texture.wrapMode = _textureWrapMode;

        float step = 1f / _resolution;
        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                float x2 = Mathf.Pow((x+0.5f) * step - _offset.x, 2);
                float y2 = Mathf.Pow((y+0.5f) * step - _offset.y, 2);
                float r2Out = Mathf.Pow(_radiusOut, 2);
                float r2In = Mathf.Pow(_radiusIn, 2);
                float result = x2 + y2;
                float interpolant = Mathf.InverseLerp(r2In, r2Out, result);
//                _texture.SetPixel(x, y, Color.yellow);
 //               Color color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), interpolant);
                Color color = _gradient.Evaluate(interpolant);
                if (result < r2Out && result > r2In)
                {
                    _texture.SetPixel(x, y, color);
                }else
                    _texture.SetPixel(x, y, Color.white);
            }
        }
        _texture.Apply();
    }
}
