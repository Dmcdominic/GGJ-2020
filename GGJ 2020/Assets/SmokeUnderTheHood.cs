using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeUnderTheHood : MonoBehaviour
{
    [SerializeField] private ParticleSystem smoke;
    private car_parts parts;
    private int playerID;

    bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        playerID = GetComponentInParent<playerID>().p;
        parts = GetComponent<car_parts>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playing && parts.partCount(playerID, part.hood) < 1)
        {
            smoke.Play();
            playing = true;
        }
        else if(playing && parts.partCount(playerID, part.hood) > 0)
        {
            smoke.Stop();
            playing = false;
        }
    }
}
