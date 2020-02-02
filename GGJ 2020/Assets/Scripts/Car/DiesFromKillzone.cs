using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesFromKillzone : MonoBehaviour {

    public IntEvent playerDied;


    private void OnTriggerEnter(Collider collider) {
        Debug.Log(collider);
        if (collider.gameObject.tag == "killzone") {
            playerDied.Invoke(GetComponentInParent<playerID>().p);
            Destroy(gameObject);
            // TODO - EXPLODE
        }
    }
}
