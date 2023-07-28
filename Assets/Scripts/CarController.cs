using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Forward Transform Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRSpeed;
    [SerializeField] private float movementAccerationRatio;
    [SerializeField] private float slowingRatio;
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

    //[SerializeField] private float maxRotationAcceration;
    [SerializeField] private float accerationRotationRatio;
    private float InitAccerationRotation;
    private Quaternion currentRotation;
    private float currentRotationAngle;

    private void Start()
    {
        InitAccerationRotation = accerationRotationRatio;    
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
        BodyRotation();
    }

    private void Movement()
    {
        Vector3 newPos = currentSpeed * transform.forward;
        transform.position += newPos * Time.deltaTime;
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

        Debug.Log("onur currentSpeed " + currentSpeed);

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
            accerationRotationRatio = 0;
            return;
        }

        if(currentSpeed <= currentSpeed / 2)
        {
            accerationRotationRatio = InitAccerationRotation;
        }

        if(currentSpeed >= currentSpeed / 2)
        {
            accerationRotationRatio = InitAccerationRotation / 2;
        }
        
    }

    private void BodyRotation()
    {
        if(directionInput > 0)
        {
            currentRotationAngle = accerationRotationRatio * Time.deltaTime;
            currentRotation = Quaternion.Euler(0,currentRotationAngle,0);
            transform.rotation *= currentRotation;
        }
        else if(directionInput < 0)
        {
            currentRotationAngle = accerationRotationRatio * Time.deltaTime;
            currentRotation = Quaternion.Euler(0,-currentRotationAngle,0);
            transform.rotation *= currentRotation;
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

    // private void ClampCurrentRotationAngle()
    // {
    //     if(directionInput > 0)
    //     {
    //         currentRotationAngle = Mathf.Clamp(currentRotationAngle,currentRotationAngle,maxRotationAngle);
    //     }
    //     if(directionInput < 0)
    //     {
    //         currentRotationAngle = Mathf.Clamp(currentRotationAngle,maxRotationAngle,currentRotationAngle);
    //     }
    // }

}
