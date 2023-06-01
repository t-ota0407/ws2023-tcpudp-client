using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extend
{
    public static Vector3 Clone(this Vector3 vec)
    {
        return new Vector3(vec.x, vec.y, vec.z);
    }
}
