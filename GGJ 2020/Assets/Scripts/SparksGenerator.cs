using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparksGenerator : MonoBehaviour
{

    public ParticleSystem sparksPrefab;
    public float sparksDelay = 0.2f;
    private ParticleSystem sparksInfo;
    private GameObject instantiatedSparks;
    private GameObject collidedWith;
    private float lifetime = 0.25f;
    private bool colliding = false;
    private Vector3 closestPoint;

    private void Start()
    {
        sparksInfo = sparksPrefab.GetComponent<ParticleSystem>();
        lifetime = sparksInfo.main.duration + sparksInfo.startLifetime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "car")
        {
            collidedWith = collision.gameObject;
            closestPoint = collision.collider.ClosestPointOnBounds(this.transform.position);
            instantiatedSparks = Instantiate(sparksPrefab.gameObject, closestPoint, Quaternion.identity) as GameObject;
            StartCoroutine("FireSparks");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "car")
        {
            StopCoroutine("FireSparks");
            Destroy(instantiatedSparks);
        }
    }

    IEnumerator FireSparks()
    {
        while (true)
        {
            Vector3 colliderPos = collidedWith.transform.position;
            Vector3 average = (this.transform.position + colliderPos) / 2;
            instantiatedSparks.transform.position = average;
            instantiatedSparks.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(sparksDelay);
        }
    }

    void OnCollisionStay()
    {
        GameObject sparks = null;
        if (colliding)
        {
            sparks = Instantiate(sparksPrefab.gameObject, closestPoint, Quaternion.identity) as GameObject;
            sparks.GetComponent<ParticleSystem>().Play();
            Destroy(sparks, lifetime);
        }
    }
}
