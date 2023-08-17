using System;
using UnityEngine;

[Serializable]
public class CarGroundData 
{
    public float raycastDistance = 1.0f;
    public LayerMask layer;
    public bool IsGrounded;

}
