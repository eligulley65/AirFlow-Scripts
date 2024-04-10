using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ReadCSV : MonoBehaviour
{
    public event EventHandler finishedParsing;
    List<Vector3> points;
    List<Vector3> vels;

    private void Start() {
        points = new List<Vector3>();
        vels = new List<Vector3>();
        ReadCSVFile(2);
        finishedParsing?.Invoke(this, EventArgs.Empty);
    }

    private List<StreamReader> InitializeFiles()
    {
        List<StreamReader> streams = new List<StreamReader>();
        string[] streamStrings = Directory.GetFiles("Assets/CSV Files/Current", "*.csv", SearchOption.AllDirectories);
        foreach (string path in streamStrings)
        {
            streams.Add(new StreamReader(path));
        }
        return streams;
    }

    void ReadCSVFile(int fillerLines)
    {
        List<StreamReader> files = InitializeFiles();

        for (int i = 0; i < files.Count; i++)
        {
            bool eof = false;
            for (int x = 0; x < fillerLines; x++) { files[i].ReadLine(); }
            int counter = 0;
            while(!eof)
            {
                string data_String = files[i].ReadLine();
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
        
    }

    private Vector3 ParseVector3(string[] values, int low)
    {
        float x = (float)Double.Parse(values[low], System.Globalization.NumberStyles.Float);
        float y = (float)Double.Parse(values[low + 2], System.Globalization.NumberStyles.Float);
        float z = (float)Double.Parse(values[low + 1], System.Globalization.NumberStyles.Float);
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
