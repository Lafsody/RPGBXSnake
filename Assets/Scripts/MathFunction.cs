
using UnityEngine;
using System.Collections;

public class MathFunction {
    
    public static float GetDistanceSquare(Vector3 v1, Vector3 v2)
    {
        return (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z);
    }
}
