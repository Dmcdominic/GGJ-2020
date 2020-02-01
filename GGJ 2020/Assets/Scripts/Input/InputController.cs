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

#if UNITY_STANDALONE_WIN
    [SerializeField]
    XInputDotNetPure.PlayerIndex player;
#endif


    private bool leftshoulderpressed;
    private bool rightshoulderpressed;

    private void Start()
    {
#if UNITY_STANDALONE_WIN
        var playerControlInfo = state[(int)player];
        state[(int) player] = playerControlInfo;
#endif

    }

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

        if (!leftshoulderpressed && GamePad.GetState(player).Buttons.LeftShoulder == ButtonState.Pressed)
        {
            leftshoulderpressed = true;
            state[(int) player].shiftDown.Invoke();
        }
        if(leftshoulderpressed && GamePad.GetState(player).Buttons.LeftShoulder == ButtonState.Released)
        {
            leftshoulderpressed = false;
        }
        if (!rightshoulderpressed && GamePad.GetState(player).Buttons.RightShoulder == ButtonState.Pressed)
        {
            rightshoulderpressed = true;
            state[(int) player].shiftDown.Invoke();
        }
        if(rightshoulderpressed && GamePad.GetState(player).Buttons.RightShoulder == ButtonState.Released)
        {
            rightshoulderpressed = false;
        }
#endif
    }
}
