using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCorrection : MonoBehaviour
{
    [SerializeField]
    private ColorConfig colorConfig;

    private int player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<playerID>().p;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            // TODO - check if mr.gameObject itself already has a playerID, to override player
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
