using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Vector3 position;
    Vector3 velocity;

    public void InsertDataInMeDaddy(Vector3 pos, Vector3 vel)
    {
        position = pos;
        velocity = vel;
    }

    public Vector3 CalculateDestination()
    {
        return position + velocity;
    }
    
    public Vector3 GetPosition()
    {
        return position;
    }
}
