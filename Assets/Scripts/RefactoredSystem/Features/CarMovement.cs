using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : CarEngine
{
    
    private CarRotationData rotationData;
    private CarMovementData movementData;
    private CarInputData inputData;
    private CarGroundData groundData;

    protected void Awake()
    {
        base.Awake();

        rotationData = engineManager.carRotation;
        movementData = engineManager.carMovement;
        inputData = engineManager.carInput;
        groundData = engineManager.carGroundData;
    }

    protected override void Start()
    {
        movementData.InitMovementAccerationRatio = movementData.movementAccerationRatio;  
    }
    
    protected override void Update()
    {
        EngineTick();
    }

    public override void EngineTick()
    {
        ClampCurrentMaxSpeed();
        ReduceSpeedAndRotationWhileOnAir();
        CalculateNoInputSlowDown();
        SpeedUpForwardBackward();
        Movement();
        Braking();
    }

    private void Movement()
    {
        movementData.movementForce = movementData.currentSpeed * transform.forward;
        transform.position += movementData.movementForce * Time.deltaTime;
    }

    private void SpeedUpForwardBackward()
    {
        if(inputData.gasInput > 0)
        {
            movementData.currentSpeed += movementData.movementAccerationRatio * Time.deltaTime;
            movementData.IsMovingForward = true;
        }
        
        if(inputData.gasInput < 0)
        {
            movementData.currentSpeed -= movementData.slowingRatio * Time.deltaTime;
            movementData.IsMovingBackWard = true;
        }
        
    }

    private void Braking()
    {
        if(inputData.brakeInput == 1)
        {
            if(movementData.currentSpeed <= 0) return;

            movementData.brakeRatio = (movementData.currentSpeed / movementData.slowingRatio) 
            * movementData.brakeRatioMultiplier;
            movementData.currentSpeed -= movementData.brakeRatio * Time.deltaTime;
        }   
    }

    private void CalculateNoInputSlowDown()
    {
        if(inputData.gasInput == 0 && movementData.IsMovingForward)
        {
            movementData.currentSpeed -= movementData.slowingRatio * Time.deltaTime;
            if(movementData.currentSpeed <= 0) 
            {
                movementData.IsMovingForward = false;
            }
        }

        if(inputData.gasInput == 0 && movementData.IsMovingBackWard)
        {
            movementData.currentSpeed += movementData.slowingRatio * Time.deltaTime;
            if(movementData.currentSpeed >= 0) 
            {
                movementData.IsMovingBackWard = false;
            }
        }
    }

    private void ClampCurrentMaxSpeed()
    {
        if(inputData.gasInput > 0)
        {
            movementData.currentSpeed = Mathf.Clamp(movementData.currentSpeed,movementData.currentSpeed,movementData.maxSpeed);
        }
        if(inputData.gasInput < 0)
        {
            movementData.currentSpeed = Mathf.Clamp(movementData.currentSpeed,movementData.maxRSpeed,movementData.currentSpeed);
        }
    }

    private void ReduceSpeedAndRotationWhileOnAir()
    {
        if(!CheckIsGrounded())
        {
            movementData.movementAccerationRatio = movementData.InitMovementAccerationRatio / 3;
            rotationData.accerationRotationRatio = rotationData.InitAccerationRotation / 3;
            rotationData.visualRotateMultiplier = rotationData.InitVisualRotationMultiplier / 4;
        }
        else
        {
            movementData.movementAccerationRatio = movementData.InitMovementAccerationRatio;
        }
    }


    private bool CheckIsGrounded()
    {
        RaycastHit hit;
        return groundData.IsGrounded = Physics.Raycast(transform.position + new Vector3(0,0.5f,0),Vector3.down, out hit, groundData.raycastDistance,groundData.layer);
    }
}
