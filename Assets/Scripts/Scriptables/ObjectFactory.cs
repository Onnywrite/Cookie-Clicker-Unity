using UnityEngine;

[CreateAssetMenu(fileName = "Factory", menuName = "Scriptable Objects/Cookie Factory")]
public class ObjectFactory : ScriptableObject
{
    public Cookie StandartPrefab;
    public Cookie CrazyPrefab;
}
