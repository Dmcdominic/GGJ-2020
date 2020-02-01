using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_model_manager : MonoBehaviour {

    // Serialized fields
    [SerializeField] private PartList my_parts;

    // The parts in the model
    [SerializeField] private List<part_ref_list> partObjs;

    // Private vars
    private int playerID;


    // Start is called before the first frame update
    void Awake() {
        playerID = GetComponentInParent<playerID>().p;
    }

    // Update is called once per frame
    void Update() {
        foreach (part_ref_list pRL in partObjs) {
            if (car_parts.parts_init[(int)pRL.Part] == 0) {
                continue;
            }
            for (int i = 0; i < pRL.Refs.Count; i++) {
                pRL.Refs[i].SetActive(my_parts[playerID].val[(int)pRL.Part] > i);
            }
        }
    }
}


[System.Serializable]
public class part_ref_list {
    public part Part;
    public List<GameObject> Refs;
}