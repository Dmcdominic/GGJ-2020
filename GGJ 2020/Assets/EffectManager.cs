using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private PlayerControlState input;
    private int player;
    private PlayerControlInfo pci => input[player];
    [SerializeField] private ParticleSystem leftBurst;
    [SerializeField] private ParticleSystem rightBurst;
    bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<playerID>().p;
    }

    // Update is called once per frame
    void Update()
    {
        if(pci.throttle > 0.1f && !playing)
        {
            leftBurst.Play();
            rightBurst.Play();
            playing = true;
        }
        else if(pci.throttle <= 0.1f  && playing)
        {
            leftBurst.Stop();
            rightBurst.Stop();
            playing = false;
        }
    }
}
