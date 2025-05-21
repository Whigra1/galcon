using System.Collections.Generic;
using UnityEngine;

public class SteeringContoller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private List<ISteeringBehaviour> _steeringBehaviours = new List<ISteeringBehaviour>(); 
    void Start()
    {
        _steeringBehaviours.AddRange(GetComponents<ISteeringBehaviour>());   
    }

    // Update is called once per frame
    void Update()
    {
        var steering = Vector3.zero;
        foreach (var steeringBehaviour in _steeringBehaviours)
        {
            var desiredVelocity = steeringBehaviour.GetDesiredVelocity() * Time.deltaTime; 
            steering += desiredVelocity;
            var angle = Mathf.Atan2(desiredVelocity.y, desiredVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        transform.position += steering;
    }
}
