using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesFromKillzone : MonoBehaviour {

    public IntEvent playerDied;

    private bool diedThisFrame = false;


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
}
