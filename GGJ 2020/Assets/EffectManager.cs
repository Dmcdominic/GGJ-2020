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

    // Update is called once per frame
    void Update()
    {
        if(pci.throttle > 0.1f && !playing && carParts[player].val[(int)part.engine] > 0)
        {
            leftBurst.Play();
            rightBurst.Play();
            playing = true;
        }
        else if(pci.throttle <= 0.1f  && playing)
        {
            leftBurst.Stop();
            rightBurst.Stop();
            playing = false;
        }
    }
}
