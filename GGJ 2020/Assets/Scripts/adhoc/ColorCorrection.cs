using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCorrection : MonoBehaviour
{
    [SerializeField]
    Material[] playerColor;

    [SerializeField] private int player;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            var mats = t.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].Equals(playerColor[player]))
                {
                    mats[i] = playerColor[player];
                }
            }
        }
    }

    
}
