using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    [SerializeField]
    private float slowmoDuration = 0.25f;
    [SerializeField]
    private float slowmoscale = 0.5f;
    private float scaleInverse = 2;
    private bool inSlowmo = false;
    private float elapsed = 0.0f;


    private void StealShit()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SideTrigger")
        {
            Time.timeScale = slowmoscale;
            inSlowmo = true;
            scaleInverse = 1 / slowmoscale;
        }
    }


    void Update()
    {
        if (inSlowmo)
        {
            elapsed += Time.deltaTime * scaleInverse;
            if(elapsed > slowmoDuration)
            {
                elapsed = 0;
                Time.timeScale = 1;
                inSlowmo = false;
            }
        }
    }
}
