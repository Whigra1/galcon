using UnityEngine;

public class AvoidPlanet : MonoBehaviour, ISteeringBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maxViewDistance = 10;
    public float avoidance_force = 20;
    private Vector3 prevPosition = Vector3.zero; 
    public Vector3 GetDesiredVelocity()
    {
        var velocity = (transform.position - prevPosition).normalized;
        var ahead = velocity * maxViewDistance;
        var hit = Physics2D.Raycast(transform.position, ahead, maxViewDistance);
        prevPosition = transform.position;

        Debug.DrawRay(transform.position, ahead, Color.red);

        if (!hit.collider) return Vector3.zero;
        if (hit.collider.gameObject == gameObject) return Vector3.zero;
        
        var planet = hit.collider.gameObject.GetComponent<Planet>();
        var seek = GetComponent<Seek>();
        if (planet && seek.target.transform.position != planet.transform.position)
        {
            var avoidance = (transform.position + ahead - planet.transform.position).normalized;
            return (avoidance - velocity).normalized * avoidance_force;
        }
        return Vector3.zero;
    }
}
