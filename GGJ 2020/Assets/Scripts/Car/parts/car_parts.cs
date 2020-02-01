using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum part { tire, steering_wheel, bumper, horn, muffler } // WHEN YOU CHANGE THIS, update parts_init

[RequireComponent(typeof(Collider))]
public class car_parts : MonoBehaviour {

    // Readonly settings
    private static readonly int[] parts_init = { 4, 1, 1, 1, 1 }; // Number of parts to start with
    private static readonly int num_diff_parts = System.Enum.GetValues(typeof(part)).Length;

    // Keep track of parts
    [HideInInspector]
    public int[] my_parts = new int[num_diff_parts];

    // Serialized fields
    [SerializeField]
    private part_config partConfig;


    // Init
    private void Awake() {
        if (parts_init.Length != num_diff_parts) {
            Debug.LogError("HEY! parts_init in car_parts.cs doesn't have all the part init numbers");
        }
        
        for (int p = 0; p < num_diff_parts; p++) {
            my_parts[p] = parts_init[p];
        }
    }

    private void OnCollisionEnter(Collision collision) {
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
        for (int p = 0; p < my_parts.Length; p++) {
            for (int i = 0; i < my_parts[p]; i++) {
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
        if (my_parts[(int)partType] <= 0) {
            Debug.LogError("Trying to lose part that you're already out of: " + partType);
        }
        my_parts[(int)partType]--;
        foreach (part_prefab pp in partConfig.partPrefabs) {
            if (pp.Part == partType) {
                GameObject floatingPart = Instantiate(pp.Prefab);
                floatingPart.GetComponent<Rigidbody>().AddForce(collisionImpulse * partConfig.flyingPartSpeedMult, ForceMode.Impulse);
                return;
            }
        }
        Debug.LogError("No part prefab found for part: " + partType);
    }

    // Called by a floating part when you pick it up
    public void pickup_part(part partType) {
        my_parts[(int)partType]++;
    }
}
