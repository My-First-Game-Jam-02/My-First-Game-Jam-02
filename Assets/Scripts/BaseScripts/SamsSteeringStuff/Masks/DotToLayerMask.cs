using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotToLayerMask : SteeringMask
{
    [SerializeField] LayerMask LayersForSteeringMask;
    [SerializeField] float weight = 1f;

    public override float[] BuildMaskMap()
    {
        maskMap = new float[resolution];

        Collider2D[] checkLayers = Physics2D.OverlapCircleAll(transform.position, Range, LayersForSteeringMask);
        if (checkLayers != null)
        {
            foreach (Collider2D collision in checkLayers)
            {
                Vector3 direction = MapOperations.VectorToTarget(transform.position, collision.ClosestPoint(transform.position));
                Vector3 mapVector = Vector3.up;
                for (int i = 0; i < maskMap.Length; i++)
                {
                    maskMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                    mapVector = Quaternion.Euler(0f, 0f, resolutionAngle) * mapVector;
                }
            }
        }
        return maskMap;
    }
}
