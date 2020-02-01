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
    private int player;
    [SerializeField] private Transform frontLWheel;
    [SerializeField] private Transform frontRWheel;
    [SerializeField] private PartList myParts;
    [SerializeField] private Transform root;
    private float steerAngle = 0;
    
    private int curGear = 0;

    private Vector3 groundVelocity => carRB.velocity - carRB.velocity.y * Vector3.up;
    
    private PlayerControlInfo pci => input[player];
    private Vector3 inputDir => pci.direction.normalized;
    private Vector3 groundDir => (transform.forward - transform.forward.y * Vector3.up).normalized;
    private int[] parts => myParts[player].val;
    

    private void Awake()
    {
        player = GetComponentInParent<playerID>().p;
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
            rearWheels.Map(wheel =>
            {
                var wheelForwardFriction = wheel.forwardFriction;    
                wheelForwardFriction.stiffness =
                    carConfig.tireForwardStiffness.Evaluate(1 - 1 / Mathf.Pow(2, parts[(int) part.tire]));
                var wheelSideFriction = wheel.sidewaysFriction;    
                wheelSideFriction.stiffness =
                    carConfig.tireSideStiffness.Evaluate(1 - 1 / Mathf.Pow(2, parts[(int) part.tire]));
                wheel.forwardFriction = wheelForwardFriction;
                wheel.sidewaysFriction = wheelSideFriction;
            });
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
            {/*
                if ((Vector3.Dot(carRB.velocity,transform.forward) < 0 || carRB.velocity.magnitude < 1) && pci.footBrake > .8f)
                {
                    wheel.motorTorque = -carConfig.reverseSpeed;
                return;
                }*/
                    wheel.brakeTorque = pci.footBrake * carConfig.maxBrake * Mathf.Sqrt(carRB.velocity.magnitude);
                    float dot = Vector3.Dot(inputDir, groundDir) * carConfig.gearThrottles[curGear];
                    if (dot < 0)
                    {
                        if (dot < -.5f)
                            dot *= .5f;
                        else
                            dot = 0;
                    }
                    wheel.motorTorque = dot;
                    


            });
    }

    private void FixedUpdate()
    {
        carRB.AddForceAtPosition(pci.throttle * 40000.0f * (Quaternion.AngleAxis(steerAngle, transform.up) * carRB.transform.forward), carRB.transform.position);
        if(pci.throttle < 0.1f)
        {
            carRB.drag = carRB.velocity.magnitude *  0.24f;
        }
        else
        {
            carRB.drag = 1;
        }
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
                float visualWheelDir = Mathf.Clamp(-Mathf.DeltaAngle(thetaCar, thetaInput) - 90.0f, -180.0f, 0f);
                frontLWheel.localEulerAngles = new Vector3(0, visualWheelDir, 0);
                frontRWheel.localEulerAngles = new Vector3(0, visualWheelDir, 0);
                steerAngle = (inputDir.x * 50 / Mathf.Max(carRB.velocity.magnitude * 50.0f, 1.0f));
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
