using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    [SerializeField]
    private float force = 200.0f;
    private Rigidbody rigid;

    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.AddForce(new Vector3(-force, 0, 0), ForceMode.Force);

    }
}
