using UnityEngine;

public class Seek : MonoBehaviour, ISteeringBehaviour
{
    public Transform target;
    public int maxVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 GetDesiredVelocity ()
    {
        if (!target) return Vector3.zero;
        var desiredVelocity = (target.position - transform.position).normalized * maxVelocity;
        var steering = desiredVelocity - transform.position;
        return steering;
    }
}
