using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    
    
    [Header("Forward Transform Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRSpeed;
    [SerializeField] private float movementAccerationRatio;
    [SerializeField] private float slowingRatio;
    [SerializeField] private float driftForceMultiplier;
    private Vector3 movementForce;
    private float currentSpeed = 0f;
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
    private bool drifting;

    private void Start()
    {
        InitAccerationRotation = accerationRotationRatio;
        InitVisualRotationMultiplier = visualRotateMultiplier;    
    }

    private void Update()
    {
        gasInput = Input.GetAxis("Vertical");
        directionInput = Input.GetAxisRaw("Horizontal");
        brakeInput = Input.GetAxis("Jump"); 

        ClampCurrentMaxSpeed();
        CalculateRotationBasedOnCurrentSpeed();
        CalculateNoGasInputSlowDown();
        SpeedUpForwardBackward();
        Movement();
        Drifting();
        BodyRotation();
        VisualRotation();
    }

    private void Movement()
    {
        movementForce = currentSpeed * transform.forward;
        transform.position += movementForce * Time.deltaTime;
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

    private void CalculateNoGasInputSlowDown()
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

        if(currentSpeed <= currentSpeed / 2)
        {
            accerationRotationRatio = InitAccerationRotation;
            visualRotateMultiplier = InitVisualRotationMultiplier;
        }

        if(currentSpeed >= currentSpeed / 2)
        {
            accerationRotationRatio = InitAccerationRotation / 2;
            visualRotateMultiplier = InitVisualRotationMultiplier / 2;
        }
        
    }
    
    private void Drifting()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            drifting = true;
            float additiveRotateAngle = Vector3.Dot(movementForce.magnitude * transform.forward,transform.right);
            transform.Translate(Vector3.one * additiveRotateAngle,Space.Self); 
            //transform.Rotate(directionInput * additiveRotateAngle * Time.deltaTime * transform.up);
            //movementForce = Vector3.Lerp(movementForce.normalized,transform.forward,1 * Time.deltaTime) * movementForce.magnitude;
            //transform.Rotate(Vector3.up * directionInput * moveForce.magnitude * Time.deltaTime * driftForceMultiplier);
            Debug.Log(additiveRotateAngle);
        }   

        if(Input.GetKeyUp(KeyCode.Space))
        {
            drifting = false;
        }
    }

    private void BodyRotation()
    {
        if(directionInput > 0)
        {
            Rotating(1); 
        }
        if(directionInput < 0)
        {
            Rotating(-1);
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
            //item.transform.rotation = Quaternion.Lerp(item.transform.rotation,Quaternion.Euler(0,directionMultiplier * directionInput + item.transform.eulerAngles.y,0),  Time.deltaTime);
            //item.transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y + directionMultiplier * directionInput, transform.eulerAngles.z);
            //item.transform.rotation = Quaternion.AngleAxis(directionMultiplier, Vector3.);
            //item.transform.rotation = Quaternion.LookRotation(transform.forward * 15,Vector3.up);
            //item.transform.rotation = Quaternion.Slerp(item.transform.rotation,Quaternion.Euler(0,directionMultiplier,0), 3);
            //item.transform.localEulerAngles = new Vector3(item.transform.localEulerAngles.x,Mathf.LerpAngle(item.transform.localEulerAngles.y, -20f, 20f),item.transform.localEulerAngles.z);
            //item.transform.rotation *= Quaternion.Euler(0, Mathf.LerpAngle(item.transform.rotation.y,directionInput * directionMultiplier,0.2f),0);
            
            
            if(directionInput == 0)
            {
                item.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(item.transform.localEulerAngles.y,0,visualRotationAngleSpeed), Time.deltaTime);
            }
        }


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
