using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollision : MonoBehaviour
{
    [SerializeField] private car_parts cp;
    [SerializeField] private part_config partConfig;
    private int playerID;
    // Start is called before the first frame update
    void Start()
    {
        playerID = GetComponentInParent<playerID>().p;
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        return;
        if (cp.lostPartDelay > 0)
        {
            return;
        }

        if (collision.gameObject.GetComponent<floating_part>())
        {
            return;
        }

        playerID pID = collision.gameObject.GetComponentInParent<playerID>();
        if (!pID)
        {
            if (collision.gameObject.tag != "ground" && (collision.impulse.magnitude > partConfig.impulseToLosePartNonPlayer))
            {
                cp.lose_random_part(collision.impulse);
            }
            return;
        }

        if (pID.p == playerID)
        {
            return;
        }

        //Debug.Log("car_parts collision impulse: " + collision.impulse.magnitude);
        if (collision.impulse.magnitude > partConfig.impulseToLosePart)
        {
            Vector3 selfVelocity = this.GetComponent<Rigidbody>().velocity;
            Vector3 otherVelcity = collision.gameObject.GetComponent<Rigidbody>().velocity;
            Vector3 impulse = collision.impulse;
            float selfDot = Vector3.Dot(selfVelocity.normalized, impulse);
            float otherDot = Vector3.Dot(otherVelcity.normalized, impulse);

            if (selfDot < otherDot) //other car suffers
            {
                //float upper_bound = Mathf.Pow(0.75f,cp.partCount(playerID, part.bumper));
                //float random_roll = Random.Range(0, 1);

                collision.gameObject.GetComponentInParent<car_parts>().lose_random_part(collision.impulse);
                
            }
            else //you suffer
            {
                //cp.lose_random_part(collision.impulse);
            }
        }
    }
}
