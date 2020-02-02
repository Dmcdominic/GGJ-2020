using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private AnimationCurve trapFreq;
    [SerializeField] private GameObject hazardParent;
    [SerializeField] private Hazard turret;
    [SerializeField] private Hazard trap;
    [SerializeField] private Hazard bar;
    [SerializeField] private Hazard ramp;
    [SerializeField] private Hazard bulldozer;

    [SerializeField] private Hazard barrier;
    [SerializeField] private AnimationCurve barrierFreq;
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
            yield return new WaitForSeconds(1);
            timer += 1;
            if (Random.value <= trapFreq.Evaluate(Mathf.Clamp01(timer / maxTime)))
            {
                int rng = Mathf.FloorToInt(Random.Range(0, 4));
                GameObject o;
                switch (rng)
                {
                    case 0:
                        //turret.Activate();
                        o = Instantiate(turret.gameObject, hazardParent.transform);
                        o.transform.position = new Vector3(Random.Range(-25, 25), o.transform.position.y, Random.Range(-15, 15));
                        if (timer / maxTime > 0.6f) o.GetComponent<Turret>().HardModeActivate();
                        else o.GetComponent<Hazard>().Activate();
                        break;
                    case 1:
                        trap.Activate();
                        break;
                    case 2:
                        if (Random.value < 0.5f) o = Instantiate(ramp.gameObject, hazardParent.transform);
                        else o = Instantiate(bar.gameObject, hazardParent.transform);
                        o.transform.position = new Vector3(Random.Range(-25, 25), o.transform.position.y, Random.Range(-15, 15));
                        o.transform.rotation = Quaternion.Euler(o.transform.rotation.eulerAngles.x, Random.Range(0, 360), o.transform.rotation.eulerAngles.z);
                        o.GetComponent<Hazard>().Activate();
                        break;
                    case 3:
                        bulldozer.Activate();
                        break;
                    default:
                        break;
                }
            }

            if(Random.value <= barrierFreq.Evaluate(Mathf.Clamp01(timer / maxTime)))
            {
                barrier.Activate();
            }
        }
    }
}
