using Onnywrite.Common;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
[SelectionBase]
public class Cookie : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sprite;

    private float _minScale = 0.1f;
    private float _maxScale = 2f;
    private Transform _colliderTransform;
    private float _scale;
    private Vector2 _pos;
    private float _trans;

    protected virtual void Awake()
    {
        MathGeek.MinMax(ref _minScale, ref _maxScale);
        _colliderTransform = GetComponent<Collider2D>().transform;
        Clicked = new();
    }

    public virtual float Scale
    {
        get => _scale;
        private set
        {
            _scale = value;
            Vector3 scaleVec = new(_scale, _scale, _scale);
            transform.localScale = scaleVec;
        }
    }
    
    public virtual Vector2 Position
    {
        get => _pos;
        set
        {
            _pos = value;
            Vector3 posVec = new(_pos.x, _pos.y, 0f);
            transform.localPosition = posVec;
            _colliderTransform.localPosition = posVec;
        }
    }

    public virtual Color SpriteColor
    {
        get => _sprite.color;
        set => _sprite.color = value;
    }

    public UnityEvent<Cookie> Clicked { get; private set; }

    public virtual float Transparency 
    {
        get => _trans;
        set
        {
            _trans = Mathf.Clamp(value, 0f, 1f);
            var old = SpriteColor;
            SpriteColor = new(old.r, old.g, old.b, _trans);
        }
    }


    public float MinScale
    {
        get => _minScale;
        set
        {
            _minScale = value;
            MathGeek.MinMax(ref _minScale, ref _maxScale);
        }
    }

    public float MaxScale
    {
        get => _maxScale;
        set
        {
            _maxScale = value;
            MathGeek.MinMax(ref _minScale, ref _maxScale);
        }
    }

    private void OnMouseDown()
    {
        Clicked.Invoke(this);
    }

    /*
    ??? void Setup(CookieFactory setup)
    {
        MaxScale = setup.MaxScale;
        MinScale = setup.MinScale;
        Transparency = setup.Transparency;
    }
    */

    public virtual void SetRandomScale()
    {
        Scale = Random.Range(MinScale, MaxScale);
    }

    public virtual void SetRandomPosition(float minX, float maxX, float minY, float maxY)
    {
        Position = MathGeek.RandomVec2(minX, maxX, minY, maxY);
    }

    public virtual void SetRandomPosition()
    {
        var screenBounds = Game.GetScreenBounds();
        SetRandomPosition(-screenBounds.x, screenBounds.x, -screenBounds.y, screenBounds.y);
    }

    public virtual void SetRandomTransparency(float min, float max)
    {
        Transparency = Random.Range(min, max);
    }

    public virtual void SetRandomTransparency() => SetRandomTransparency(0f, 1f);
}