using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public ColorConfig config;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Light>().color = config.playerColor[GetComponentInParent<playerID>().p].color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
