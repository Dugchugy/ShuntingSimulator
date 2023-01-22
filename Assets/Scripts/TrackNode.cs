using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackNode
{
    //stores the points that make up the track segment
    public vector3[] TrackPoints;

    //stores refernces to the next and previous nodes on the track
    public TrackNode Next;
    public TrackNode Prev;

    //a float to store the length of the track segment
    public float length;

    //an int to determine how many repitions to do when calculating length
    public const int PERECISION = 50;

    //creates a new track segment with the specifed points array
    public TrackNode(Vector3[] TPs){
        //sets the track points
        TrackPoint = TP;

        //updates the length
        UpdateLength(PERECISION);
    }

    //updates the length of the track segment to reflect the current trackpoints array
    public void UpdateLength(int reps){
        //creates a varaible to store the length
        length = 0;

        //uses the reps variable to subdivide the curve into linear segments
        for(int i = 1; i < reps; i++){
            //adds the distance between the two points to the length
            length += Vector3.Distance(FindPoint(TrackPoints[j], ((float) (i-1))/reps), FindPoint(points[j], ((float) (i))/reps));
        }
    }

    //uses the points array to find a point on the track
    public static Vector3 FindPoint(Vector3[] pts, float t){

        //ensures make sure theres always multiple items in the array
        if(pts.Length == 1){
            return(pts[0]);
        }

        //creates an array to store the arrays outputs;
        Vector3[] outpts = new Vector3[pts.Length - 1];

        //loops through the array to excluding the last value and interpolates between the values
        for(int i = 0; i < pts.Length - 1; i++){

            //adds the interpolation between values to the outpts;
            outpts[i] = Vector3.Lerp(pts[i], pts[i + 1], t);
        }

        //finds the points within remaining points
        return(FindPoint(outpts, t));
    }


}
