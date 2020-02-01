using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float moveSpeed;
#pragma warning restore 0649

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if is car
        //Knock off part if so
    }
}
