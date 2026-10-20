using UnityEngine;

public class _MaterialOverride : MonoBehaviour
{
    Renderer _renderer;
    MaterialPropertyBlock _MPB;
    public Texture2D Texture2D;
    public Color _color = Color.white;

    public Vector2 _tiling;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _MPB = new MaterialPropertyBlock();
    }

    void Start()
    {
        _renderer.GetPropertyBlock(_MPB);
        _MPB.SetColor("_BaseColor",_color);
        if (Texture2D != null)
        {
            _MPB.SetTexture("_BaseTexture", Texture2D);
        }
        _MPB.SetVector("_circleTiling",_tiling);
        _renderer.SetPropertyBlock(_MPB);
    }
}
