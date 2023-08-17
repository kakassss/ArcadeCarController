using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotation : CarEngine
{
    private CarRotationData rotationData;
    private CarMovementData movementData;
    private CarInputData inputData;
    protected void Awake()
    {
        base.Awake();

        rotationData = engineManager.carRotation;
        movementData = engineManager.carMovement;
        inputData = engineManager.carInput;
    }

    protected override void Start()
    {
        rotationData.InitAccerationRotation = rotationData.accerationRotationRatio;
        rotationData.InitVisualRotationMultiplier = rotationData.visualRotateMultiplier; 
    }
    protected override void Update()
    {
        EngineTick();
    }
    public override void EngineTick()
    {
        CalculateRotationBasedOnCurrentSpeed();
        BodyRotation();
        VisualRotation();
    }

    private void CalculateRotationBasedOnCurrentSpeed()
    {
        
        if(movementData.currentSpeed >= rotationData.minRequiredRotationSpeed && movementData.currentSpeed <= rotationData.maxRequiredRotationSpeed)
        {
            rotationData.accerationRotationRatio = 2f;
            rotationData.visualRotateMultiplier = 2f;
            return;
        }

        if(movementData.currentSpeed <= movementData.maxSpeed / 2)
        {
            rotationData.accerationRotationRatio = rotationData.InitAccerationRotation;
            rotationData.visualRotateMultiplier = rotationData.InitVisualRotationMultiplier;
        }

        if(movementData.currentSpeed >= movementData.maxSpeed / 2)
        {
            rotationData.accerationRotationRatio = rotationData.InitAccerationRotation / 2;
            rotationData.visualRotateMultiplier = rotationData.InitVisualRotationMultiplier / 2;
        }
        
    }

    private void BodyRotation()
    {
        if(inputData.directionInput > 0)
        {
            Rotating(1); //Right Direction
        }
        if(inputData.directionInput < 0)
        {
            Rotating(-1); //Left Direction
        }

        void Rotating(float direction)
        {
            rotationData.currentRotationAngle = rotationData.accerationRotationRatio * Time.deltaTime;
            rotationData.currentRotation = Quaternion.Euler(0,direction * rotationData.currentRotationAngle,0);
            transform.rotation *= rotationData.currentRotation;
        }
    }  

    private void VisualRotation()
    {
        foreach (var item in rotationData.visualObjects)
        {   
            item.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(item.transform.localEulerAngles.y,(inputData.directionInput * rotationData.visualRotateMultiplier),rotationData.visualRotationAngleSpeed), 0);

            if(inputData.directionInput == 0)
            {
                item.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(item.transform.localEulerAngles.y,0,rotationData.visualRotationAngleSpeed), Time.deltaTime);
            }
        }

    } 

}
