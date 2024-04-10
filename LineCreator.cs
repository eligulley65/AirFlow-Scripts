using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField] ReadCSV readCSV;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] LineRenderer lineRendererPrefab;
    [SerializeField] Transform nodeParent;
    [SerializeField] ParticleManager particleManager;
    List<Node> nodes;

    public event EventHandler NodesCreated;

    // Start is called before the first frame update
    void Start()
    {
        readCSV.finishedParsing += ReadCSV_FinishedParsing;
    }

    private void ReadCSV_FinishedParsing(object sender, EventArgs e)
    {
        List<Vector3> points = readCSV.GetPoints();
        List<Vector3> vels = readCSV.GetVels();
        nodes = new List<Node>();
        for (int i = 0; i < points.Count; i++)
        {
            Node node = Instantiate(nodePrefab, nodeParent).GetComponent<Node>();
            node.transform.position = points[i];
            node.GetComponent<Node>().Initialize(points[i], vels[i]);
            nodes.Add(node);
        }
        //GenerateLines();
        Debug.Log("please create particles");
        //NodesCreated?.Invoke(this, EventArgs.Empty);
        particleManager.CreateParticles();
    }

    private void GenerateLines()
    {
        foreach(Node node in nodes)
        {
            LineRenderer line = Instantiate(lineRendererPrefab, node.transform);
            line.positionCount = 2;
            line.SetPosition(0, node.GetPosition());
            line.SetPosition(1, node.CalculateDestination());
        }
    }

    public List<Node> GetNodes()
    {
        return nodes;
    }
}
