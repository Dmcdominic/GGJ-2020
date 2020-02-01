using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct WheelAxis
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public Tuple<T,T> Map<T>(Func<WheelCollider,T> f)
    {
        return Tuple.Create(f(leftWheel),f(rightWheel));
    }

    public void Map(Action<WheelCollider> f)
    {
        f(leftWheel);
        f(rightWheel);
    }
}

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelAxis frontWheels;
    [SerializeField] private WheelAxis rearWheels;
    [SerializeField] private PlayerControlState input;
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private CarConfig carConfig;
    
    //                 Car State                   //
    private int curGear = 0;

    private Vector3 groundVelocity => carRB.velocity - carRB.velocity.y * Vector3.up;
    private Vector3 inputDir => input.direction;
    private Vector3 groundDir => transform.forward - transform.forward.y * Vector3.up;
    

    private void Start()
    {
        StartCoroutine(DriveNormal());
    }

    IEnumerator DriveNormal()
    {
        while (true)
        {
            ThrottleNormal();
            SteerNormal();
            yield return null;
        }
    }

    void ThrottleNormal()
    {
        rearWheels.Map
        (
            wheel => wheel.motorTorque = Vector3.Dot(transform.forward,inputDir) * carConfig.gearThrottles[curGear]
        );
    }

    void SteerNormal()
    {
        frontWheels.Map
        (
            wheel =>
            {
                var relativeDir = inputDir - groundDir;
                var theta = Mathf.Atan2(relativeDir.y,relativeDir.x) * Mathf.Rad2Deg;
                theta = Mathf.Clamp(theta, carConfig.maxSteer, carConfig.maxSteer);
                
                return wheel.steerAngle = Mathf.LerpAngle(wheel.steerAngle,theta * Time.deltaTime,Time.deltaTime);
                
            });
    }
}
