using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerashake : MonoBehaviour
{
    
    [SerializeField] private AnimationCurve intesityCurve;
    [SerializeField] private float duration = .5f;
    [SerializeField] private float travelSpeed = 10;
    [SerializeField] private IntEvent activate;

    void Start()
    {
        activate.AddListener(i =>
        {
            Activate(i);
        });
    }


    public void Activate(float magnitude = 1)
    {
        StartCoroutine(shake(magnitude));
    }

    IEnumerator shake(float norm_magnitude = 1)
    {
        float timePassed = 0;
        while (timePassed < 1)
        {
            float beginTime = Time.time;
            yield return travelTo(randomVector2() * intesityCurve.Evaluate(timePassed) * norm_magnitude,short_out: duration);
            timePassed += (Time.time - beginTime) * (1f / duration);
        }

        yield return travelTo(Vector3.zero);
    }

    Vector2 randomVector2()
    {
        return new Vector2(UnityEngine.Random.value - .5f,UnityEngine.Random.value - .5f).normalized;
    }

    IEnumerator travelTo(Vector3 localDestination,float short_out = Mathf.Infinity)
    {
        localDestination -= Vector3.forward * 10;
        float time_spent = 0;
        var dir = (localDestination - transform.localPosition).normalized ;
        while (Vector2.Distance(transform.localPosition, localDestination) > .1f)
        {
            transform.localPosition += dir
                                       * travelSpeed 
                                     //  * Mathf.Sqrt(Vector2.Distance(transform.localPosition, localDestination))
                                       * Time.deltaTime;
            time_spent += Time.deltaTime;
            if(time_spent > short_out) yield break;
            yield return null;
        }

        transform.localPosition = localDestination;
    }

}
