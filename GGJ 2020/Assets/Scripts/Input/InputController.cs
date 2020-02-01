using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputController : MonoBehaviour
{

    [SerializeField]
    PlayerControlState state;

    [SerializeField]
    XInputDotNetPure.PlayerIndex player;


    private bool leftshoulderpressed;
    private bool rightshoulderpressed;

    private void Start()
    {
        
        var playerControlInfo = state[(int)player];
        state[(int) player] = playerControlInfo;
        
    }

    // Update is called once per frame
    void Update()
    {
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
       

    }
}
