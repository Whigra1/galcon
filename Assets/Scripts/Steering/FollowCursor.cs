using Unity.VisualScripting;
using UnityEngine;

public class FollowCursor : MonoBehaviour, ISteeringBehaviour
{
    public float maxVelocity = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 GetDesiredVelocity()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; // Adjust for the camera's z-axis
        var targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var desiredVelocity = targetPosition - transform.position;

        // Clamp to maximum velocity
        if (desiredVelocity.magnitude > maxVelocity)
        {
            desiredVelocity = desiredVelocity.normalized * maxVelocity;
        }

        return desiredVelocity;

    }
}

