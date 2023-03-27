using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class Mathfs 
{
    public const float TAU = 6.28318530718f;

    public static Vector2 GetUnitVectorByAngle(float andRag)
    {
        return new Vector2(
            Mathf.Cos(andRag),
            Mathf.Sin(andRag)
            );
    }


}
