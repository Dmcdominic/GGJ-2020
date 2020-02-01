using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject door1;
    [SerializeField] private Collider door1Collider;
    [SerializeField] private GameObject door2;
    [SerializeField] private Collider door2Collider;
    [SerializeField] private GameObject warningIndicator;
    [SerializeField] private float openTime;
    [SerializeField] private float duration;
    [SerializeField] private float warningDuration;
    [SerializeField] private AnimationCurve warningInterval;
    [SerializeField] private bool startOnEnable;
#pragma warning restore 0649

    private Quaternion closed = Quaternion.Euler(0, 0, 0);
    private Quaternion opened1 = Quaternion.Euler(0, 0, 90);
    private Quaternion opened2 = Quaternion.Euler(0, 0, -90);

    private Coroutine routine;

    private void OnEnable()
    {
        if (startOnEnable) Activate();
    }

    public void Activate()
    {
        if(routine == null)
            routine = StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return StartCoroutine(Warning());
        StartCoroutine(Open());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(Close());
        routine = null;
    }

    private IEnumerator Open()
    {
        float timer = 0;
        door1Collider.enabled = false;
        door2Collider.enabled = false;
        while(timer < openTime)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            door1.transform.rotation = Quaternion.Lerp(closed, opened1, timer / openTime);
            door2.transform.rotation = Quaternion.Lerp(closed, opened2, timer / openTime);
        }
    }

    private IEnumerator Close()
    {
        float timer = 0;
        door1Collider.enabled = true;
        door2Collider.enabled = true;
        while (timer < openTime)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            door1.transform.rotation = Quaternion.Lerp(opened1, closed, timer / openTime);
            door2.transform.rotation = Quaternion.Lerp(opened2, closed, timer / openTime);
        }
    }

    private IEnumerator Warning()
    {
        float timer = 0;
        float intervalTimer = 0;

        while(timer < warningDuration)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            intervalTimer += Time.fixedDeltaTime;
            if (intervalTimer >= warningInterval.Evaluate(timer / warningDuration))
            {
                warningIndicator.SetActive(!warningIndicator.activeSelf);
                intervalTimer = 0;
            }
        }
        warningIndicator.SetActive(false);
    }
}
