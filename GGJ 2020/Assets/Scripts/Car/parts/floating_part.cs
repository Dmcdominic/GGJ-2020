using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class floating_part : MonoBehaviour {

    // Serialized fields
    [SerializeField] private part_config partConfig;
    [SerializeField] private part part_type;

    // Private vars
    private float pickup_delay;


    // Start is called before the first frame update
    void Start() {
        pickup_delay = partConfig.pickupDelay;
    }

    // Update is called once per frame
    void Update() {
        if (pickup_delay > 0) {
            pickup_delay -= Time.deltaTime;
        }
    }


    // While something is touching you, try to collide with it
    private void OnTriggerEnter(Collider collider) {
        tryPickupFromCollider(collider);
    }
    private void OnTriggerStay(Collider collider) {
        tryPickupFromCollider(collider);
    }

    // If the collision is with a car that can pick this up, get picked up
    private void tryPickupFromCollider(Collider collider) {
        if (pickup_delay > 0) {
            return;
        }

        GameObject obj = collider.gameObject;
        car_parts carParts = obj.GetComponent<car_parts>();
        if (carParts) {
            carParts.pickup_part(this.part_type);
            Destroy(this);
            return;
        }
    }
}
