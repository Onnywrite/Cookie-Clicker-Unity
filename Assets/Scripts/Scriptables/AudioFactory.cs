using UnityEngine;

[CreateAssetMenu(fileName = "Factory", menuName = "Scriptable Objects/Audio Factory")]
public class AudioFactory : ScriptableObject
{
    public AudioClip PlusScore;
    public AudioClip MinusScore;
    public AudioClip StateChanged;
    public AudioClip DataTransfering;
    public AudioClip[] Clicks;
}
