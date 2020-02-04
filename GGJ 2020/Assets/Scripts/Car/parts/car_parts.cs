using System.Collections;
using System.Collections.Generic;
using TypeUtil;
using UnityEngine;

public enum part { tire, steering_wheel, bumper, horn, muffler, hood, door, engine, brake } // WHEN YOU CHANGE THIS, update parts_init

[System.Serializable]
public class SerializedParts
{
    public int[] val;
    public List<int> horns;
} 


public class car_parts : MonoBehaviour {

    // Readonly settings
    public static readonly int[] parts_init = { 4, 1, 2, 1, 1, 2, 2, 1, 1}; // Number of parts to start with
    public static readonly int num_diff_parts = System.Enum.GetValues(typeof(part)).Length; //icky
    [SerializeField] private AudioClip pick_up_sound;
    [SerializeField] private PlayerBoolRef alive;


    [SerializeField] private PartList my_parts;
    [SerializeField] private PartList pickup_player_ids;

    public int partCount(int player, part p) => my_parts[player].val[(int)p];
    

    // Serialized fields
    [SerializeField]
    private part_config partConfig;

    // Private vars
    private int playerID;
    public float lostPartDelay = 0;

    // Init
    private void Awake() {
        playerID = GetComponentInParent<playerID>().p;
        
        // Keep track of parts
        my_parts[playerID].val = new int[num_diff_parts];
        my_parts[playerID].horns = new List<int>();
        my_parts[playerID].horns.Add(playerID);
        pickup_player_ids[playerID].val = new int[num_diff_parts];

        if (parts_init.Length != num_diff_parts) {
            Debug.LogError("HEY! parts_init in car_parts.cs doesn't have all the part init numbers");
        }
        
        for (int p = 0; p < num_diff_parts; p++) {
            my_parts[playerID].val[p] = parts_init[p];
            pickup_player_ids[playerID].val[p] = p;
        }
    }

    private void Start()
    {
        
        alive[playerID] = true;
    }

    private void OnDestroy()
    {
        alive[playerID] = false;
    }

    // Called every frame
    private void Update() {
        lostPartDelay -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (lostPartDelay > 0) {
            return;
        }

        if (collision.gameObject.GetComponent<floating_part>()) {
            return;
        }

        playerID pID = collision.gameObject.GetComponentInParent<playerID>();
        if (!pID) {
            if (collision.gameObject.tag != "ground" && (collision.impulse.magnitude > partConfig.impulseToLosePartNonPlayer)) {
                lose_random_part(collision.impulse);
            }
            return;
        }

        if (pID.p == playerID) {
            return;
        }

        //Debug.Log("car_parts collision impulse: " + collision.impulse.magnitude);
        if (collision.impulse.magnitude > partConfig.impulseToLosePart) {
            Vector3 selfVelocity = this.GetComponent<Rigidbody>().velocity;
            Vector3 impulse = collision.impulse;

            float selfDot = Mathf.Abs(Vector3.Dot(transform.forward, impulse.normalized));

            const float dotThreshold = 0.5f;
            const float veloThreshold = 9f;
            if (selfDot < dotThreshold || selfVelocity.magnitude < veloThreshold) { // always lose a part if your dot or velo is under a threshold
                const int maxPartsLost = 3;
                int partsLost = Random.Range(1, maxPartsLost + 1);
                for (int i=0; i < partsLost; i++) {
                    lose_random_part(collision.impulse);
                }
            } else {
                int protects = my_parts[playerID].val[(int)part.bumper] + 1;
                for (int i=0; i < protects; i++) {
                    if(Random.Range(0f, 1f) > 0.5f) {
                        return;
                    }
                }

                if (my_parts[playerID].val[(int)part.bumper] > 0) {
                    lose_part(part.bumper, collision.impulse);
                } else {
                    lose_random_part(collision.impulse);
                }
            }
        }
    }

    // Send a random part flying off
    public void lose_random_part(Vector3 collisionImpulse) {
        List<part> loseableParts = new List<part>();
        for (int p = 0; p < my_parts[playerID].val.Length; p++) {
            for (int i = 0; i < my_parts[playerID].val[p]; i++) {
                if ((part)p != part.engine) {
                    loseableParts.Add((part)p);
                }
                // TODO - determine this instead based on how strong the impulse is?
                // e.g. hard hit = lose more important piece
            }
        }
        if (loseableParts.Count > 2) {
            lose_part(loseableParts[Random.Range(0, loseableParts.Count)], collisionImpulse);
        } else {
            lose_part(part.engine, collisionImpulse);
        }
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


        Sum<int, Unit> hornID = Sum<int, Unit>.Inr(new Unit());
        if (partType == part.horn && my_parts[playerID].horns.Count > 0)
        {
            hornID = Sum<int, Unit>.Inl(my_parts[playerID].horns[0]);
            my_parts[playerID].horns.RemoveAt(0);
        }
        my_parts[playerID].val[(int)partType]--;
        foreach (part_prefab pp in partConfig.partPrefabs) {
            if (pp.Part == partType) {
                if (!pp.Prefab) {
                    Debug.LogError("No floating part prefab available for part: " + pp.Part + ". Please make one and add it to partConfig");
                    return;
                }
                GameObject floatingPart = Instantiate(pp.Prefab);
                floatingPart.transform.position = transform.position + Vector3.up * partConfig.flyingPartYOffset * Random.Range(1f, 3f);
                floatingPart.GetComponent<Rigidbody>().AddForce(partImpulse, ForceMode.Impulse);
                floatingPart.GetComponent<playerID>().p = hornID.Match(i => i,u => playerID);
                return;
            }
        }
        Debug.LogError("No part prefab found for part: " + partType);
    }

    // Called by a floating part when you pick it up
    public void pickup_part(part partType, int p) {
        my_parts[playerID].val[(int)partType]++;

        if(partType == part.horn) my_parts[playerID].horns.Add(p);
        
        SoundManager.instance.PlayOnce(pick_up_sound);
       
        pickup_player_ids[playerID].val[(int)partType] = p;
        //Debug.Log("Car now has " + (my_parts[playerID].val[(int)partType]) + " " + partType + "(s)");

    }
}
