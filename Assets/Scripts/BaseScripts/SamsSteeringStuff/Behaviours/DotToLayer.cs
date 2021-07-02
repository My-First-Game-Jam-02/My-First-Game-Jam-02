using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotToLayer : SteeringBehaviour
{
    [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
    [SerializeField] LayerMask Layers;
    [SerializeField] float weight = 1f;

    public override float[] BuildContextMap()
    {
        steeringMap = new float[resolution];

        Collider2D[] checkLayers = Physics2D.OverlapCircleAll(transform.position, Range, Layers);
        if (checkLayers != null)
        {
            foreach (Collider2D collision in checkLayers)
            {
                Vector3 direction = MapOperations.VectorToTarget(transform.position, collision.ClosestPoint(transform.position));
                Vector3 mapVector = Vector3.up;
                for (int i = 0; i < steeringMap.Length; i++)
                {
                    steeringMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                    mapVector = Quaternion.Euler(0f, 0f, resolutionAngle) * mapVector;
                }
            }
        }

        if (direction == SteerDirection.REPULSE)
            steeringMap = MapOperations.ReverseMap(steeringMap);

        return steeringMap;
    }
}
