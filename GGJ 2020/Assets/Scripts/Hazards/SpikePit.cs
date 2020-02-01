using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePit : MonoBehaviour
{
    [SerializeField] private Trapdoor trapdoor;

    public void Activate()
    {
        trapdoor.Activate();
    }
}
