using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesFromKillzone : MonoBehaviour {



    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "killzone") {
            // TODO - send playerDied event (to spawner)
            // TODO - DIE. EXPLODE
        }
    }
}
