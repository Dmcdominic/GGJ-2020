using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DiesFromKillzone : MonoBehaviour
{

    [SerializeField] private GameObject carModel;
    public IntEvent playerDied;

    private bool diedThisFrame = false;

    private float creationTime;

    private void Start()
    {
        creationTime = Time.time;
    }


    private void OnTriggerEnter(Collider collider) {
        if (diedThisFrame) {
            return;
        }
        if (collider.gameObject.tag == "killzone") {
            playerDied.Invoke(GetComponentInParent<playerID>().p);
            diedThisFrame = true;
            Destroy(gameObject);
            // TODO - EXPLODE
            carModel.transform.parent = null;
            foreach (MeshRenderer meshRenderer in carModel.GetComponentsInChildren<MeshRenderer>())
            {
                var rb = meshRenderer.gameObject.AddComponent<Rigidbody>();
                rb.AddExplosionForce(10,transform.position,20);
            }

            foreach (TrailRenderer trail in carModel.GetComponentsInChildren<TrailRenderer>())
            {
                StartCoroutine(fadeTrail(trail));
            }
        }
    }


    IEnumerator fadeTrail(TrailRenderer trail)
    {
        while (true)
        {
            trail.widthMultiplier *= (1 - Time.deltaTime);
            trail.time = Time.time - creationTime + 2;
            yield return null;
        }
    }
}
