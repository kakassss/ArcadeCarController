using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputs : CarEngine
{
    private CarInputData inputData;
    protected void Awake()
    {
        base.Awake();
        
        inputData = engineManager.carInput;
    }

    protected override void Start()
    {
        
    }
    protected override void Update()
    {
        EngineTick();
    }

    public override void EngineTick()
    {
        inputData.gasInput = Input.GetAxis("Vertical");
        inputData.directionInput = Input.GetAxisRaw("Horizontal");
        inputData.brakeInput = Input.GetAxisRaw("Jump");
    }
}
