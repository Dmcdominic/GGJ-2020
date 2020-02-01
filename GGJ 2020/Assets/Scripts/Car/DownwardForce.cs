using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class DownwardForce : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private CarConfig carConfig;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForceAtPosition(Vector3.down * carConfig.downForce,transform.position + transform.forward,ForceMode.Force);
        rb.AddForceAtPosition(Vector3.down * carConfig.downForce,transform.position - transform.forward,ForceMode.Force);
    }
}
