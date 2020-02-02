using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesFromKillzone : MonoBehaviour {

    public IntEvent playerDied;

    public GameObject explosionEffect;

    private bool diedThisFrame = false;
    private int playerId;

    private car_parts parts;

    private void Start()
    {
        parts = GetComponentInParent<car_parts>();
        playerId = GetComponentInParent<playerID>().p;
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
        }
    }

    private void Update()
    {
        if(parts.partCount(playerId, part.engine) < 1)
        {
            GameObject expl = GameObject.Instantiate(explosionEffect);
            expl.transform.position = gameObject.transform.position;
            
            playerDied.Invoke(GetComponentInParent<playerID>().p);
            diedThisFrame = true;
            Destroy(gameObject);
        }
    }
}
