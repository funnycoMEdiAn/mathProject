using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezierCurve : MonoBehaviour
{
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    [Range(0f, 1f)]
    public float T = 0.0f;

    private void OnDrawGizmos()
    {
        Vector3 PtA = A.transform.position;
        Vector3 PtB = B.transform.position;
        Vector3 PtC = C.transform.position;
        Vector3 PtD = D.transform.position;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(PtA, PtB);
        Gizmos.DrawLine(PtB, PtC);
        Gizmos.DrawLine(PtC, PtD);

        //Lerp
        Vector3 PtX = (1 - T) * PtA + T * PtB;
        Vector3 PtY = (1 - T) * PtB + T * PtC;
        Vector3 PtZ = (1 - T) * PtC + T * PtD;

        //Fraw spheres at points X, Y, Z
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(PtX, 0.08f);
        Gizmos.DrawSphere(PtY, 0.08f);
        Gizmos.DrawSphere(PtZ, 0.08f);

        //Draw lines from X to Y, Y to Z
        Gizmos.color = Color.white;
        Gizmos.DrawLine(PtX, PtY);
        Gizmos.DrawLine(PtY, PtZ);

        //Lerp
        Vector3 PtR = (1 - T) * PtX + T * PtY;
        Vector3 PtS = (1 - T) * PtY + T * PtZ;

        //Draw spehers at ponits R and S
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(PtR, 0.05f);
        Gizmos.DrawSphere(PtS, 0.05f);


        Gizmos.color = Color.white;
        Gizmos.DrawLine(PtR, PtS);

        //Lerp
        Vector3 PtO = (1 - T) * PtR + T * PtS;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(PtO, 0.05f);

    }
}
