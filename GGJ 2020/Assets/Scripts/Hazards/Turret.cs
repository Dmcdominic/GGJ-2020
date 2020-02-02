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
    [SerializeField] private AudioClip whirrSFX;
    [SerializeField] private AudioClip shootSFX;
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

    public void HardModeActivate()
    {
        if (Random.value <= 0.5f) doubleFire = true;
        Activate();
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
        Vector3 target = new Vector3(transform.position.x, transform.position.y + riseHeight, transform.position.z);

        string id = "Turret" + Random.value.ToString();
        SoundManager.instance.StartLoop(whirrSFX, id, 0.3f);
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, riseSpeed);
            yield return new WaitForFixedUpdate();
        }
        SoundManager.instance.StopLoop(whirrSFX, id);

    }

    private IEnumerator Fall()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y - riseHeight, transform.position.z);

        string id = "Turret" + Random.value.ToString();
        SoundManager.instance.StartLoop(whirrSFX, id, 0.3f);
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, riseSpeed);
            yield return new WaitForFixedUpdate();
        }
        SoundManager.instance.StopLoop(whirrSFX, id);

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
            SoundManager.instance.PlayOnce(shootSFX);
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
