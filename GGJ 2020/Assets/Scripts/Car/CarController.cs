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
    [SerializeField] private int player;
    
    //                 Car State                   //
    private int curGear = 0;

    private Vector3 groundVelocity => carRB.velocity - carRB.velocity.y * Vector3.up;
    
    private PlayerControlInfo pci => input[player];
    private Vector3 inputDir => pci.direction;
    private Vector3 groundDir => transform.forward - transform.forward.y * Vector3.up;
    

    private void Start()
    {
        carRB.centerOfMass += Vector3.down;
        StartCoroutine(DriveNormal());
        StartCoroutine(UnFlip());
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

    IEnumerator UnFlip()
    {
        while(true)
        {
            while (carRB.transform.up.y < Mathf.Sqrt(2))
            {
                carRB.inertiaTensorRotation = Quaternion.Euler(0, 0, carConfig.unflipSpeed);
                yield return null;
            }

            yield return null;
        }
    }

    void ThrottleNormal()
    {
        rearWheels.Map
        (
            wheel =>
            {
                wheel.brakeTorque = pci.handBrakePulled * carConfig.maxBrake;
                wheel.motorTorque =
                    pci.throttle * carConfig.gearThrottles[curGear] * 
                    Mathf.Sign(Vector3.Dot(transform.forward, inputDir) * carConfig.gearThrottles[curGear]);
            });
    }

    void SteerNormal()
    {
        frontWheels.Map
        (
            wheel =>
            {
                var thetaCar = Mathf.Atan2(groundDir.z, groundDir.x) * Mathf.Rad2Deg;
                var thetaInput = Mathf.Atan2(inputDir.z, inputDir.x) * Mathf.Rad2Deg;
                var thetaDelta = Mathf.DeltaAngle(thetaCar, thetaInput);
                print($"0Car: {thetaCar}, 0I: {thetaInput}, 0D: {thetaDelta}");
                thetaDelta = Mathf.Clamp(thetaDelta, -carConfig.maxSteer, carConfig.maxSteer);
                return wheel.steerAngle = -thetaDelta;

            });
    }
}
