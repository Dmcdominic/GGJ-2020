using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Derby/Player Control State")]
public class PlayerControlState : ScriptableObject
{
    public Vector3 direction;
    public bool handBrakePulled;
    public Event shiftUp;
    public Event shiftDown;
}
