using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSuspension : MonoBehaviour
{
    [SerializeField] private List<GameObject> springs;
  
    [SerializeField] private float maxForce;
    [SerializeField] private float maxDistance;
    [SerializeField] private float gravityMultiplier;

    private Rigidbody rigidbody;
    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        SuspansionBehaviour();
        ApplyGravity();
    }

    private void SuspansionBehaviour() // Currently not working as it should
    {

        foreach (var spring in springs) 
        {
            RaycastHit hit;
            if(Physics.Raycast(spring.transform.position,-transform.up, out hit, maxDistance))
            {
                rigidbody.AddForceAtPosition(maxForce * Time.fixedDeltaTime * transform.up *
                    (maxDistance - hit.distance) / maxDistance,
                    spring.transform.position);
            }
        }
    }

    private void ApplyGravity()
    {
        rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass) * gravityMultiplier);
    }

}
