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

    public AudioClip movesound;

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
        string r = "Bulldoze" + Random.value.ToString();
        SoundManager.instance.StartLoop(movesound, r,0.3f);
        while (transform.position.x != targetX)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetX, moveSpeed), transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        yield return StartCoroutine(Fall());
        transform.position = orig;
        routine = null;
        SoundManager.instance.StopLoop(movesound, r);
    }

    private IEnumerator Fall()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y - riseHeight, transform.position.z);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, riseSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}
