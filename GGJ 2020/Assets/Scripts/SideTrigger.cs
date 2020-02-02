using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    public float slowmoDuration = 1.0f;
    public float slowmoscale = 0.125f;
    public float slowmoCooldown = 5f;
    private float cooldownElapsed = 0f;
    private float scaleInverse = 2;
    private bool inSlowmo = false;
    private bool inCooldown = false;
    private float elapsed = 0.0f;


    private void StealShit()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SideTrigger")
        {
            SideTrigger other_side = other.transform.GetComponent<SideTrigger>();
            slowmoDuration = Mathf.Max(slowmoDuration, other_side.slowmoDuration);
            slowmoscale = Mathf.Min(slowmoscale, other_side.slowmoscale);
            Time.timeScale = slowmoscale;
            inSlowmo = true;
            scaleInverse = 1 / slowmoscale;
        }
    }


    void Update()
    {
        if (inSlowmo && !inCooldown)
        {
            elapsed += Time.deltaTime * scaleInverse;
            if(elapsed > slowmoDuration)
            {
                elapsed = 0;
                Time.timeScale = 1;
                inSlowmo = false;
                inCooldown = true;
            }
        }

        if (inCooldown)
        {
            cooldownElapsed += Time.deltaTime;
            if(cooldownElapsed > slowmoCooldown)
            {
                cooldownElapsed = 0;
                inCooldown = false;
            }
        }
    }
}
