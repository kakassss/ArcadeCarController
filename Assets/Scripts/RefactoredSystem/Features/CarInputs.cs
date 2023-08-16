using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputs : CarEngine
{

    protected override void Start()
    {
        engineLoops.Add(this);
    }

    public override void EngineTick()
    {
        engineManager.carInput.gasInput = Input.GetAxis("Vertical");
        engineManager.carInput.directionInput = Input.GetAxisRaw("Horizontal");
        engineManager.carInput.brakeInput = Input.GetAxisRaw("Jump");
    }
}
