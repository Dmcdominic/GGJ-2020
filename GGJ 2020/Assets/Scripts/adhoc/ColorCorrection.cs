using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCorrection : MonoBehaviour
{
    [SerializeField]
    private ColorConfig colorConfig;

    // Start is called before the first frame update
    void Start()
    {
        int rootPlayer = GetComponentInParent<playerID>().p;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            int player = rootPlayer;
            playerID pID = mr.gameObject.GetComponent<playerID>();
            if (pID) {
                player = pID.p;
            }

            var mats = mr.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].color.Equals(colorConfig.playerColor[0].color))
                {
                    mats[i] = colorConfig.playerColor[player];
                }
            }
            mr.materials = mats;

        }
    }

    
}
