using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionForce : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier = 50;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Car")
        {
            Vector3 selfVelocity = this.GetComponent<Rigidbody>().velocity;
            Vector3 otherVelcity = collision.gameObject.GetComponent<Rigidbody>().velocity;
            Vector3 impulse = collision.impulse;
            float selfDot = Vector3.Dot(selfVelocity, impulse);
            float otherDot = Vector3.Dot(otherVelcity, impulse);

            if (selfDot < otherDot)
            {
                Vector3 oppositeForce = -selfVelocity  * selfVelocity.sqrMagnitude * this.GetComponent<Rigidbody>().mass * forceMultiplier;
                oppositeForce.y = 0;
                selfVelocity.y = 0;
                this.GetComponent<Rigidbody>().velocity = selfVelocity;
                this.GetComponent<Rigidbody>().AddForce(oppositeForce, ForceMode.Impulse);
            }
            else
            {
                Vector3 appliedForce = otherVelcity * otherVelcity.sqrMagnitude * this.GetComponent<Rigidbody>().mass * forceMultiplier;
                Debug.Log("Why am I suffering");
                Debug.Log(appliedForce.y);
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, appliedForce.y, 0), ForceMode.Impulse);
            }
        }

    }


    private void OnCollisionExit(Collision collision)
    {
        
    }

}
