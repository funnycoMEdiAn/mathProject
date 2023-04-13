using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMesh : MonoBehaviour
{

    [Range(3, 255)]
    public int N = 8;
    public float Radius = 1.0f;
    public float OuterRadius = 2.0f;

    private float TAU = 2 * Mathf.PI;

    private void GenerateDonut()
    {
        Mesh mesh = new Mesh();

        // Circular mesh

        // Vertices
        List<Vector3> verts = new List<Vector3>();
        //verts.Add(Vector3.zero);  // Add the center point of the "circle"

        // UVs
        List<Vector2> uvs = new List<Vector2>();

        //Normals
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < N; i++)
        {
            float theta = TAU * i / N;  // angle of current iteration (in radians)
            //Debug.Log("Angle: " + theta + ", which in deg is: " + 360f * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta),
                                    Mathf.Sin(theta),
                                    0);
            verts.Add(v * Radius); // Inner point
            verts.Add(v * OuterRadius); // Outer point

            //Better UVs???
            // Mid point (as a vector) + v ???
            Vector2 mid = new Vector2(0.5f, 0.5f);
            Vector2 s = v * 0.5f;
            uvs.Add(mid + s*(Radius/OuterRadius)); // Inner UV
            uvs.Add(mid + s); // Outer UV

            //Not great UVs
            //uvs.Add(new Vector2(i /(float)N, 0)); // Inner UV
            //uvs.Add(new Vector2(i /(float)N, 1)); // Outer UV

            // The normals should just be (0,0,1)
            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);
        }

        List<int> tri_indices = new List<int>();
        for (int i = 0; i < N - 1; i++)
        {
            int InnerFirst = 2 * i;
            int OuterFirst = InnerFirst + 1;
            int InnerSecond = OuterFirst + 1;
            int OuterSecond = InnerSecond + 1;

            // First triangle
            tri_indices.Add(InnerFirst);
            tri_indices.Add(OuterFirst);
            tri_indices.Add(OuterSecond);

            // Second triangle
            tri_indices.Add(InnerFirst);
            tri_indices.Add(OuterSecond);
            tri_indices.Add(InnerSecond);
        }

        int InFirst = 2 * (N - 1);
        int OutFirst = InFirst + 1;
        int InSecond = 0;
        int OutSecond = 1;

        // First triangle
        tri_indices.Add(InFirst);
        tri_indices.Add(OutFirst);
        tri_indices.Add(OutSecond);

        // Second triangle
        tri_indices.Add(InFirst);
        tri_indices.Add(OutSecond);
        tri_indices.Add(InSecond);

        mesh.SetVertices(verts);
        mesh.SetTriangles(tri_indices, 0);
        mesh.SetNormals(normals);
        //mesh.RecalculateNormals();
        mesh.SetUVs(0, uvs);

        GetComponent<MeshFilter>().sharedMesh = mesh;


        /* Simple plane mesh
        Vector3[] verts = {
            new Vector3(-1f,  1f, 0f),
            new Vector3( 1f,  1f, 0f),
            new Vector3(-1f, -1f, 0f),
            new Vector3( 1f, -1f, 0f)
        };

        int[] tri_indices = { 
            2, 1, 0,
            2, 3, 1
        }; 
        */

    }


    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        // Circular mesh
        List<Vector3> verts = new List<Vector3>();
        verts.Add(Vector3.zero);  // Add the center point of the "circle"
        for (int i = 0; i < N; i++)
        {
            float theta = TAU * i / N;  // angle of current iteration (in radians)
            Debug.Log("Angle: " + theta + ", which in deg is: " + 360f * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta),
                                    Mathf.Sin(theta),
                                    0);
            verts.Add(v * Radius);
        }

        List<int> tri_indices = new List<int>();
        for (int i = 0; i < N - 1; i++)
        {
            tri_indices.Add(0);
            tri_indices.Add(i + 1);
            tri_indices.Add(i + 2);
        }
        tri_indices.Add(0);
        tri_indices.Add(N);
        tri_indices.Add(1);

        mesh.SetVertices(verts);
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;


        /* Simple plane mesh
        Vector3[] verts = {
            new Vector3(-1f,  1f, 0f),
            new Vector3( 1f,  1f, 0f),
            new Vector3(-1f, -1f, 0f),
            new Vector3( 1f, -1f, 0f)
        };

        int[] tri_indices = { 
            2, 1, 0,
            2, 3, 1
        }; 
        */

    }

    // Start is called before the first frame update
    void Start()
    {
        //GenerateMesh();
        GenerateDonut();
    }

    private void OnValidate()
    {
        //GenerateMesh(); 
        GenerateDonut();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
