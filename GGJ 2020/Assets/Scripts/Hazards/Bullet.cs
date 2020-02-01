using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration = 30f;
#pragma warning restore 0649

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * moveSpeed;
        StartCoroutine(life());
    }

    private IEnumerator life()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if is car
        //Knock off part if so
        if(collision.collider.tag == "Car")
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
