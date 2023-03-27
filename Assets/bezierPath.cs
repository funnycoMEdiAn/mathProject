using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bezierPath : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    [Range(0.0f, 1.0f)]
    public float TValue = 0.0f;

    public bezierPoint[] points; //For storing all bezier points

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length-1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i + 1].Anchor.position,
                               points[i].control1.position,
                               points[i + 1].control0.position,
                               Color.magenta, default, 2f);
        }

        //Last part of the path from last bezier point to the first bezier point
        //Handles.DrawBezier(points[points.Length - 1].Anchor.position,
        //                       points[0].Anchor.position,
        //                       points[points.Length - 1].control1.position,
        //                       points[0].control0.position,
        //                       Color.magenta, default, 2f);


        // Get the point from bezier curve that corresponds our t-value
        Vector3 tPos = GetBezierPosition(TValue, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(TValue, points[0], points[1]);

        //Draw the position on the curve
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(tPos, 0.3f);

        //Draw the direction at the position
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(tPos, tPos + 5.0f * tDir);

        //Try to get the roation
        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot);

        //Draw some points wrt t-position
        for (int i = 0; i < road2D.vertices.Length; i++)
        {
            Vector3 roadPoint = road2D.vertices[i].point;
            Gizmos.DrawSphere(tPos + rot*roadPoint, 0.2f);
        }


        //Gizmos.DrawSphere(tPos + (rot * Vector3.right), 0.3f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.left), 0.3f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.up), 0.3f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.up * 2.0f), 0.3f);

    }


    Vector3 GetBezierPosition(float t, bezierPoint bp1, bezierPoint bp2)
    {
        // 1st Lerp
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.control1.position;
        Vector3 PtY = (1 - t) * bp1.control1.position + t * bp2.control0.position;
        Vector3 PtZ = (1 - t) * bp2.control0.position + t * bp2.Anchor.position;

        // 2nd Lerp
        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        return (1 - t) * PtR + t * PtS;


    }

    Vector3 GetBezierDirection(float t, bezierPoint bp1, bezierPoint bp2)
    {
        // 1st Lerp
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.control1.position;
        Vector3 PtY = (1 - t) * bp1.control1.position + t * bp2.control0.position;
        Vector3 PtZ = (1 - t) * bp2.control0.position + t * bp2.Anchor.position;

        // 2nd Lerp
        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        return (PtS - PtR).normalized;


    }


    //Vanha koodi
    /*
    public bezierPoint BezierP1;
    public bezierPoint BezierP2;
    public bezierPoint BezierP3;
    public bezierPoint BezierP4;

    private void OnDrawGizmos()
    {

        Handles.DrawBezier(BezierP1.gameObject.transform.position, 
                           BezierP2.gameObject.transform.position,
                           BezierP1.control1.position,
                           BezierP2.control0.position,
                           Color.magenta, default, 1f);

        Handles.DrawBezier(BezierP2.gameObject.transform.position,
                           BezierP3.gameObject.transform.position,
                           BezierP2.control1.position,
                           BezierP3.control0.position,
                           Color.magenta, default, 1f);

        Handles.DrawBezier(BezierP3.gameObject.transform.position,
                           BezierP4.gameObject.transform.position,
                           BezierP3.control1.position,
                           BezierP4.control0.position,
                           Color.magenta, default, 1f);

        Handles.DrawBezier(BezierP4.gameObject.transform.position,
                           BezierP1.gameObject.transform.position,
                           BezierP4.control1.position,
                           BezierP1.control0.position,
                           Color.magenta, default, 1f);



    }
    */
}
