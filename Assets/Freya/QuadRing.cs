using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class QuadRing : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField] float radiusInner;
    [Range(0.01f, 2)]
    [SerializeField] float thickness;
    [Range(3, 20)]
    [SerializeField] int angularSegmentCount = 3;
    Mesh mesh;
    float RadiusOuter => radiusInner + thickness;
    int VertexCount => angularSegmentCount * 2;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation, radiusInner, angularSegmentCount);
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation, RadiusOuter, angularSegmentCount);
    }
    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "QuadRing";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    void Update()
    {
        GenerateMesh();
    }
    void GenerateMesh()
    {
        mesh.Clear();
        int vCount = VertexCount;
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < angularSegmentCount; i++)
        {
            float t = i / (float)angularSegmentCount;
            float angRad = t * Mathfs.TAU;
            Vector2 dir = Mathfs.GetUnitVectorByAngle(angRad);
            vertices.Add(dir * RadiusOuter);
            vertices.Add(dir * radiusInner);
        }
        List<int> triangleIndices = new List<int>();
        for (int i = 0; i < angularSegmentCount; i++)
        {
            int indexRoot = i * 2;
            int indexInnerRoot = indexRoot + 1;
            int indexOuterNext = (indexRoot + 2) % vCount;
            int indexInnerNext = (indexRoot + 3) % vCount;

            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexOuterNext);
            triangleIndices.Add(indexInnerNext);

            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexInnerNext);
            triangleIndices.Add(indexInnerRoot);
        }
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangleIndices, 0);
        mesh.RecalculateNormals();
    }
}