using UnityEngine;

[CreateAssetMenu(fileName = "Params", menuName = "Scriptable Objects/Cookie Params")]
public class CookieParams : ScriptableObject
{
    public float MinScale;
    public float MaxScale;
    public float MinTransparency;
    public float MaxTransparency;
}
