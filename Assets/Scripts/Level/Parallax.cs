using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera Cam;
    public Transform Subject;

    private Vector2 StartPosition;
    private float StartY, StartZ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPosition = transform.position;
        StartY = transform.position.y;
        StartZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float distToFarPlane = Mathf.Abs(Subject.position.z - Cam.farClipPlane);
        float distToNearPlane = Mathf.Abs(Subject.position.z - Cam.nearClipPlane);

        Vector2 travel = (Vector2)Cam.transform.position - StartPosition;
        float zDistanceToSubject = transform.position.z - Subject.position.z;

        float clippingPlane = (Cam.transform.position.z + (zDistanceToSubject > 0 ? distToFarPlane : distToNearPlane));

        float parallaxFactor = Mathf.Abs(zDistanceToSubject) / clippingPlane;

        Vector3 newPos = StartPosition + (travel * parallaxFactor);
        transform.position = new Vector3(newPos.x, StartY, StartZ);
    }
}
