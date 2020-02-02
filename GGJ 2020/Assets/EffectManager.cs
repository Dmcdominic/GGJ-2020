using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TireTreads
{
    public WheelCollider wheel;
    public Transform wheelModel;
}

public class EffectManager : MonoBehaviour
{
    [SerializeField] private PlayerControlState input;
    private int player;
    private PlayerControlInfo pci => input[player];
    [SerializeField] private ParticleSystem leftBurst;
    [SerializeField] private ParticleSystem rightBurst;
    [SerializeField] private PartList carParts;
    [SerializeField] private TrailRenderer trail_prefab;
    [SerializeField] private TireTreads FrontLTire;
    [SerializeField] private TireTreads FrontRTire;
    [SerializeField] private TireTreads RearLTire;
    [SerializeField] private TireTreads RearRTire;

    
    bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<playerID>().p;
        StartCoroutine(DisparentTread(FrontLTire));
        StartCoroutine(DisparentTread(FrontRTire));
        StartCoroutine(DisparentTread(RearLTire));
        StartCoroutine(DisparentTread(RearRTire));
        StartCoroutine(control());

    }

    IEnumerator DisparentTread(TireTreads tt)
    {
        TrailRenderer tr;
        while (true)
        {
            yield return new WaitUntil(() => tt.wheel.isGrounded);
            tr = Instantiate(trail_prefab,tt.wheelModel.position + Vector3.down * .2f,Quaternion.identity, tt.wheelModel);
            yield return new WaitUntil(() => !tt.wheel.isGrounded);
            tr.transform.parent = null;
        }
        
    }


    IEnumerator control()
    {
        while (true)
        {
            while (carParts[player].val[(int) part.engine] > 0)
            {
                yield return new WaitUntil(() => pci.throttle > .1f);

                leftBurst.Play();
                rightBurst.Play();
                yield return new WaitWhile(() => pci.throttle > .1f);
            
                leftBurst.Stop();
                rightBurst.Stop();
            }

            yield return null;
        }
    }
}
