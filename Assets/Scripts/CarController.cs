using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Forward Transform/Braking Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRSpeed;
    [SerializeField] private float movementAccerationRatio;
    [SerializeField] private float slowingRatio;
    [SerializeField] private float brakeRatioMultiplier;

    private Vector3 movementForce;

    private float brakeRatio;
    private float currentSpeed = 0f;
    private float InitMovementAccerationRatio;

    private bool IsMovingForward;
    private bool IsMovingBackWard;

    [Header("Inputs")]
    private float gasInput;
    private float directionInput;
    private float brakeInput;

    [Header("Rotation Movement")]
    private const int minRequiredRotationSpeed = -2;
    private const int maxRequiredRotationSpeed = 2;

    [SerializeField] List<GameObject> visualObjects;

    [SerializeField] private float visualRotateMultiplier = 15f;
    [SerializeField] private float visualRotationAngleSpeed;
    [SerializeField] private float accerationRotationRatio;

    private float InitAccerationRotation;
    private float InitVisualRotationMultiplier;
    private Quaternion currentRotation;
    private float currentRotationAngle;

    [Header("Grounded")]
    [SerializeField] private float raycastDistance = 1.0f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private bool IsGrounded;

    private void Start()
    {
        InitAccerationRotation = accerationRotationRatio;
        InitVisualRotationMultiplier = visualRotateMultiplier;    
        InitMovementAccerationRatio = movementAccerationRatio;
    }

    private void Update()
    {
        GetInputs();

        ClampCurrentMaxSpeed();
        CalculateRotationBasedOnCurrentSpeed();
        ReduceSpeedAndRotationWhileOnAir();
        CalculateNoInputSlowDown();
        SpeedUpForwardBackward();
        Movement();
        Braking();
        BodyRotation();
        VisualRotation();
    }

    private void GetInputs()
    {
        gasInput = Input.GetAxis("Vertical");
        directionInput = Input.GetAxisRaw("Horizontal");
        brakeInput = Input.GetAxisRaw("Jump");
    }

    private void Movement()
    {
        movementForce = currentSpeed * transform.forward;
        transform.position += movementForce * Time.deltaTime;
    }

    private void ReduceSpeedAndRotationWhileOnAir()
    {
        if(!CheckIsGrounded())
        {
            movementAccerationRatio = InitMovementAccerationRatio / 3;
            accerationRotationRatio = InitAccerationRotation / 3;
            visualRotateMultiplier = InitVisualRotationMultiplier / 4;
        }
        else
        {
            movementAccerationRatio = InitMovementAccerationRatio;
        }
    }

    private void SpeedUpForwardBackward()
    {
        if(gasInput > 0)
        {
            currentSpeed += movementAccerationRatio * Time.deltaTime;
            IsMovingForward = true;
        }
        
        if(gasInput < 0)
        {
            currentSpeed -= slowingRatio * Time.deltaTime;
            IsMovingBackWard = true;
        }
        
    }

    private void Braking()
    {
        if(brakeInput == 1)
        {
            if(currentSpeed <= 0) return;

            brakeRatio = (currentSpeed / slowingRatio) * brakeRatioMultiplier;
            currentSpeed -= brakeRatio * Time.deltaTime;
        }   
    }


    private void CalculateNoInputSlowDown()
    {
        if(gasInput == 0 && IsMovingForward)
        {
            currentSpeed -= slowingRatio * Time.deltaTime;
            if(currentSpeed <= 0) 
            {
                IsMovingForward = false;
            }
        }

        if(gasInput == 0 && IsMovingBackWard)
        {
            currentSpeed += slowingRatio * Time.deltaTime;
            if(currentSpeed >= 0) 
            {
                IsMovingBackWard = false;
            }
        }
    }

    private void CalculateRotationBasedOnCurrentSpeed()
    {
        
        if(currentSpeed >= minRequiredRotationSpeed && currentSpeed <= maxRequiredRotationSpeed)
        {
            accerationRotationRatio = 2f;
            visualRotateMultiplier = 2f;
            return;
        }

        if(currentSpeed <= maxSpeed / 2)
        {
            accerationRotationRatio = InitAccerationRotation;
            visualRotateMultiplier = InitVisualRotationMultiplier;
        }

        if(currentSpeed >= maxSpeed / 2)
        {
            accerationRotationRatio = InitAccerationRotation / 2;
            visualRotateMultiplier = InitVisualRotationMultiplier / 2;
        }
        
    }
    
    private void BodyRotation()
    {
        if(directionInput > 0)
        {
            Rotating(1); //Right Direction
        }
        if(directionInput < 0)
        {
            Rotating(-1); //Left Direction
        }

        void Rotating(float direction)
        {
            currentRotationAngle = accerationRotationRatio * Time.deltaTime;
            currentRotation = Quaternion.Euler(0,direction * currentRotationAngle,0);
            transform.rotation *= currentRotation;
        }
    }   

    private void VisualRotation()
    {
        foreach (var item in visualObjects)
        {   
            item.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(item.transform.localEulerAngles.y,(directionInput * visualRotateMultiplier),visualRotationAngleSpeed), 0);

            if(directionInput == 0)
            {
                item.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(item.transform.localEulerAngles.y,0,visualRotationAngleSpeed), Time.deltaTime);
            }
        }

    }

    private bool CheckIsGrounded()
    {
        RaycastHit hit;
        return IsGrounded = Physics.Raycast(transform.position + new Vector3(0,0.5f,0),Vector3.down, out hit, raycastDistance,layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 endPosition = transform.position + Vector3.down ;
        Gizmos.DrawLine(transform.position + new Vector3(0,0.5f,0), endPosition);
    }

    private void ClampCurrentMaxSpeed()
    {
        if(gasInput > 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed,currentSpeed,maxSpeed);
        }
        if(gasInput < 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed,maxRSpeed,currentSpeed);
        }
    }
}
