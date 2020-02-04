using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

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
        carRB.centerOfMass += -carRB.transform.up + -carRB.transform.forward * 1.25f;
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
                                    return val * partConfig.part_weights[index];
                                }))
                                .Aggregate((l,r) => l + r);
            carRB.mass = Mathf.Lerp(carRB.mass, newWeight, .5f);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator DriveNormal()
    {
        while (true)
        {
            rearWheels.Map(ThrottleNormal);
            frontWheels.Map(ThrottleNormal);
            SteerNormal();
            rearWheels.Map(SetStiffness);
            frontWheels.Map(SetStiffness);
            yield return new WaitForFixedUpdate();
        }
    }

    private void SetStiffness(WheelCollider wheel)
    {
        var wheelForwardFriction = wheel.forwardFriction;
        wheelForwardFriction.stiffness =
            carConfig.tireForwardStiffness.Evaluate(1 - 1 / Mathf.Pow(2, parts[(int) part.tire] - 1));
        
        var wheelSideFriction = wheel.sidewaysFriction;    
        wheelSideFriction.stiffness =
            carConfig.tireSideStiffness.Evaluate(1 - 1 / Mathf.Pow(2, parts[(int) part.tire] - 1)) *  (1 - pci.throttle + .35f);
        
        wheel.forwardFriction = wheelForwardFriction;
        wheel.sidewaysFriction = wheelSideFriction;
    }

    IEnumerator UnFlip()
    {
        yield return new WaitForSeconds(5);
        while(true)
        {
            bool mustFlip = true;
            rearWheels.Map(wheel => mustFlip &= !wheel.isGrounded);
            frontWheels.Map(wheel => mustFlip &= !wheel.isGrounded);
            yield return null;
            while (mustFlip)
            {
                Vector3 pos = transform.position;
                yield return new WaitForSeconds(.2f);
                
                mustFlip = true;
                rearWheels.Map(wheel => mustFlip &= !wheel.isGrounded);
                frontWheels.Map(wheel => mustFlip &= !wheel.isGrounded);

                print($"mustflip: {mustFlip}. dist {Vector3.Distance(transform.position,pos)}");
                if (mustFlip && Vector3.Distance(transform.position,pos) < .5f)
                {
                    for (float dur = 0; dur < 1; dur += Time.deltaTime * 2)
                    {
                        carRB.transform.position += Vector3.up * .5f * Time.deltaTime;
                        carRB.transform.rotation = Quaternion.Lerp(carRB.rotation,Quaternion.identity, Time.deltaTime / 2);
                        carRB.velocity = inputDir * 3;
                        yield return null;
                    }
                }
            }
        }
    }

    void ThrottleNormal(WheelCollider wheel)
    {

                var throttle = pci.throttle * parts[(int) part.engine] - .1f;
                
                if (pci.footBrake > .1f)
                {
                    wheel.motorTorque = -carConfig.reverseSpeed * pci.footBrake;
                    return;
                }
                if (pci.handBrakePulled == 0 && parts[(int)part.brake] > 0)
                {
                    wheel.brakeTorque = Mathf.Pow(2,30);
                    return;
                }

                wheel.brakeTorque = 0;//pci.footBrake * carConfig.maxBrake * Mathf.Sqrt(carRB.velocity.magnitude);
                
                //if (wheel.brakeTorque > 0) return;
                
                float dot = Vector3.Dot(inputDir, groundDir);


               
                
                var target = 
                    Mathf.SmoothDamp( wheel.motorTorque, 
                        carConfig.gearThrottles[curGear] * (pci.direction.magnitude + pci.throttle) * (throttle + 1),
                        ref acceleration,.1f);

                if (parts[(int) part.brake] > 0)
                {
                    wheel.motorTorque = target;
                    if (dot < 0 && throttle < .15f) 
                    {
                        wheel.motorTorque *= Mathf.Clamp01(1.5f - Mathf.Pow(dot,2));
                    }
                }
                else
                {
                    wheel.motorTorque = Mathf.Max(target, (wheel.motorTorque + target) / 2);
                }
                
                

    }

    private void FixedUpdate()
    {

        var rocketBoost = pci.throttle * carConfig.rearForceConstant * transform.forward * Mathf.Clamp01(2 - 1 / Mathf.Pow(2,parts[(int)part.engine] - 1));
        Vector3 inputVec = new Vector3(inputDir.x, 0, inputDir.z);
        Vector3 dir = new Vector3(carRB.transform.forward.x, 0, carRB.transform.forward.z);
        carRB.AddTorque(5f * Vector3.Cross(dir.normalized, inputVec.normalized), ForceMode.Acceleration);
        
        rocketBoost = new Vector3(rocketBoost.x,Mathf.Sqrt(Mathf.Abs(rocketBoost.y)),rocketBoost.z);
        bool canBoost = false;
        rearWheels.Map(wheel => canBoost |= wheel.isGrounded);
        frontWheels.Map(wheel => canBoost |= wheel.isGrounded);
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
                    maxSteer += (carConfig.maxSteer - maxSteer) / Mathf.Pow(2,i);


                if (pci.throttle < .1f)
                {
                    maxSteer = Mathf.Lerp(maxSteer,Mathf.Max(maxSteer,75),20 - carRB.velocity.magnitude);
                }

                if (pci.handBrakePulled == 0)
                {
                    maxSteer = 60;
                }
                
                thetaDelta = 
                    minABS(Mathf.Clamp(thetaDelta, -maxSteer, maxSteer),
                           Mathf.Clamp(thetaDelta, -maxSteer + 180, maxSteer - 180));
                
                
                float visualWheelDir = Mathf.Clamp(-Mathf.DeltaAngle(thetaCar, thetaInput), -45.0f, 45f);
                frontLWheel.localEulerAngles = new Vector3(0, visualWheelDir, 0);
                frontRWheel.localEulerAngles = new Vector3(0, visualWheelDir, 0);
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
