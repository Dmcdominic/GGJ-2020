using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : Hazard
{
#pragma warning disable 0649
    [SerializeField] private float riseSpeed;
    [SerializeField] private float riseHeight;
    [SerializeField] private float duration;
    [SerializeField] private bool startOnEnable;
#pragma warning restore 0649

    private void OnEnable()
    {
        if (startOnEnable)
            Activate();
    }

    override public void Activate()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return StartCoroutine(Rise());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(Fall());

        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator Rise()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y + riseHeight, transform.position.z);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, riseSpeed);
            yield return new WaitForFixedUpdate();
        }

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
