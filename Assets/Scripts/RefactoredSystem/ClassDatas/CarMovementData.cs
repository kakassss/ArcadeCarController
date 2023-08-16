using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarMovementData
{
    public float maxSpeed;
    public float maxRSpeed;
    public float movementAccerationRatio;
    public float slowingRatio;
    public float brakeRatioMultiplier;

    [HideInInspector] public Vector3 movementForce;

    [HideInInspector] public float brakeRatio;
    [HideInInspector] public float currentSpeed = 0f;
    [HideInInspector] public float InitMovementAccerationRatio;

    [HideInInspector] public bool IsMovingForward;
    [HideInInspector] public bool IsMovingBackWard;
}
