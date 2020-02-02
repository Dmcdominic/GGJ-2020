using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Derby/Audio Config")]
public class AudioConfig : ScriptableObject {
    public List<AudioClip> revs;
    public List<AudioClip> horns;
}
