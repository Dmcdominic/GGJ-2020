using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Hazard
{
#pragma warning disable 0649
    [SerializeField] private float riseSpeed;
    [SerializeField] private float riseHeight;
    [SerializeField] private float duration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;
    [SerializeField] private bool doubleFire;
    [SerializeField] private bool startOnEnable;
#pragma warning restore 0649

    private Coroutine rot;

    private Coroutine routine;

    private void OnEnable()
    {
        if(startOnEnable)
            Activate();
    }

    override public void Activate()
    {
        if(routine == null)
            routine = StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return StartCoroutine(Rise());
        rot = StartCoroutine(Rotate());
        yield return StartCoroutine(Fire());
        StopCoroutine(Rotate());
        yield return StartCoroutine(Fall());

        routine = null;
        Destroy(gameObject);

        //gameObject.SetActive(false);
    }

    private IEnumerator Rise()
    {
        float targetY = transform.position.y + riseHeight;

        while(transform.position.y < targetY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + riseSpeed, transform.position.z);
            yield return new WaitForFixedUpdate();
        }

    }

    private IEnumerator Fall()
    {
        float targetY = transform.position.y - riseHeight;

        while(transform.position.y > targetY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - riseSpeed, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + turnSpeed, transform.rotation.eulerAngles.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Fire()
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            GameObject o = Instantiate(bullet);
            o.transform.position = transform.position;
            o.transform.forward = transform.forward;
            if (doubleFire)
            {
                o = Instantiate(bullet);
                o.transform.position = transform.position;
                o.transform.forward = transform.forward;
            }
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

}
