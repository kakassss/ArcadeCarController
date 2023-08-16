using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarEngine : MonoBehaviour, IEngineTick
{
    protected List<IEngineTick> engineLoops;

    protected CarEngineManager engineManager;

    protected CarMovementData carMove => engineManager.carMovement;
    protected CarInputData carInput => engineManager.carInput;
    protected CarRotationData carRotate => engineManager.carRotation;

    private void Awake()
    {
        engineManager = GetComponent<CarEngineManager>();    
    }
    private void StartLoops()
    {
        for (int i = 0; i < engineLoops.Count; i++)
        {
            engineLoops[i].EngineTick();
        }
    }

    public virtual void Update()
    {
        StartLoops();
    }

    protected abstract void Start();

    public abstract void EngineTick();
}
