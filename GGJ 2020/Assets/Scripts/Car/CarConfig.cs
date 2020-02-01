using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Derby/Car Config")]
public class CarConfig : ScriptableObject
{
    public float minSteer = 8.5f;
    public float maxSteer = 45;
    public List<float> gearThrottles;
    public float maxBrake;
    public float downForce = 10;
    public float unflipSpeed = 10;
    public float reverseSpeed = 1000;
    public AnimationCurve tireForwardStiffness;
    public AnimationCurve tireSideStiffness;
}
