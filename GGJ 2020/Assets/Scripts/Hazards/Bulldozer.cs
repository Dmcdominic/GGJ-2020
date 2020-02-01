using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulldozer : Hazard
{
#pragma warning disable 0649
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetX;
    [SerializeField] private float riseSpeed;
    [SerializeField] private float riseHeight;

#pragma warning restore 0649

    private Vector3 orig;
    private Coroutine routine;

    private void Awake()
    {
        orig = transform.position;
    }

    public override void Activate()
    {
        if(routine == null)
            routine = StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        while (transform.position.x != targetX)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetX, moveSpeed), transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        yield return StartCoroutine(Fall());
        transform.position = orig;
        routine = null;
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
