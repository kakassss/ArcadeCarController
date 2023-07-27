using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRSpeed;
    [SerializeField] private float accerationRatio;
    [SerializeField] private float slowingRatio;
    private float currentSpeed = 0f;
    private float gasInput;
    private float directionInput;
    private float brakeInput = 0f;

    private Quaternion currentRotation;

    private bool IsMovingForward;
    private bool IsMovingBackWard;

    private void Update()
    {
        // Kullanıcıdan giriş alın
        gasInput = Input.GetAxis("Vertical");
        directionInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetAxis("Jump"); 

        SetCurrentMaxSpeed();
        SpeedUpDown();
        Movement();
        BodyRotation();
    }

    private void Movement()
    {
        Vector3 newPos = currentSpeed * transform.forward;
        transform.position += newPos * Time.deltaTime;
    }

    private void SpeedUpDown()
    {
        if(gasInput > 0)
        {
            currentSpeed += accerationRatio * Time.deltaTime;
            IsMovingForward = true;
        }
        
        if(gasInput < 0)
        {
            currentSpeed -= slowingRatio * Time.deltaTime;
            IsMovingBackWard = true;
        }

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

        Debug.Log("onur currentSpeed " + currentSpeed);

    }

    private void BodyRotation()
    {
        if(directionInput > 0)
        {
            currentRotation = Quaternion.Euler(0,5,0);
            transform.rotation *= currentRotation;
        }
    }   

    private void SetCurrentMaxSpeed()
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
