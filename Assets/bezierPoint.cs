using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bezierPoint : MonoBehaviour { 

    //public Transform anchor; // Anchor point
    public Transform control0; // First Control point
    public Transform control1; // Second control point 

    private Transform anchor;

    public Transform Anchor { get { return gameObject.transform; } }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Anchor.position, control0.position);
        Gizmos.DrawLine(Anchor.position, control1.position);
    }

}
