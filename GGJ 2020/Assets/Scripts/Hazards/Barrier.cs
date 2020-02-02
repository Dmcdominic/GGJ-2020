using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Hazard
{
#pragma warning disable 0649
    [SerializeField] private float riseSpeed;
    [SerializeField] private float riseHeight;
    [SerializeField] private float duration;
    [SerializeField] private AudioClip whirrSFX;
    [SerializeField] private bool startOnEnable;
#pragma warning restore 0649

    private Coroutine routine;

    private void OnEnable()
    {
        if (startOnEnable)
            Activate();
    }

    override public void Activate()
    {
        if(routine == null)
            routine = StartCoroutine(Routine());
        else
        {
            StopCoroutine(routine);
            routine = StartCoroutine(Extend());
        }
    }

    private IEnumerator Routine()
    {
        yield return StartCoroutine(Fall());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(Rise());
        routine = null;
    }

    private IEnumerator Extend()
    {
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(Rise());
        routine = null;
    }

    private IEnumerator Rise()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y + riseHeight, transform.position.z);

        string id = "Barrier" + Random.value.ToString();
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

        string id = "Barrier" + Random.value.ToString();
        SoundManager.instance.StartLoop(whirrSFX, id, 0.3f);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, riseSpeed);
            yield return new WaitForFixedUpdate();
        }
        SoundManager.instance.StopLoop(whirrSFX, id);

    }
}
