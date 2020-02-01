using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePit : Hazard
{
    [SerializeField] private Trapdoor trapdoor;

    override public void Activate()
    {
        trapdoor.Activate();
    }
}
