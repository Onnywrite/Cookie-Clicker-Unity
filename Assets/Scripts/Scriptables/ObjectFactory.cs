using UnityEngine;

[CreateAssetMenu(fileName = "Factory", menuName = "Scriptable Objects/Cookie Factory")]
public class ObjectFactory : ScriptableObject
{
    [SerializeField]
    private Cookie _standartPrefab;

    [SerializeField]
    private Cookie _crazyPrefab;

    [SerializeField]
    private Granny _grannyPrefab;

    [SerializeField, Range(0f, 1f)]
    private float Transparency;

    [SerializeField]
    private float MinScale;

    [SerializeField]
    private float MaxScale;

    public Cookie StandartPrefab => GetConfigured(_standartPrefab);

    public Cookie CrazyPrefab => GetConfigured(_crazyPrefab);

    public Granny GrannyPrefab => _grannyPrefab;

    private Cookie GetConfigured(Cookie @base)
    {
        @base.Transparency = Transparency;
        @base.MinScale = MinScale;
        @base.MaxScale = MaxScale;
        return @base;
    }
}
