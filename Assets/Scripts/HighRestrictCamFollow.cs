using UnityEngine;

public class HighRestrictCamFollow : MonoBehaviour
{
    public Transform target; // the object that the camera should follow
    public float smoothTime = 0.3f; // the time it takes for the camera to catch up to the target
    public Vector3 offset; // the offset between the camera and the target
    public bool restrictX; // whether to restrict the camera's movement along the x-axis
    public bool restrictY; // whether to restrict the camera's movement along the y-axis
    public bool restrictZ; // whether to restrict the camera's movement along the z-axis
    public float minX; // the minimum x-coordinate for the camera's position
    public float maxX; // the maximum x-coordinate for the camera's position
    public float minY; // the minimum y-coordinate for the camera's position
    public float maxY; // the maximum y-coordinate for the camera's position
    public float minZ; // the minimum z-coordinate for the camera's position
    public float maxZ; // the maximum z-coordinate for the camera's position
    private Vector3 velocity = Vector3.zero; // the velocity of the camera's movement

    void Update()
    {
        // If the target is null, do nothing
        if (target == null)
        {
            return;
        }

        // Calculate the new position for the camera based on the target's position
        Vector3 targetPosition = target.position + offset;
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Restrict the camera's movement along the x-axis if necessary
        if (restrictX)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        }

        // Restrict the camera's movement along the y-axis if necessary
        if (restrictY)
        {
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        }

        // Restrict the camera's movement along the z-axis if necessary
        if (restrictZ)
        {
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
        }

        // Update the camera's position
        transform.position = newPosition;
    }
}


