using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Derby/Car Config")]
public class CarConfig : ScriptableObject
{
    public float maxSteer;
    public List<float> gearThrottles;
}
