using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarRotationData 
{
    public List<GameObject> visualObjects;
    public float visualRotateMultiplier = 15f;
    public float visualRotationAngleSpeed;
    public float accerationRotationRatio;
    [HideInInspector] public int minRequiredRotationSpeed = -2;
    [HideInInspector] public int maxRequiredRotationSpeed = 2;


    [HideInInspector] public float InitAccerationRotation;
    [HideInInspector] public float InitVisualRotationMultiplier;
    [HideInInspector] public Quaternion currentRotation;
    [HideInInspector] public float currentRotationAngle;
}
