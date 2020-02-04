using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeUtil;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif


public class InputController : MonoBehaviour
{
    [SerializeField]
    PlayerControlState state;

    [SerializeField]


    // Horn sound
    public AudioConfig audioConfig;
    [SerializeField] private AudioClip EngineRun;
    [SerializeField] private AudioClip Fire;
    // Whether this horn is supposed to loop or not

    private int p;

    [SerializeField] private PartList myParts;
    
#if UNITY_STANDALONE_WIN
    XInputDotNetPure.PlayerIndex player;
#endif


    private float vibration_move = 0.01f;
    private float vibration_boost = 0.03f;
    private float vibration_break = 0.02f;
    private List<AudioClip> horns;
    private Dictionary<int,float> leftVibrations;
    private Dictionary<int, float> rightVibrations;
    
    
    
    private void Awake()
    {
        leftVibrations = new Dictionary<int, float>();
        rightVibrations = new Dictionary<int, float>();
        p = GetComponentInParent<playerID>().p;
#if UNITY_STANDALONE_WIN
        player = (PlayerIndex)p;

        var playerControlInfo = state[(int)player];
        state[(int)player] = playerControlInfo;
#endif
    }

    private void Start()
    {
        StartCoroutine(EngineMonitor(state[(int)player]));
        for (int i = 1; i < audioConfig.horns.Count; i++)
        {
            SoundManager.instance.PlayWhile(() =>
            {
                return state.val[p].horn && myParts[p].horns.Contains(i);
            },audioConfig.horns[i],p.ToString());
        }
        SoundManager.instance.PlayWhen(() =>
        {
            return state.val[p].horn && myParts[p].horns.Contains(0);
        },audioConfig.horns[0]);
        
        SoundManager.instance.PlayWhile(() => state[p].throttle > 0,Fire,p.ToString());
        SoundManager.instance.PlayWhen(() => state[p].throttle > 0,getRev());

        StartCoroutine(RumbleWhile(() => state[p].footBrake > 0,() => vibration_break,Sum<Unit, Unit>.Inl(new Unit())));
        StartCoroutine(RumbleWhile(() => state[p].throttle > 0, () => vibration_boost,
            Sum<Unit, Unit>.Inr(new Unit())));
        StartCoroutine(RumbleWhile(() => state[p].throttle > .5f,() => vibration_boost * 2,Sum<Unit, Unit>.Inr(new Unit())));
        StartCoroutine(RumbleWhile(() => state[p].direction.magnitude > .1f, () => vibration_move,
            Sum<Unit, Unit>.Inl(new Unit())));
        StartCoroutine(RumbleWhile(() => state[p].direction.magnitude > .1f, () => vibration_move,
            Sum<Unit, Unit>.Inr(new Unit())));
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        var stick = GamePad.GetState(player).ThumbSticks.Left;
        var playerControlInfo = state[(int)player];
        playerControlInfo.direction = new Vector3(stick.X, 0, stick.Y);
        playerControlInfo.footBrake = GamePad.GetState(player).Triggers.Left;
        playerControlInfo.handBrakePulled = (int)GamePad.GetState(player).Buttons.X;
        playerControlInfo.throttle = GamePad.GetState(player).Triggers.Right;
        playerControlInfo.horn = GamePad.GetState(player).Buttons.A == ButtonState.Pressed;
        playerControlInfo.hornNo = GamePad.GetState(player).Buttons.A == ButtonState.Released;
        state[(int)player] = playerControlInfo;
#else

#endif
    }


    IEnumerator RumbleWhile(Func<bool> P,Func<float> amount,Sum<Unit,Unit> side)
    {
        int id;
        var vib = (side.Match(l => leftVibrations,r => rightVibrations));
        while (true)
        {
            yield return new WaitUntil(P);
            id = (int)(Random.value * 100000);
            vib.Add(id, amount());
            float max = vib.Max((pair => pair.Value)) * (myParts[p].val[(int) part.muffler] == 0 ? 3 : 1);
            side.Match(u => { 
                    XInputDotNetPure.GamePad.SetVibration(player,max,0);
                    return 0;
                },
                u => {
                    XInputDotNetPure.GamePad.SetVibration(player,0,max);
                    return 0;
                });
            yield return new WaitWhile(P);
            vib.Remove(id);
            if (vib.Count > 0)
                max =  vib.Max((pair => pair.Value)) * (myParts[p].val[(int) part.muffler] == 0 ? 3 : 1);
            else
                max = 0;
            side.Match(u => { 
                    XInputDotNetPure.GamePad.SetVibration(player,max,0);
                    return 0;
                },
                u => {
                    XInputDotNetPure.GamePad.SetVibration(player,0,max);
                    return 0;
                });
        }
    }
    private List<AudioClip> getHorns()
    {
        var result = new List<AudioClip>();
        for (int i = p; i < myParts[p].val[(int) part.horn] + p; i++)
        {
            result.Add(audioConfig.horns[i % audioConfig.horns.Count]);
        }
        return result;
    }
    private AudioClip getRev()
    {
        return audioConfig.revs[p % audioConfig.revs.Count];
    }

    private IEnumerator EngineMonitor(PlayerControlInfo playerControlInfo)
    {
        while(true)
        {
            GameObject MySound;
            yield return new WaitUntil(() => playerControlInfo.direction.magnitude > .2f);
            MySound = SoundManager.instance.StartLoop(EngineRun, p.ToString(), 0.3f * (myParts[p].val[(int)part.muffler] == 0 ? 2 : 1));
            MySound.GetComponent<AudioSource>().pitch = 1f + playerControlInfo.direction.magnitude;
            yield return new WaitUntil(() => playerControlInfo.direction == Vector3.zero);
            SoundManager.instance.StopLoop(EngineRun, p.ToString());
        }
    }

#if UNITY_STANDALONE_WIN
    private void OnDestroy()
    {
        XInputDotNetPure.GamePad.SetVibration(player, 0, 0);
    }
#endif


    }