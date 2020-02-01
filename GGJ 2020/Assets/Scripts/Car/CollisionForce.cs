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
            float selfDot = Mathf.Abs(Vector3.Dot(selfVelocity, impulse));
            float otherDot = Mathf.Abs(Vector3.Dot(otherVelcity, impulse));

            if (selfDot > otherDot)
            {
                float boostForce = Mathf.Sqrt(selfVelocity.sqrMagnitude * this.GetComponent<Rigidbody>().mass * 0.5f);
                this.GetComponent<Rigidbody>().AddForce(boostForce * selfVelocity.normalized, ForceMode.Impulse);
            }
            else
            {
                float forceMag = this.GetComponent<Rigidbody>().mass * forceMultiplier;
                Vector3 forceVec = forceMag * (selfVelocity + otherVelcity).normalized;
                Debug.Log(this.transform.name);
                Debug.Log(forceVec);
                this.GetComponent<Rigidbody>().AddForceAtPosition(forceVec, collision.collider.ClosestPointOnBounds(this.transform.position), ForceMode.Impulse);
            }
        }

    }


    private void OnCollisionExit(Collision collision)
    {
        
    }

}
