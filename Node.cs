using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Node : MonoBehaviour
{
    Vector3 position;
    public Vector3 velocity;
    float rad = 2.5f;

    public void Initialize(Vector3 pos, Vector3 vel)
    {
        position = pos;
        velocity = vel;
    }

    public Vector3 CalculateDestination()
    {
        return position + velocity * 1.5f;
    }
    
    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool IsPointInside(Vector3 pos)
    {
        bool x = false;
        bool y = false;
        bool z = false;
        if ((pos.x <= position.x + rad) && (pos.x >= position.x - rad)){ x = true; }
        if ((pos.y <= position.y + rad) && (pos.y >= position.y - rad)){ y = true; }
        if ((pos.z <= position.z + rad) && (pos.z >= position.z - rad)){ z = true; }
        return x && y && z;
    }
}
