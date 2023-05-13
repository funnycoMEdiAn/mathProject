using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class bezierPath : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    [Range(2, 100)]
    public int Segments = 2;

    [Range(0.1f, 10f)]
    public float RoadScaler = 1.0f;

    [Range(0.0f, 1.0f)]
    public float TValue = 0.0f;

    public bezierPoint[] points; // For storing all bezier points

    [SerializeField]
    private Mesh mesh;

    // vertices
    List<Vector3> verts = new List<Vector3>();
    // uvs
    List<Vector2> uvs = new List<Vector2>();

    List<int> triangle_indices = new List<int>();  // List of tri_inds

    [SerializeField]
    GameObject CarCube;

    private void OnDrawGizmos()
    {

        // Loop from beginning of array until the 2nd to last element (because i+1)
        for (int i = 0; i < points.Length - 1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i + 1].Anchor.position,
                               points[i].control1.position,
                               points[i + 1].control0.position,
                               Color.magenta, default, 2f);
        }

        // Last part of the path from last bezier point to first bezier point
        /* Handles.DrawBezier(points[points.Length-1].Anchor.position,
                           points[0].Anchor.position,
                           points[points.Length-1].control1.position,
                           points[0].control0.position,
                           Color.magenta, default, 2f);
        */

        float RoadLength = 0f;

        for (int i = 0; i < points.Length - 1; i++)
        {
            RoadLength += Vector3.Distance(points[i].Anchor.position, points[i + 1].Anchor.position);
        }

        // Get the point from bezier curve that corresponds our t-value
        float distanceAlongPath = RoadLength * TValue;
        int segmentIndex = 0;
        float segmentStart = 0f;
        float segmentLength = 0f;

        // Find the segment that contains the distance along the path
        for (int i = 0; i < points.Length - 1; i++)
        {
            segmentLength = Vector3.Distance(points[i].Anchor.position, points[i + 1].Anchor.position);
            if (distanceAlongPath < segmentStart + segmentLength)
            {
                segmentIndex = i;
                break;
            }
            segmentStart += segmentLength;
        }

        // Map the t-value to the range of the current segment
        float t = Mathf.InverseLerp(segmentStart, segmentStart + segmentLength, distanceAlongPath);

        Vector3 tPos = GetBezierPosition(t, points[segmentIndex], points[segmentIndex + 1]);
        Vector3 tDir = GetBezierDirection(t, points[segmentIndex], points[segmentIndex + 1]);

        // Get the point from bezier curve that corresponds our t-value
        //Vector3 tPos = GetBezierPosition(TValue, points[0], points[1]);
        //Vector3 tDir = GetBezierDirection(TValue, points[0], points[1]);

        // Draw the position on the curve
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tPos, 0.25f);

        // Try to get the rotation
        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot);

        if (CarCube != null)
        {
            CarCube.transform.position = tPos;
            CarCube.transform.rotation = Quaternion.LookRotation(tDir * 2f);
        }

        // Draws all parts of the Bezier path
        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawBezierPart(points[i], points[i + 1]);
        }

        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(tPos + (rot * Vector3.right), 0.25f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.left), 0.25f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.up), 0.25f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.up * 2.0f), 0.25f);
    }

    private void DrawBezierPart(bezierPoint point0, bezierPoint point1)
    {
        Vector3 tPos, tDir;
        Quaternion rot;

        // Draws one part of the Bezier path
        for (int n = 0; n < Segments; n++)
        {
            float tTest = n / (float)Segments;
            // Get the point from bezier curve that corresponds our t-value
            tPos = GetBezierPosition(tTest, point0, point1);
            tDir = GetBezierDirection(tTest, point0, point1);
            rot = Quaternion.LookRotation(tDir);

            float tTestNext = (n + 1) / (float)Segments;
            // Get the point from bezier curve that corresponds our t-value
            Vector3 tPosNext = GetBezierPosition(tTestNext, point0, point1);
            Vector3 tDirNext = GetBezierDirection(tTestNext, point0, point1);
            Quaternion rotNext = Quaternion.LookRotation(tDirNext);

            for (int i = 0; i < road2D.vertices.Length; i++)
            {
                Vector3 roadpoint = road2D.vertices[i].point;
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(tPos + rot * roadpoint, 0.15f);
                Gizmos.DrawSphere(tPosNext + rotNext * roadpoint, 0.15f);
                Gizmos.color = Color.green;
                // Draw lines from current to next vertex between slices
                Gizmos.DrawLine(tPos + rot * roadpoint, tPosNext + rotNext * roadpoint);

            }

            // Draw lines for each slice
            for (int i = 0; i < road2D.vertices.Length - 1; i++)
            {
                Vector3 roadpoint = road2D.vertices[i].point;
                Vector3 roadpointNext = road2D.vertices[i + 1].point;

                Gizmos.DrawLine(tPos + rot * roadpoint, tPos + rot * roadpointNext);
            }
            Vector3 pointLast = road2D.vertices[road2D.vertices.Length - 1].point;
            Vector3 pointFirst = road2D.vertices[0].point;

            Gizmos.DrawLine(tPos + rot * pointLast, tPos + rot * pointFirst);

        }

    }

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void Awake()
    {
        GenerateMesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void GenerateTrianglesForBezierPart(int part)
    {
        // triangles

        // how many lines:
        int num_lines = road2D.lineIndices.Length / 2;

        //How much we need to offset the riangle indices
        int offset = part * (Segments + 1) * road2D.vertices.Length;

        // Go through each but the last segment
        for (int n = 0; n < Segments; n++)
        {
            for (int line = 0; line < num_lines; line++)
            {
                // current "slice"
                int curr_first = offset + n * road2D.vertices.Length +
                                    road2D.lineIndices[2 * line];
                int curr_second = offset +  n * road2D.vertices.Length +
                                    road2D.lineIndices[2 * line + 1];
                // next "slice"
                int next_first = curr_first + road2D.vertices.Length;
                int next_second = curr_second + road2D.vertices.Length;

                triangle_indices.Add(curr_first); // 1st tri
                triangle_indices.Add(next_first);
                triangle_indices.Add(curr_second);
                triangle_indices.Add(curr_second);  // 2nd tri
                triangle_indices.Add(next_first);
                triangle_indices.Add(next_second);

            }

        }
    }

    private void GenerateVerticesForBezierPart(bezierPoint point0, bezierPoint point1)
    {
        // Go through each segment
        for (int n = 0; n <= Segments; n++)
        {
            // Compute the t-value for current segment
            float t = n / (float)Segments;

            // Get the point from bezier curve that corresponds our t-value
            Vector3 tPos = GetBezierPosition(t, point0, point1);
            Vector3 tDir = GetBezierDirection(t, point0, point1);
            Quaternion rot = Quaternion.LookRotation(tDir);

            // Loop through our road slice
            for (int index = 0; index < road2D.vertices.Length; index++)
            {
                // Local point
                Vector3 roadpoint = road2D.vertices[index].point * RoadScaler;
                // Local to World-transform 
                Vector3 worldpoint = tPos + rot * roadpoint;
                // Add this world point to our verts
                verts.Add(worldpoint);
                // Add the corresponding UV-coord - hack hack!
                uvs.Add(new Vector2((roadpoint.x / RoadScaler) / 10.0f + 0.5f, 3 * t)); // * t voidaan säätää meshin kokoa
            }
        }

    }

    void GenerateMesh()
    {

        verts.Clear();
        uvs.Clear();
        triangle_indices.Clear();

        //Just the first one
        //GenerateMeshForBezierPart(points[0], points[1]);

        for (int i = 0; i < points.Length - 1; i++)
        {
            GenerateVerticesForBezierPart(points[i], points[i + 1]);
            GenerateTrianglesForBezierPart(i);
        }

        // normals?

        // Clear the mesh
        if (this.mesh != null)
            this.mesh.Clear();
        else
            this.mesh = new Mesh();

        // Set everything!!!
        this.mesh.SetVertices(verts);
        this.mesh.SetUVs(0, uvs);
        this.mesh.SetTriangles(triangle_indices, 0);
        this.mesh.RecalculateNormals();

    }

    Vector3 GetBezierPosition(float t, bezierPoint bp1, bezierPoint bp2)
    {

        // 1st Lerp: 
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.control1.position;
        Vector3 PtY = (1 - t) * bp1.control1.position + t * bp2.control0.position;
        Vector3 PtZ = (1 - t) * bp2.control0.position + t * bp2.Anchor.position;

        // 2nd Lerp: 
        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        // 3rd Lerp:
        return (1 - t) * PtR + t * PtS;
    }

    Vector3 GetBezierDirection(float t, bezierPoint bp1, bezierPoint bp2)
    {

        // 1st Lerp: 
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.control1.position;
        Vector3 PtY = (1 - t) * bp1.control1.position + t * bp2.control0.position;
        Vector3 PtZ = (1 - t) * bp2.control0.position + t * bp2.Anchor.position;

        // 2nd Lerp: 
        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        // Compute the direction vector
        return (PtS - PtR).normalized;
    }

}
