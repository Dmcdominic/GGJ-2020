using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Derby/Part Config")]
public class part_config : ScriptableObject {
    public float impulseToLosePart;
    public float delayBetweenLosingParts;

    public float pickupDelay;
    public float flyingPartYOffset;
    public float flyingPartImpulseUpBoost;
    public float flyingPartImpulseMult;
    
    
    // Floating part prefabs
    public List<part_prefab> partPrefabs;
}


[System.Serializable]
public class part_prefab {
    public part Part;
    public GameObject Prefab;
}