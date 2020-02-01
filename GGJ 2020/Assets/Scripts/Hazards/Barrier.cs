using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Hazard
{
#pragma warning disable 0649
    [SerializeField] private float riseSpeed;
    [SerializeField] private float riseHeight;
    [SerializeField] private float duration;
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
    }

    private IEnumerator Routine()
    {
        yield return StartCoroutine(Fall());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(Rise());
        routine = null;
    }

    private IEnumerator Rise()
    {
        float targetY = transform.position.y + riseHeight;

        while (transform.position.y < targetY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + riseSpeed, transform.position.z);
            yield return new WaitForFixedUpdate();
        }

    }

    private IEnumerator Fall()
    {
        float targetY = transform.position.y - riseHeight;

        while (transform.position.y > targetY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - riseSpeed, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
    }
}
