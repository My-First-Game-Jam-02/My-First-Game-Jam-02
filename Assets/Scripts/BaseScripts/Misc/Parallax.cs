using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform follow;
    [SerializeField] float coefficient;
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;

    Vector3 lastFollowPosition;
    Vector3 initialPosition;

    void Start()
    {
        lastFollowPosition = follow.position;
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector2 followDelta = follow.position - lastFollowPosition;
        if (lockX) followDelta.x = 0;
        if (lockY) followDelta.y = 0;

        transform.position += (Vector3)followDelta * coefficient;

        lastFollowPosition = follow.position;
    }
}