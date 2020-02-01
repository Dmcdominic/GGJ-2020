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
    [SerializeField] private Transform frontLWheel;
    [SerializeField] private Transform frontRWheel;
    [SerializeField] private PartList myParts;
    
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
                if ((Vector3.Dot(carRB.velocity,transform.forward) < 0 || carRB.velocity.magnitude < 1) && pci.footBrake > .8f)
                {
                    wheel.motorTorque = -carConfig.reverseSpeed;
                }
                else
                {
                    wheel.brakeTorque = pci.footBrake * carConfig.maxBrake * Mathf.Sqrt(carRB.velocity.magnitude);
                    wheel.motorTorque = pci.throttle * carConfig.gearThrottles[curGear];
                }
            });
    }

    void SteerNormal()
    {
        frontWheels.Map
        (
            wheel =>
            {
                if (inputDir.magnitude < .1f)
                {
                    return wheel.steerAngle = 0;
                }
                var thetaCar = Mathf.Atan2(groundDir.z, groundDir.x) * Mathf.Rad2Deg;
                var thetaInput = Mathf.Atan2(inputDir.z, inputDir.x) * Mathf.Rad2Deg;
                var thetaDelta = Mathf.DeltaAngle(thetaCar, thetaInput);

                var maxSteer = carConfig.minSteer;
                for (int i = 0; i < myParts[player].val[(int) part.steering_wheel]; i++)
                    maxSteer += (carConfig.maxSteer - carConfig.minSteer) / Mathf.Pow(3,i);

                thetaDelta = 
                    minABS(Mathf.Clamp(thetaDelta, -maxSteer, maxSteer),
                           Mathf.Clamp(thetaDelta, -maxSteer + 180, maxSteer - 180)
                    );
                float visualWheelDir = Mathf.Clamp(-Mathf.DeltaAngle(thetaCar, thetaInput) - 90.0f, -135.0f, -45.0f);
                frontLWheel.localEulerAngles = new Vector3(0, visualWheelDir, 0);
                frontRWheel.localEulerAngles = new Vector3(0, visualWheelDir, 0);
                return wheel.steerAngle = -thetaDelta;
            });
    }


    private float minABS(float a, float b)
    {
        if (Mathf.Abs(a) < Mathf.Abs(b))
            return a;
        return b;
    }
}
