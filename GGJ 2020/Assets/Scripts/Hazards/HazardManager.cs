using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private AnimationCurve trapFreq;
    [SerializeField] [Range(0, 1)] private float trapFreqRange;
    [SerializeField] private GameObject[] hazards;
    [SerializeField] private float maxTime;
#pragma warning restore 0649

    private Coroutine routine;

    public void Start()
    {
        routine = StartCoroutine(Routine());
    }

    public void Stop()
    {
        StopCoroutine(routine);
    }

    private IEnumerator Routine()
    {
        float timer = 0;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }
    }
}
