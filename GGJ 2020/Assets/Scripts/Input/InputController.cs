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

    private float vibration = 0;
    private float vibration_standby = 0.00f;
    private float vibration_move = 0.001f;
    private float vibration_boost = 0.02f;
    private float vibration_break = 0.05f;

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

            if (playerControlInfo.throttle > 0)
            {
                SoundManager.instance.PlayOnce(getRev());
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

            if (!leftshoulderpressed && GamePad.GetState(player).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                leftshoulderpressed = true;
                state[(int)player].shiftDown.Invoke();
            }
            if (leftshoulderpressed && GamePad.GetState(player).Triggers.Left == 0)
            {
                leftshoulderpressed = false;
            }
            if (!rightshoulderpressed && GamePad.GetState(player).Triggers.Right > 0)
            {
                rightshoulderpressed = true;
                state[(int)player].shiftDown.Invoke();

            }
            if (rightshoulderpressed && GamePad.GetState(player).Triggers.Right == 0)
            {
                rightshoulderpressed = false;

            }

            if (leftshoulderpressed)
            {
                XInputDotNetPure.GamePad.SetVibration(player, vibration_break, 0);
            }
            else if (rightshoulderpressed)
            {
                XInputDotNetPure.GamePad.SetVibration(player, 0, vibration_boost);
            }
            else if (playerControlInfo.direction != new Vector3())
            {
                XInputDotNetPure.GamePad.SetVibration(player, vibration_move, vibration_standby);
            }
            else
            {
                XInputDotNetPure.GamePad.SetVibration(player, vibration_standby, vibration_standby);
            }

        }

    private AudioClip getHorn() {
        Debug.Log(audioConfig);
        Debug.Log(audioConfig.horns);
        return audioConfig.horns[p % audioConfig.horns.Count];
    }
    private AudioClip getRev() {
        Debug.Log(audioConfig);
        Debug.Log(audioConfig.revs);
        return audioConfig.revs[p % audioConfig.revs.Count];
    }

    private void OnDestroy()
    {
        XInputDotNetPure.GamePad.SetVibration(player, 0, 0);
    }

#endif
}