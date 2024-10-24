using Onnywrite.Common;
using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[SelectionBase]
public class Cookie : MonoBehaviour
{
    [SerializeField]
    private CookieParams _params;
    [SerializeField]
    private CookieVisualBase _visual;
    
    private float _trans;
    private float _scale;
    private Vector2 _pos;
    private Transform _colliderTransform;
    private Collider2D _collider;
    private Rigidbody2D _rb;

    //private const float HEIGHT = 360f;
    //private const float WIDTH = 360f;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _colliderTransform = _collider.transform;
        _rb = GetComponent<Rigidbody2D>();
        ApplyGravity(false);
        Clicked = new();
    }

    public virtual float Transparency
    {
        get => _trans;
        set
        {
            _trans = Mathf.Clamp(value, 0f, 1f);
            var old = _visual.Color;
            _visual.Color = new(old.r, old.g, old.b, _trans);
        }
    }

    public virtual float Scale
    {
        get => _scale;
        set
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

    public UnityEvent<Cookie> Clicked { get; private set; }

    public virtual bool TryClick(Vector2 point, Action<Cookie> clickedCallback)
    {
        if (_collider.OverlapPoint(point) && !_visual.IsBeingClicked)
        {
            _visual.Click(clickedCallback, this);
            Clicked.Invoke(this);
            return true;
        }
        return false;
    }

    public void ApplyGravity(bool apply = true)
    {
        _rb.simulated = apply;
    }

    public virtual void SetRandomScale(float min, float max)
    {
        MathGeek.MinMax(ref min, ref max);
        Scale = Random.Range(min, max);
    }

    public virtual void SetRandomScale()
        => SetRandomScale(_params.MinScale, _params.MaxScale);

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

    public virtual void SetRandomTransparency()
        => SetRandomTransparency(_params.MinTransparency, _params.MaxTransparency);

    public virtual void Randomize(RandomizeBitmap bitmap = RandomizeBitmap.All)
    {
        if ((bitmap & RandomizeBitmap.Position) == RandomizeBitmap.Position)
            SetRandomPosition();
        if ((bitmap & RandomizeBitmap.Scale) == RandomizeBitmap.Scale)
            SetRandomScale();
        if ((bitmap & RandomizeBitmap.Transparency) == RandomizeBitmap.Transparency)
            SetRandomTransparency();
    }
}

public enum RandomizeBitmap
{
    Position = 1,
    Scale = 2,
    Transparency = 4,
    All = 7
}