using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum part { tire, steering_wheel, bumper, horn, muffler } // WHEN YOU CHANGE THIS, update parts_init

[System.Serializable]
public class SerializedParts
{
    public int[] val;
} 


public class car_parts : MonoBehaviour {

    // Readonly settings
    private static readonly int[] parts_init = { 4, 1, 1, 1, 1 }; // Number of parts to start with
    private static readonly int num_diff_parts = System.Enum.GetValues(typeof(part)).Length;


    [SerializeField] private PartList my_parts;
    

    // Serialized fields
    [SerializeField]
    private part_config partConfig;

    // Private vars
    private int playerID;
    private float lostPartDelay = 0;


    // Init
    private void Awake() {
        playerID = GetComponentInParent<playerID>().p;
        
        // Keep track of parts
        my_parts[playerID].val = new int[num_diff_parts];
        
        if (parts_init.Length != num_diff_parts) {
            Debug.LogError("HEY! parts_init in car_parts.cs doesn't have all the part init numbers");
        }
        
        for (int p = 0; p < num_diff_parts; p++) {
            my_parts[playerID].val[p] = parts_init[p];
        }
    }

    // Called every frame
    private void Update() {
        lostPartDelay -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (lostPartDelay > 0) {
            return;
        }

        Debug.Log("car_parts collision impulse: " + collision.impulse.magnitude);
        if (collision.impulse.magnitude > partConfig.impulseToLosePart) {
            lose_random_part(collision.impulse);
        }
    }

    // Send a random part flying off
    public void lose_random_part(Vector3 collisionImpulse) {
        // TODO - for now (TESTING) just lose tire
        lose_part(part.tire, collisionImpulse);
        return;

        List<part> loseableParts = new List<part>();
        for (int p = 0; p < my_parts[playerID].val.Length; p++) {
            for (int i = 0; i < my_parts[playerID].val[p]; i++) {
                loseableParts.Add((part)i);
                // TODO - determine this instead based on how strong the impulse is?
                // e.g. hard hit = lose more important piece
            }
        }
        if (loseableParts.Count > 0) {
            lose_part(loseableParts[Random.Range(0, loseableParts.Count)], collisionImpulse);
        } // TODO - if you're out of parts?? default part to keep losing weight? explode?
    }

    // Send a specific part flying off
    public void lose_part(part partType, Vector3 collisionImpulse) {
        lostPartDelay = partConfig.delayBetweenLosingParts;

        Vector3 partImpulse = collisionImpulse + Vector3.up * collisionImpulse.magnitude * partConfig.flyingPartImpulseUpBoost; // TODO - make this go up
        partImpulse = partImpulse.normalized * collisionImpulse.magnitude;
        partImpulse *= partConfig.flyingPartImpulseMult;
        partImpulse = new Vector3(partImpulse.x, Mathf.Abs(partImpulse.y), partImpulse.z);

        if (my_parts[playerID].val[(int)partType] <= 0) {
            Debug.LogError("Trying to lose part that you're already out of: " + partType);
        }

        my_parts[playerID].val[(int)partType]--;
        foreach (part_prefab pp in partConfig.partPrefabs) {
            if (pp.Part == partType) {
                GameObject floatingPart = Instantiate(pp.Prefab);
                floatingPart.transform.position = transform.position + Vector3.up * partConfig.flyingPartYOffset;
                floatingPart.GetComponent<Rigidbody>().AddForce(partImpulse, ForceMode.Impulse);
                return;
            }
        }
        Debug.LogError("No part prefab found for part: " + partType);
    }

    // Called by a floating part when you pick it up
    public void pickup_part(part partType) {
        my_parts[playerID].val[(int)partType]++;
        Debug.Log("Car now has " + (my_parts[playerID].val[(int)partType]) + " " + partType + "(s)");
    }
}
