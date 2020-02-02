using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DiesFromKillzone : MonoBehaviour
{

    [SerializeField] private GameObject carModel;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip dieClip;
    private car_parts parts;
    private int playerid;
    public IntEvent playerDied;

    private bool diedThisFrame = false;

    private float creationTime;

    private void Start()
    {
        parts = GetComponentInParent<car_parts>();
        playerid = GetComponentInParent<playerID>().p;
        creationTime = Time.time;
    }

    private void Update()
    {
        if (parts.partCount(playerid, part.engine) < 1)
        {
            GameObject expl = GameObject.Instantiate(explosionEffect);
            expl.transform.position = gameObject.transform.position;
            SoundManager.instance.PlayOnce(dieClip, 1);
            playerDied.Invoke(GetComponentInParent<playerID>().p);
            diedThisFrame = true;
            Destroy(gameObject);
            carModel.transform.parent = null;
            foreach (MeshRenderer meshRenderer in carModel.GetComponentsInChildren<MeshRenderer>())
            {
                var rb = meshRenderer.gameObject.AddComponent<Rigidbody>();
                rb.AddExplosionForce(1500, transform.position, 20);
            }

            foreach (TrailRenderer trail in carModel.GetComponentsInChildren<TrailRenderer>())
            {
                StartCoroutine(fadeTrail(trail));
            }
        }
    }


    private void OnTriggerEnter(Collider collider) {
        if (diedThisFrame) {
            return;
        }
        if (collider.gameObject.tag == "killzone") {
            SoundManager.instance.PlayOnce(dieClip, 1);
            GameObject expl = GameObject.Instantiate(explosionEffect);
            expl.transform.position = gameObject.transform.position;
            playerDied.Invoke(GetComponentInParent<playerID>().p);
            diedThisFrame = true;
            Destroy(gameObject);
            // TODO - EXPLODE
            carModel.transform.parent = null;
            foreach (MeshRenderer meshRenderer in carModel.GetComponentsInChildren<MeshRenderer>())
            {
                //meshRenderer.gameObject.AddComponent<Rigidbody>();
                var rb = meshRenderer.gameObject.AddComponent<Rigidbody>();
                rb.AddExplosionForce(1500,transform.position,20);
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
