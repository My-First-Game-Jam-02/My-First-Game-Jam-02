using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class SteeringBehaviour: MonoBehaviour
{

    [SerializeField] protected float Range;
    protected int resolution { get; private set; } // The number of directions we compute weights for.
    protected float[] steeringMap = null; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
    protected float resolutionAngle { get; private set; } // Each point is seperated by a some degrees rotation (360/steeringMap.Length)

    [Header("Debug")]
    [SerializeField] private bool ShowDebug = false;
    [SerializeField] private float MapSize = 2f;
    [SerializeField] private Color DebugColor = Color.green;

    /// <summary>
    /// Instantiates the context map weights and computes the angle between each direction
    /// </summary>
    /// <param name="resolution"></param>
    public void InstantiateContextMap(int resolution)
    {
        this.resolution = resolution;
        resolutionAngle = 360 / (float)resolution;
        steeringMap = new float[resolution];        
    }

    /// <summary>
    /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
    /// </summary>
    /// <returns></returns>
    public abstract float[] BuildContextMap();


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        if (!ShowDebug || steeringMap is null || steeringMap.Length == 0)
        {
            return;
        }

        Vector3 position = transform.position;
        Handles.DrawWireDisc(position, Vector3.forward, Range);

        position = new Vector3(position.x, position.y + 0.1f, position.z);
        Vector3 direction = Vector3.up;

        foreach (float weight in MapOperations.NormaliseMap(steeringMap, MapSize))
        {
            Gizmos.color = DebugColor;
            Gizmos.DrawRay(transform.position, direction * weight);
            direction = Quaternion.Euler(0, 0, resolutionAngle) * direction;
        }
        Handles.color = DebugColor;
        Handles.DrawWireDisc(position, Vector3.forward, MapSize);

    }
#endif

}
