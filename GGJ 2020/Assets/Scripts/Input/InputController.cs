using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif


public class InputController : MonoBehaviour
{
    [SerializeField]
    PlayerControlState state;

    [SerializeField]

    bool usingKeyboard;

    // Horn sound
    public AudioConfig audioConfig;
    // Whether this horn is supposed to loop or not
    public bool isHornLooping;

    private int p;

#if UNITY_STANDALONE_WIN
    XInputDotNetPure.PlayerIndex player;
#endif


    private bool leftshoulderpressed;
    private bool rightshoulderpressed;
    private bool startPressed;
    private float vibration = 0;
    private float vibration_standby = 0.000f;
    private float vibration_move = 0.01f;
    private float vibration_boost = 0.03f;
    private float vibration_break = 0.02f;

    private void Awake()
    {
        p = GetComponentInParent<playerID>().p;
#if UNITY_STANDALONE_WIN
        player = (PlayerIndex)p;

        var playerControlInfo = state[(int)player];
        state[(int)player] = playerControlInfo;
#endif
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
        state[(int)player] = playerControlInfo;

        
        
        if (!startPressed && GamePad.GetState(player).Buttons.Start == ButtonState.Pressed)
        {
            playerControlInfo.inGame = !playerControlInfo.inGame;
            //spawner.changeState((int)player);
            startPressed = true;
        }

        if (startPressed && GamePad.GetState(player).Buttons.Start == ButtonState.Released)
        {
            startPressed = false;
        }

        if (!leftshoulderpressed && GamePad.GetState(player).Triggers.Left > 0)
            if (playerControlInfo.horn)
            {
                if (isHornLooping)
                    SoundManager.instance.StartLoop(getHorn(), p.ToString());
                else
                    SoundManager.instance.PlayOnce(getHorn());
            }
            else if (!playerControlInfo.horn)
            {
                if (isHornLooping)
                    SoundManager.instance.StopLoop(getHorn(), p.ToString());
            }

        if (playerControlInfo.horn)
        {
            if (isHornLooping)
                SoundManager.instance.StartLoop(getHorn(), p.ToString());
            else
                SoundManager.instance.PlayOnce(getHorn());
        }
        else if (!playerControlInfo.horn)
        {
            if (isHornLooping)
                SoundManager.instance.StopLoop(getHorn(), p.ToString());
        }

        if (!leftshoulderpressed && GamePad.GetState(player).Triggers.Left > 0)
        {
            leftshoulderpressed = true;
        }
        if (leftshoulderpressed && GamePad.GetState(player).Triggers.Left == 0)
        {
            leftshoulderpressed = false;
        }
        if (!rightshoulderpressed && GamePad.GetState(player).Triggers.Right > 0)
        {
            SoundManager.instance.PlayOnce(getRev());
            rightshoulderpressed = true;

        }
        if (rightshoulderpressed && GamePad.GetState(player).Triggers.Right == 0)
        {
            rightshoulderpressed = false;

        }

        if (leftshoulderpressed)
        {
            XInputDotNetPure.GamePad.SetVibration(player, Mathf.Sqrt(vibration_break * Random.value), 0);
        }
        else if (rightshoulderpressed)
        {
            XInputDotNetPure.GamePad.SetVibration(player, 0, Mathf.Sqrt(vibration_boost * Random.value));
        }
        else if (playerControlInfo.direction != Vector3.zero)
        {
            XInputDotNetPure.GamePad.SetVibration(player, vibration_move * Random.value, vibration_move * Random.value);
        }
        else
        {
            XInputDotNetPure.GamePad.SetVibration(player, vibration_standby * Random.value, vibration_standby * Random.value);
        }
#endif
    }
    private AudioClip getHorn()
    {
        return audioConfig.horns[p % audioConfig.horns.Count];
    }
    private AudioClip getRev()
    {
        return audioConfig.revs[p % audioConfig.revs.Count];
    }
#if UNITY_STANDALONE_WIN
    private void OnDestroy()
    {
        XInputDotNetPure.GamePad.SetVibration(player, 0, 0);
    }
#endif


    }