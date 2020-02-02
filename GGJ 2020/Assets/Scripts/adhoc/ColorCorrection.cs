using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCorrection : MonoBehaviour
{
    [SerializeField]
    private ColorConfig colorConfig;

    public static ColorConfig globColorConfig;


    // Init
    private void Awake() {
        if (colorConfig) {
            globColorConfig = colorConfig;
        }    
    }

    // Start is called before the first frame update
    void Start() {
        int rootPlayer = GetComponentInParent<playerID>().p;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
            correctColor(mr, rootPlayer);
        }
    }


    public static void correctColor(MeshRenderer mr, int player) {
        if (!mr) {
            return;
        }

        var mats = mr.materials;
        for (int i = 0; i < mats.Length; i++) {
            Color currentColor = mats[i].color;
            bool playerColored = (currentColor.Equals(globColorConfig.playerColor[0].color) ||
                currentColor.Equals(globColorConfig.playerColor[1].color) ||
                currentColor.Equals(globColorConfig.playerColor[2].color) ||
                currentColor.Equals(globColorConfig.playerColor[3].color));
            if (playerColored) {
                mats[i] = globColorConfig.playerColor[player];
            }
        }
        mr.materials = mats;
    }
    
}
