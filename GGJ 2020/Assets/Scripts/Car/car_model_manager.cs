using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_model_manager : MonoBehaviour {

    // Serialized fields
    [SerializeField] private PartList my_parts;
    [SerializeField] private PartList pickup_player_ids;

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
                bool setActive = my_parts[playerID].val[(int)pRL.Part] > i;
                bool justEnabledNow = !(pRL.Refs[i].activeInHierarchy) && setActive;
                pRL.Refs[i].SetActive(setActive);
                if (justEnabledNow) {
                    ColorCorrection.correctColor(pRL.Refs[i].GetComponent<MeshRenderer>(), pickup_player_ids[playerID].val[(int)pRL.Part]);
                }
            }
        }
    }
}


[System.Serializable]
public class part_ref_list {
    public part Part;
    public List<GameObject> Refs;
}