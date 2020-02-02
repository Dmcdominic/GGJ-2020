using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct PlayerControlInfo
{
    public Vector3 direction;
    public float footBrake;
    public int handBrakePulled;
    public float throttle;
    public bool horn;
    public UnityEvent shiftUp;
    public UnityEvent shiftDown;
    public UnityEvent playHorn;
}

public class PCSEvent : UnityEvent<PlayerControlInfo[]> {}

[CreateAssetMenu(menuName = "Derby/Player Control State")]
public class PlayerControlState : PlayerRef<PlayerControlInfo,PCSEvent>{}
