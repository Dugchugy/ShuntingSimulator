using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TrackLoader
{

    public static TrackNode[] LoadedNodes;

    public static TrackNode LoadTrack(string filename){
        //loads the track text from the game resouces
        TextAsset text = Resources.Load<TextAsset>(filename);

        //converts the txt to a string for processing
        string file = text.ToString();

        //splits the string on the newline chars (one string per line)
        string[] lines = file.Split('\n');

        //checks if no lines were read
        if(lines.Length < 1){
            //returns null
            return null;
        }

        //creates varaibles to store the links for the track objects
        int[] PrevIndexs = new int[lines.Length];
        int[] NextIndexs = new int[lines.Length];

        //initalizes the list of track nodes
        LoadedNodes = new TrackNode[lines.Length];

        //creates loops through each line read
        for(int i = 0; i < lines.Length; i++){
            if(lines[i] != ""){
                //splits the lines by the | char
                string[] parts = lines[i].Split('|');

                Debug.Log(parts[0]);

                //reads the first and last items in the parts list (numbers to determine where this track piece links)
                PrevIndexs[i] = int.Parse(parts[0]);
                NextIndexs[i] = int.Parse(parts[parts.Length - 1]);

                //creates a array to temporarily store the vectors for the bezier curves
                Vector3[] bezPoints = new Vector3[parts.Length - 2]; 

                //loops through all but the first and last points in the curve specifcations
                for(int j = 1; j < parts.Length - 1; j++){

                    //splits the current section on the comma (so coords can be read x,y,z)
                    string[] coords  = parts[j].Split(',');

                    //reads the coords as floats and writes them to the current point in the curve
                    bezPoints[j - 1].x = float.Parse(coords[0]); //x
                    bezPoints[j - 1].y = float.Parse(coords[1]); //y
                    bezPoints[j - 1].z = float.Parse(coords[2]); //z
                }

                //adds the node read from the file to to the nodes list
                LoadedNodes[i] = new TrackNode(bezPoints);
            }
        }

        //loops through each node again to set its links
        for(int i = 0; i < LoadedNodes.Length; i++){
            //checks that the node set as the prev node is valid
            if(PrevIndexs[i] > 0 && PrevIndexs[i] <= LoadedNodes.Length){
                //sets the current nodes previous node to the specifeid node
                LoadedNodes[i].Prev = LoadedNodes[PrevIndexs[i] - 1];
            }

            //checks that the node set as the next node is valid
            if(NextIndexs[i] > 0 && NextIndexs[i] <= LoadedNodes.Length){
                //sets the current nodes next node to the specifed node
                LoadedNodes[i].Next = LoadedNodes[NextIndexs[i] - 1];
            }
        }

        //returns the first loaded node as the head node
        return LoadedNodes[0];
    }



    /*public static Vector3[][] LoadTrack(string filename){
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
    }*/
}
