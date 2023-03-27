using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class Vectors : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public float scalarDot;
    public float axis_length = 2.0f;



    private void OnDrawGizmos()
    {
        Vector3 origin = Vector3.zero;



        // Compute the dot product
        Vector2 vecA = a.transform.position;
        Vector2 vecB = b.transform.position;



        //float vecAlen = vecA.magnitude;
        //float vecAlen = Mathf.Sqrt( vecA.x * vecA.x + vecA.y * vecA.y);
        //Vector2 vecN = vecA / vecAlen;   // normalized vector
        Vector2 vecNA = vecA.normalized;
        Vector2 vecNB = vecB.normalized;
        scalarDot = Vector2.Dot(vecNA, vecNB);




        DrawVector(new Vector3(-axis_length, 0, 0), new Vector3(axis_length, 0, 0), Color.red);
        DrawVector(new Vector3(0, -axis_length, 0), new Vector3(0, axis_length, 0), Color.green);



        Gizmos.color = Color.white;
        Handles.DrawWireDisc(origin, Vector3.forward, 1.0f);



        // Draw vector A
        DrawVector(origin, a.transform.position, Color.black);
        Gizmos.color = Color.black;



        // Draw normalized vector (from A)
        DrawVector(origin, vecNA, Color.blue);
        // Draw normalized vector (from B)
        DrawVector(origin, vecNB, Color.blue);



        // Draw vector B
        DrawVector(origin, b.transform.position, Color.black);
        Gizmos.color = Color.black;



        // Vector projection: Dot(vecN, vecB)*vecN;
        //Vector2 vecProj = vecN * scalarDot;
        //DrawVector(Vector3.zero, vecProj, Color.magenta);



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



    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {

    }
}