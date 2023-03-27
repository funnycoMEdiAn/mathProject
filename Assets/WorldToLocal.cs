using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToLocal : MonoBehaviour
{
    public GameObject world_point;

    public float local_x;
    public float local_y;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(world_point.transform.position, 0.05f);

        Vector2 v = world_point.transform.position - transform.position;

        // Compute the local coords using dot product
        local_x = Vector2.Dot(v, transform.right);
        local_y = Vector2.Dot(v, transform.up);
    }


}
