using UnityEngine;

public class AllInOneSteering : MonoBehaviour
{
    public Transform seek;
    private Vector3 Velocity => transform.position - _prevVelocity;
    private Vector3 _prevVelocity = Vector3.zero;
    public bool seekEnabled = true;
    public int maxSpeed = 30;
    public int maxViewDistance = 10;
    public float avoidanceForce = 20;
    public float separationForce = 20;
    public float maxAllowedForce = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        var steering = Vector3.zero;

        if (seek && seekEnabled) steering += Seek();
        // if (!seek && seekEnabled) steering += FollowCursor();
        steering += Avoid();
        steering += Separate();
        _prevVelocity = transform.position;
        transform.position += (steering + Velocity) * Time.deltaTime;
        var angle = Mathf.Atan2(steering.y, steering.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private Vector3 Seek()
    {
        var desiredVelocity = (seek.position - transform.position).normalized * maxSpeed;
        return desiredVelocity - Velocity;
    }

    private Vector3 Avoid()
    {
        var ahead = Velocity.normalized * maxViewDistance;
        var hit = Physics2D.Raycast(transform.position, ahead, maxViewDistance);
        // Debug.DrawRay(transform.position, Velocity.normalized * maxViewDistance, Color.red);
        // Debug.DrawRay(transform.position, Velocity*60, Color.magenta);
        if (!hit.collider) return Vector3.zero;
        if (hit.collider.gameObject == gameObject) return Vector3.zero;
        var planet = hit.collider.gameObject.GetComponent<Planet>();
        if (!planet) return Vector3.zero;
        var avoidance = (ahead + transform.position - planet.transform.position).normalized;
        // Debug.DrawRay(planet.transform.position, (ahead + transform.position - planet.transform.position), Color.yellow);
        // Debug.Log($"Velocity: {Velocity}/Velocity(norm): {Velocity.normalized}/Ahead: {ahead} / Ahead (with current pos): {transform.position + ahead} / Ahead - planet: {ahead - planet.transform.position }/ avoidance: {avoidance}");
        if (seek.transform.position == planet.transform.position)
        {
            return Vector3.zero;
        }
        var distance = hit.distance;
        Debug.DrawRay(transform.position, avoidance*avoidanceForce, Color.green);
     
        // if (distance < 5)
        // {
        //     return -steering + ((avoidance - -steering) / 2 - Velocity);
        //     return Vector3.ClampMagnitude(-steering, avoidanceForce*2);
        // }
        return avoidance.normalized * avoidanceForce; // Vector3.ClampMagnitude(avoidance - Velocity, avoidanceForce);
    }

    private Vector3 Separate()
    {
        var nearbyTriangles = Physics2D.OverlapCircleAll(transform.position, 10, LayerMask.GetMask("Default"));
        // Debug.Log($"Nearby triangles: {nearbyTriangles.Length}");
        var sum = Vector3.zero;
        var desiredDistance = 20;
        var count = 0;
        foreach (var collider in nearbyTriangles)
        {
            var nearObjectWithCollider = collider.gameObject;
            if (nearObjectWithCollider == gameObject) continue;
            if (nearObjectWithCollider.tag != "Triangle") continue;
            var distance = Vector3.Distance(transform.position, nearObjectWithCollider.transform.position);
            if (distance > desiredDistance) continue;
            var direction = (transform.position - nearObjectWithCollider.transform.position).normalized;
            sum += direction * (1 / distance);
            count++;
        }

        if (count <= 0) return Vector3.zero;
        
        sum = sum.normalized * separationForce;
        var steering = sum - Velocity;
        return steering.normalized * separationForce;

    }

    private Vector3 FollowCursor()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; 
        var targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var desiredVelocity = targetPosition - transform.position;

        if (desiredVelocity.magnitude > maxSpeed)
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
        }

        return desiredVelocity;
    }
}
