using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : CarEngine
{

    protected override void Start()
    {
        engineLoops.Add(this);
    }

    public override void EngineTick()
    {
        
    }


    private void Movement()
    {
        
        carMove.movementForce = carMove.currentSpeed * transform.forward;
        transform.position += carMove.movementForce * Time.deltaTime;
    }

    private void SpeedUpForwardBackward()
    {
        if(carInput.gasInput > 0)
        {
            carMove.currentSpeed += carMove.movementAccerationRatio * Time.deltaTime;
            carMove.IsMovingForward = true;
        }
        
        if(carInput.gasInput < 0)
        {
            carMove.currentSpeed -= carMove.slowingRatio * Time.deltaTime;
            carMove.IsMovingBackWard = true;
        }
        
    }

    private void Braking()
    {
        

        if(carInput.brakeInput == 1)
        {
            if(carMove.currentSpeed <= 0) return;

            carMove.brakeRatio = (carMove.currentSpeed / carMove.slowingRatio) 
            * carMove.brakeRatioMultiplier;
            carMove.currentSpeed -= carMove.brakeRatio * Time.deltaTime;
        }   
    }

}
