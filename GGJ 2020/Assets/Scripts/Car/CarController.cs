using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private part_config partConfig;
    private float steerAngle = 0;
    
    private int curGear = 0;

    private Vector3 groundVelocity => carRB.velocity - carRB.velocity.y * Vector3.up;
    
    private PlayerControlInfo pci => input[player];
    private Vector3 inputDir => pci.direction.normalized;
    private Vector3 groundDir => (transform.forward - transform.forward.y * Vector3.up).normalized;
    private int[] parts => myParts[player].val;


    private float acceleration = 1;

    private void Awake()
    {
        player = GetComponentInParent<playerID>().p;
        carRB.centerOfMass += -carRB.transform.up + -carRB.transform.forward;
        StartCoroutine(DriveNormal());
        StartCoroutine(UnFlip());
        StartCoroutine(ControlWeight());
    }

    IEnumerator ControlWeight()
    {
        while (true)
        {
            var newWeight = carConfig.defaultWeight + parts
                                .Select(((val, index) =>
                                {
                                    print($"index {index}");
                                    return val * partConfig.part_weights[index];
                                }))
                                .Aggregate((l,r) => l + r);
            carRB.mass = Mathf.LerpUnclamped(carRB.mass, newWeight, .5f);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator DriveNormal()
    {
        while (true)
        {
            ThrottleNormal();
            SteerNormal();
            rearWheels.Map(SetStiffness);
            frontWheels.Map(SetStiffness);
            yield return null;
        }
    }

    private void SetStiffness(WheelCollider wheel)
    {
        var wheelForwardFriction = wheel.forwardFriction;    
        wheelForwardFriction.stiffness =
            carConfig.tireForwardStiffness.Evaluate(1 - 1 / Mathf.Pow(2, parts[(int) part.tire] - 1));
        var wheelSideFriction = wheel.sidewaysFriction;    
        wheelSideFriction.stiffness =
            carConfig.tireSideStiffness.Evaluate(1 - 1 / Mathf.Pow(2, parts[(int) part.tire] - 1));
        wheel.forwardFriction = wheelForwardFriction;
        wheel.sidewaysFriction = wheelSideFriction;
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
                var throttle = pci.throttle * parts[(int) part.engine] - .1f;
                if ((Vector3.Dot(carRB.velocity,transform.forward) < 0 || carRB.velocity.magnitude < 100) && pci.footBrake > .1f)
                {
                    wheel.motorTorque = -carConfig.reverseSpeed * pci.footBrake * parts[(int)part.brake];
                return;
                }

                if (pci.handBrakePulled == (int)XInputDotNetPure.ButtonState.Pressed)
                {
                    wheel.brakeTorque = Mathf.Pow(2,32);
                    return;
                }
                    wheel.brakeTorque = pci.footBrake * carConfig.maxBrake * Mathf.Sqrt(carRB.velocity.magnitude);
                    if (wheel.brakeTorque > 0) return;
                    float dot = Vector3.Dot(inputDir, groundDir);


                    if (dot < 0 && throttle < .15f) //IF TODO boost reduce effect
                    {
                        wheel.motorTorque *= (1 - Mathf.Pow(dot,2)) * Time.deltaTime;
                    }
                    else
                    {
                        wheel.motorTorque = Mathf.SmoothDamp( wheel.motorTorque, carConfig.gearThrottles[curGear] * (pci.direction.magnitude + pci.throttle) * (throttle + 1),ref acceleration,.01f);
                    }

            });
    }

    private void FixedUpdate()
    {

        var rocketBoost = pci.throttle * carConfig.rearForceConstant * transform.forward * Mathf.Clamp01(2 - 1 / Mathf.Pow(2,parts[(int)part.engine] - 1));
          
        rocketBoost = new Vector3(rocketBoost.x,Mathf.Sqrt(Mathf.Abs(rocketBoost.y)),rocketBoost.z);
        bool canBoost = true;
        rearWheels.Map(wheel => canBoost &= wheel.isGrounded);
        frontWheels.Map(wheel => canBoost &= wheel.isGrounded);
        if(canBoost)
            carRB.AddForceAtPosition(rocketBoost, carRB.transform.position);
        if(pci.throttle < 0.1f && pci.direction.sqrMagnitude < .6f)
        {
            //carRB.drag = carRB.velocity.magnitude *  0.3f;
        }
        else
        {
            //carRB.drag = 1;
        }
    }

    void SteerNormal()
    {
        frontWheels.Map
        (
            wheel =>
            {
                if (pci.direction.magnitude < .05f)
                {
                    wheel.steerAngle = 0;
                    return;
                }
                
                var thetaCar = Mathf.Atan2(groundDir.z, groundDir.x) * Mathf.Rad2Deg;
                var thetaInput = Mathf.Atan2(inputDir.z, inputDir.x) * Mathf.Rad2Deg;
                var thetaDelta = Mathf.DeltaAngle(thetaCar, thetaInput);

                var maxSteer = carConfig.minSteer;
                for (int i = 0; i < myParts[player].val[(int) part.steering_wheel]; i++)
                    maxSteer += (carConfig.maxSteer - maxSteer) / Mathf.Pow(3,i);


                if (carRB.velocity.magnitude < 5 && pci.throttle < .1f)
                {
                    maxSteer *= Mathf.Lerp(1,1.7f,6 - carRB.velocity.magnitude);
                }
                
                thetaDelta = 
                    minABS(Mathf.Clamp(thetaDelta, -maxSteer, maxSteer),
                           Mathf.Clamp(thetaDelta, -maxSteer + 180, maxSteer - 180));
                
                
                float visualWheelDir = Mathf.Clamp(-Mathf.DeltaAngle(thetaCar, thetaInput) - 90.0f, -180.0f, 0f);
                frontLWheel.localEulerAngles = new Vector3(0, 90 + visualWheelDir, 0);
                frontRWheel.localEulerAngles = new Vector3(0, 90 + visualWheelDir, 0);
                //steerAngle = (inputDir.x * 50 / Mathf.Max(carRB.velocity.magnitude * 50.0f, 1.0f));
                wheel.steerAngle = -thetaDelta;
            });
    }


    private float minABS(float a, float b)
    {
        if (Mathf.Abs(a) < Mathf.Abs(b))
            return a;
        return b;
    }
}
