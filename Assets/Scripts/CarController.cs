using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UIElements;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backLeft;
    [SerializeField] WheelCollider backRight;

    public float Acceleration = 300f;
    public float MaxTurtnAngle = 7f;

    private GameObject character;
    private Animator anim;
    private Animator characterAnim;
    private AudioSource[] sources;
    private float currentAcceleration = 0f;
    private float brakeState = 0;
    private float currentTurnAngle = 0f;
    private bool wasStarted = false;
    private bool isStarted = false;


    void Start()
    {
        character = GameObject.Find("Character");
        sources = GetComponents<AudioSource>();
        anim = GetComponent<Animator>();
        characterAnim = character.GetComponent<Animator>();
    }


    void Update()
    {
        if (isStarted)
        {
            currentAcceleration = Acceleration * Input.GetAxis("Vertical");
            currentTurnAngle = MaxTurtnAngle * Input.GetAxis("Horizontal");
        }
        EnginePowerSwitch();
        EngineSound();
        ElevatorSwitch();
        BrakeCheck();


        frontLeft.brakeTorque = brakeState;
        frontRight.brakeTorque = brakeState;
        backLeft.brakeTorque = brakeState;
        backRight.brakeTorque = brakeState;

        frontLeft.motorTorque = currentAcceleration;
        frontRight.motorTorque = currentAcceleration;

        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }


    void EngineSound()
    {
        if (isStarted && !sources[1].isPlaying && !wasStarted)
        {
            sources[0].Play();
            wasStarted = true;
        }
        else if (isStarted && !sources[0].isPlaying && !sources[1].isPlaying && wasStarted)
        {
            sources[1].Play();
        }
        else if (!isStarted)
        {
            if (sources[0].isPlaying)
            {
                sources[0].Stop();
            }
            if (sources[1].isPlaying)
            {
                sources[1].Stop();
            }
            wasStarted = false;
        }
    }


    void ElevatorSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("isUp") && isStarted)
            {
                characterAnim.Play("CharacterDown");
                anim.Play("Down");
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("isDown") && isStarted)
            {
                characterAnim.Play("CharacterUp");
                anim.Play("Up");
            }
        }
    }

    
    void BrakeCheck()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            brakeState = 1;
        }
        else
        {
            brakeState = 0;
        }
    }


    void EnginePowerSwitch()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isStarted = !isStarted;
        }
    }
}
