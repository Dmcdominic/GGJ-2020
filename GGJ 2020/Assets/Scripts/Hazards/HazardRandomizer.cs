using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardRandomizer : Hazard
{
    private Hazard[] hazards;

    private void Start()
    {
        hazards = GetComponentsInChildren<Hazard>();
    }

    override public void Activate()
    {
        int rng = Mathf.FloorToInt(Random.Range(0, hazards.Length));
        hazards[rng].Activate();
    }
}
