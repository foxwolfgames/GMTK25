using Chronomance.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "Rockfall", menuName = "Scriptable Objects/Spells/Rockfall")]
public class Rockfall : ScriptableObject
{
    [SerializeField] private SoundClipData soundClip;

}
