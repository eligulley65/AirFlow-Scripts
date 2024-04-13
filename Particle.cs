using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Particle : MonoBehaviour
{
    private Node currentNode;
    private int numOfHops;
    private LineRenderer lineRendererPrefab;
    private Vector3 start;

    public void StartSchmoovin()
    {
        //Get a list of all nodes
        List<Node> nodes = FindObjectsOfType<Node>().ToList<Node>();
        LineRenderer line = Instantiate(lineRendererPrefab, transform);
        line.positionCount = 1;
        line.SetPosition(0, currentNode.GetPosition());
        for (int i = 0; i < numOfHops; i++)
        {
            Vector3 position = currentNode.GetPosition();
            Vector3 velocity = currentNode.GetVelocity();
            float minDistance = float.MaxValue;
            Node nextNode = null;
            //currentNode.GetComponent<MeshRenderer>().enabled = true;
            foreach(Node node in nodes)
            {
                if (currentNode == node) { continue; }
                if (Mathf.Abs(Vector3.Distance(position, node.GetPosition())) >= minDistance){ continue; }
                if (node.GetVelocity() == Vector3.zero){ continue; }
                
                Vector3 positionToCheck = node.GetPosition();
                if (CanWeGetThere(position.x, positionToCheck.x, velocity.x) &&
                    CanWeGetThere(position.y, positionToCheck.y, velocity.y) &&
                    CanWeGetThere(position.z, positionToCheck.z, velocity.z))
                {
                    float xTime = IntersectionOnPlane(position.x, velocity.x, positionToCheck.x);
                    float yTime = IntersectionOnPlane(position.y, velocity.y, positionToCheck.y);
                    float zTime = IntersectionOnPlane(position.z, velocity.z, positionToCheck.z);
                    float time = Mathf.Min(xTime, yTime, zTime);
                    if (node.IsPointInside(position + velocity*time))
                    {
                        //Debug.Log("Start: " + currentNode.GetPosition() + " End: " + node.GetPosition());
                        nextNode = node;
                        minDistance = Mathf.Abs(Vector3.Distance(position, node.GetPosition()));
                    }
                    //Debug.Log(position + velocity*time);
                }
            }
            if (nextNode)
            {
                //Debug.Log(position + " to " + nextNode.GetPosition());
                line.positionCount++;
                line.SetPosition(i+1, nextNode.GetPosition());
                nodes.Remove(currentNode);
                currentNode = nextNode;
            }
            else
            {
                //Debug.Log("dies to removal: " + position + " " + i);
                if (i > 50)
                {
                    Debug.Log("Got it! : " + start + " died at " + i);
                }
                else
                {
                    line.enabled = false;
                }
                break;
            }
        }
    }

    private bool CanWeGetThere(float start, float end, float vel)
    {
        if (start == end){ return false; }
        if (start > end)
        {
            if (vel > 0){ return false; }
        }
        else
        {
            if (vel < 0){ return false; }
        }
        if (vel == 0) { return false; }
        return true;
    }

    private float IntersectionOnPlane(float start, float vel, float pLocation)
    {
        return (pLocation - start) / vel;
    }

    public void SetNode(Node node){ 
        currentNode = node; 
        start = currentNode.GetPosition();
        }
    public void SetNumOfHops(int num){ numOfHops = num; }
    public void SetLine(LineRenderer line)
    {
        lineRendererPrefab = line;
    }
}
