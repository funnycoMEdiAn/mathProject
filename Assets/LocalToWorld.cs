using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToWorld : MonoBehaviour
{
    public float LocalX = 0.0f;
    public float LocalY = 0.0f;

    public void OnDrawGizmos()
    {
        // Local to World
        Vector2 worldPoint = transform.position + LocalX * transform.right + LocalY * transform.up;

        //World axes
        DrawVector(Vector2.zero, Vector2.right, Color.red);
        DrawVector(Vector2.zero, Vector2.up, Color.green);

        //Local axes
        DrawVector(transform.position, transform.position + transform.right, Color.red);
        DrawVector(transform.position, transform.position + transform.up, Color.green);

        //Debug.Log(transform.localToWorldMatrix.ToString());

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(worldPoint, 0.05f);

    }

    private void DrawVector(Vector3 from, Vector3 to, Color c)
    {
        Color curr = Gizmos.color;
        Gizmos.color = c;
        Gizmos.DrawLine(from, to);
        // Compute a location from "to towards from with 30degs"
        Vector3 loc = -(to - from);
        loc = Vector3.ClampMagnitude(loc, 0.1f);
        Quaternion rot30 = Quaternion.Euler(0, 0, 30);
        Vector3 loc1 = rot30 * loc;
        rot30 = Quaternion.Euler(0, 0, -30);
        Vector3 loc2 = rot30 * loc;
        Gizmos.DrawLine(to, to + loc1);
        Gizmos.DrawLine(to, to + loc2);
        Gizmos.color = curr;
    }

}
