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
    

    // Update is called once per frame
    void Update()
    {
        var stick = GamePad.GetState(player).ThumbSticks.Left;
        state.direction = new Vector3(stick.X,0,stick.Y);
        
    }
}
