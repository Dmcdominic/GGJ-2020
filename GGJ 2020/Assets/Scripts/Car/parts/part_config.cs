using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Derby/Part Config")]
public class part_config : ScriptableObject {
    public float pickupDelay;
    public float impulseToLosePart;
    public float flyingPartSpeedMult;

    // Floating part prefabs
    public List<part_prefab> partPrefabs;
}


[System.Serializable]
public class part_prefab {
    public part Part;
    public GameObject Prefab;
}