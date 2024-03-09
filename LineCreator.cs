using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField] ReadCSV readCSV;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] LineRenderer lineRendererPrefab;
    List<Node> nodes;

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
            Node node = Instantiate(nodePrefab, points[i], Quaternion.identity).GetComponent<Node>();
            node.GetComponent<Node>().InsertDataInMeDaddy(points[i], vels[i]);
            nodes.Add(node);
        }
        GenerateLines();
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
}
