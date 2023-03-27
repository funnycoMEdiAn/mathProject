using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Mesh2D : ScriptableObject
{
    [System.Serializable]
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
        //public Vector2 u; // Later hater

    }

    public Vertex[] vertices;
    public int[] lineIndices;


}
