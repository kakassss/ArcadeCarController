using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarEngine : MonoBehaviour, IEngineTick
{    
    protected CarEngineManager engineManager;

    protected virtual void Awake()
    {
        engineManager = GetComponent<CarEngineManager>();    
    }
    
    public abstract void EngineTick();
    protected abstract void Update();
    protected abstract void Start();
}
