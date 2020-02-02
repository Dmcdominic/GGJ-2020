using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif


public class InputController : MonoBehaviour
{
    // Used to uniquely identify this car for audio purposes.
    public string carId;

    [SerializeField]
    PlayerControlState state;

    [SerializeField]

    bool usingKeyboard;

    // Throttle sound
    public AudioClip revSound;

    // Horn sound
    public AudioClip honk;
    // Whether this horn is supposed to loop or not
    public bool isHornLooping;


#if UNITY_STANDALONE_WIN
    XInputDotNetPure.PlayerIndex player;
#endif


    private bool leftshoulderpressed;
    private bool rightshoulderpressed;

    private float vibration = 0;
    private float vibration_standby = 0.05f;
    private float vibration_move = 0.1f;
    private float vibration_boost = 0.5f;
    private float vibration_break = 0.3f;

    private void Awake()
    {
#if UNITY_STANDALONE_WIN
    private void Awake()
    {
        player = (PlayerIndex)GetComponentInParent<playerID>().p;

        var playerControlInfo = state[(int)player];
        state[(int) player] = playerControlInfo;
#endif

    }
#endif

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        var stick = GamePad.GetState(player).ThumbSticks.Left;
        var playerControlInfo = state[(int)player];
        playerControlInfo.direction = new Vector3(stick.X,0,stick.Y);
        playerControlInfo.footBrake = GamePad.GetState(player).Triggers.Left;
        playerControlInfo.handBrakePulled = (int)GamePad.GetState(player).Buttons.X;
        playerControlInfo.throttle = GamePad.GetState(player).Triggers.Right;
        playerControlInfo.horn = GamePad.GetState(player).Buttons.A == ButtonState.Pressed;
        state[(int) player] = playerControlInfo;



        if (!leftshoulderpressed && GamePad.GetState(player).Triggers.Left > 0)
        if (playerControlInfo.horn)
        {
            if (isHornLooping)
                SoundManager.instance.StartLoop(honk, carId);
            else
                SoundManager.instance.PlayOnce(honk);
        }
        else if (!playerControlInfo.horn)
        {
            if (isHornLooping)
                SoundManager.instance.StopLoop(honk, carId);
        }

        if (playerControlInfo.throttle > 0) 
        {
            SoundManager.instance.PlayOnce(revSound);
        }

        if (playerControlInfo.horn )
        {
            if (isHornLooping)
                SoundManager.instance.StartLoop(honk, carId);
            else
                SoundManager.instance.PlayOnce(honk);
        }
        else if (!playerControlInfo.horn )
        {
            if (isHornLooping)
                SoundManager.instance.StopLoop(honk, carId);
        }

        if (!leftshoulderpressed && GamePad.GetState(player).Buttons.LeftShoulder == ButtonState.Pressed)
        {
            leftshoulderpressed = true;
            state[(int) player].shiftDown.Invoke();
        }
        if(leftshoulderpressed && GamePad.GetState(player).Triggers.Left == 0)
        {
            leftshoulderpressed = false;
        }
        if (!rightshoulderpressed && GamePad.GetState(player).Triggers.Right > 0)
        {
            rightshoulderpressed = true;
            state[(int) player].shiftDown.Invoke();

        }
        if(rightshoulderpressed && GamePad.GetState(player).Triggers.Right == 0)
        {
            rightshoulderpressed = false;

        }

        if (leftshoulderpressed)
        {
            XInputDotNetPure.GamePad.SetVibration(player, vibration_break, 0);
        } else if (rightshoulderpressed)
        {
            XInputDotNetPure.GamePad.SetVibration(player, 0, vibration_boost);
        } else if(playerControlInfo.direction != new Vector3())
        {
            XInputDotNetPure.GamePad.SetVibration(player, vibration_move, vibration_standby);
        }
        else
        {
            XInputDotNetPure.GamePad.SetVibration(player, vibration_standby, vibration_standby);
        }
#endif
    }
}
