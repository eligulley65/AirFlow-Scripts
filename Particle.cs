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

    public void StartSchmoovin()
    {
        /* currently deprecated code
        LineRenderer line = Instantiate(lineRendererPrefab, transform);
        line.positionCount = 1;
        for (int i = 0; i < 5; i++)
        {
            transform.position = currentNode.GetPosition();
            RaycastHit hit;
            if (line.positionCount > i)
            {
                line.SetPosition(i, transform.position);
            }
            Debug.Log(currentNode.GetVelocity().ToString("G") + ", " + transform.position.ToString("G"));
            if (currentNode.GetVelocity() == Vector3.zero)
            {
                Debug.Log("died");
                break;
            }
            if (Physics.Raycast(transform.position, currentNode.GetVelocity(), out hit, Mathf.Infinity, 0))
            {
                Debug.Log("fired");
                if (hit.collider.gameObject)
                {
                    Debug.Log("hit something: " + hit.collider.gameObject.name);
                }
                if (hit.collider.gameObject.TryGetComponent<Node>(out Node node))
                {
                    if (node == currentNode){ 
                        Debug.Log("why u hitting urself");
                        continue; }
                    line.positionCount++;
                    currentNode = node;
                    Debug.Log("hit that bitch");
                }
            }
        }
        //line.SetPosition(numOfHops - 1, transform.position);
        */
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
            foreach(Node node in nodes)
            {
                if (currentNode == node) { continue; }
                if (Mathf.Abs(Vector3.Distance(position, node.GetPosition())) >= minDistance){ continue; }
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
                Debug.Log(position + " to " + nextNode.GetPosition());
                line.positionCount++;
                line.SetPosition(i+1, nextNode.GetPosition());
                currentNode = nextNode;
            }
            else
            {
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

    public void SetNode(Node node){ currentNode = node; }
    public void SetNumOfHops(int num){ numOfHops = num; }
    public void SetLine(LineRenderer line)
    {
        lineRendererPrefab = line;
    }
}
