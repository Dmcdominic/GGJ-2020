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
    private void OnCollisionEnter(Collision collision) {
        tryPickupFromCollision(collision);
    }
    private void OnCollisionStay(Collision collision) {
        tryPickupFromCollision(collision);
    }

    // If the collision is with a car that can pick this up, get picked up
    private void tryPickupFromCollision(Collision collision) {
        if (pickup_delay > 0) {
            return;
        }

        GameObject obj = collision.gameObject;
        car_parts carParts = obj.GetComponentInParent<car_parts>();
        if (carParts) {
            carParts.pickup_part(this.part_type);
            Instantiate(partConfig.dust_cloud).transform.position = transform.position;
            Destroy(gameObject);
            return;
        }
    }
}
