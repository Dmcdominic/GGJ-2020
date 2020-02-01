using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Derby/Car Config")]
public class CarConfig : ScriptableObject
{
    public float maxSteer;
    public List<float> gearThrottles;
    public float maxBrake;
    public float downForce = 10;
    public float unflipSpeed = 10;

}
