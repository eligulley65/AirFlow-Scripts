using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ReadCSV : MonoBehaviour
{
    public EventHandler finishedParsing;
    List<Vector3> points;
    List<Vector3> vels;

    private void Start() {
        points = new List<Vector3>();
        vels = new List<Vector3>();
        ReadCSVFile(2);
        finishedParsing?.Invoke(this, EventArgs.Empty);
    }

    void ReadCSVFile(int fillerLines)
    {
        StreamReader strReader = new StreamReader("Assets\\Plane table.csv");
        bool eof = false;
        for (int i = 0; i < fillerLines; i++) { strReader.ReadLine(); }
        int counter = 0;
        while(!eof)
        {
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                eof = true;
                break;
            }
            string[] data_values = data_String.Split(',');
            if (counter >= 999)
            {
                points.Add(ParseVector3(data_values, 2));
                vels.Add(ParseVector3(data_values, 5));
                continue;
            }
            else
            {
                points.Add(ParseVector3(data_values, 1));
                vels.Add(ParseVector3(data_values, 4));
            }
            counter++;
        }
    }

    Vector3 ParseVector3(string[] values, int low)
    {
        float x = (float)Double.Parse(values[low], System.Globalization.NumberStyles.Float);
        float y = (float)Double.Parse(values[low + 1], System.Globalization.NumberStyles.Float);
        float z = (float)Double.Parse(values[low + 2], System.Globalization.NumberStyles.Float);
        return new Vector3 (x, y, z);
    }

    public List<Vector3> GetPoints()
    {
        return points;
    }

    public List<Vector3> GetVels()
    {
        return vels;
    }
}
