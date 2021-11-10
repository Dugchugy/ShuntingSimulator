using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackLoader
{
    public static Vector3[][] LoadTrack(string filename){
        TextAsset text = Resources.Load<TextAsset>(filename);

        string file = text.ToString();

        string[] lines = file.Split('\n');

        Debug.Log(lines.Length);

        List<Vector3[]> Vectors = new List<Vector3[]>();

        for(int i = 0; i < lines.Length - 1; i++){
            List<Vector3> curve = new List<Vector3>();

            Debug.Log(lines[i][2]);

            int axis = 0;
            string current = "";


            float f = 0;

            Vector3 currentVect = Vector3.zero;

            for(int j = 0; j < lines[i].Length; j++){
                switch(lines[i][j]){
                case '[':
                    break;
                case ',':
                    Debug.Log("parseing: " + current);
                    Single.TryParse(current, out f);
                    currentVect[axis] = f;
                    axis++;
                    current = "";
                    break;
                case ']':
                    Single.TryParse(current, out f);
                    currentVect[axis] = f;
                    axis = 0;
                    curve.Add(currentVect);
                    currentVect = Vector3.zero;
                    current = "";
                    break;
                default:
                    current = current + lines[i][j];
                    break;
                }
            }

            Vectors.Add(curve.ToArray());
        }

        Vector3[][] vects = Vectors.ToArray();

        for(int i  = 0; i < vects.Length; i++){
            for(int j = 0; j < vects[i].Length; j++){
                Debug.Log(vects[i][j]);
            }
        }

        return(vects);
    }
}
