using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] Particle particlePrefab;
    [SerializeField] LineRenderer lineRendererPrefab;
    [SerializeField] Transform particleParent;
    [SerializeField] LineCreator lineCreator;
    private Vector3[] intakeCorners = {new Vector3(2.117143f, 0.3610001f, -0.048913f), new Vector3(2.3f, 0.3610001f, -0.048913f),
        new Vector3(2.11656f, 0.0556001f, -0.041304365f), new Vector3(2.330573f, 0.0556001f, -0.04130435f),
        new Vector3(2.3f, 0.05560013f, -0.2847826f), new Vector3(2.11656f, 0.05560013f, -0.2847826f),
        new Vector3(2.117143f, 0.3610001f, -0.2771738f), new Vector3(2.3f, 0.3610001f, -0.2771738f)};
    private Vector3[] intakeCorners2 = {new Vector3 (4.646666f, 2.5f, 2.3092308f), new Vector3 (4.646666f, 2.5f, 2.661539f), 
        new Vector3 (4.433333f, 2.5f, 2.3092308f), new Vector3 (4.433333f, 2.5f, 2.661539f)};

    float[] maxes = {float.MinValue, float.MinValue, float.MinValue};
    float[] mins = {float.MaxValue, float.MaxValue, float.MaxValue};

    private void Start() 
    {
        lineCreator.NodesCreated += LineCreator_NodesCreated;
    }

    private void LineCreator_NodesCreated(object sender, EventArgs e)
    {
        Debug.Log("particles");
        //CreateParticles();
    }

    public void CreateParticles()
    {
        FindIntakeMaxesAndMins();
        List<Node> nodes = lineCreator.GetNodes();
        int numOfParticles = nodes.Count;
        for (int i = 0; i < 500; i++)
        {
            if (!IsInIntakeBox(nodes[i].GetPosition()))
            {
                //continue;
            }
            ///*
            System.Random rand = new System.Random();
            int nodeIndex = rand.Next(0, nodes.Count - 1);
            //int nodeIndex = i;
            Particle particle = Instantiate(particlePrefab, particleParent).GetComponent<Particle>();
            particle.SetNode(nodes[nodeIndex]);
            particle.SetNumOfHops(200);
            particle.SetLine(lineRendererPrefab);
            particle.StartSchmoovin();
            //*/
        }
    }

    private void FindIntakeMaxesAndMins()
    {
        /*
        for (int i = 0; i < intakeCorners.Length; i++)
        {
            CheckAndSet(intakeCorners[i].x, 0);
            CheckAndSet(intakeCorners[i].y, 1);
            CheckAndSet(intakeCorners[i].z, 2);
        }
        //*/
        ///*
        for (int i = 0; i < intakeCorners2.Length; i++)
        {
            CheckAndSet(intakeCorners2[i].x, 0);
            CheckAndSet(intakeCorners2[i].y, 1);
            CheckAndSet(intakeCorners2[i].z, 2);
        }
        //*/
        for (int i = 0; i < mins.Length; i++)
        {
            mins[i] -= 0.2f;
            maxes[i] += 0.2f;
        }
    }

    private void CheckAndSet(float value, int pos)
    {
        if (value < mins[pos])
        {
            mins[pos] = value;
        }
        else if (value > maxes[pos])
        {
            maxes[pos] = value;
        }
    }

    private bool IsInIntakeBox(Vector3 pointToCheck)
    {
        if (pointToCheck.x < mins[0] || pointToCheck.x > maxes[0]){ return false; }
        if (pointToCheck.y < mins[1] || pointToCheck.y > maxes[1]){ return false; }
        if (pointToCheck.z < mins[2] || pointToCheck.z > maxes[2]){ return false; }

        return true;
    }
}
