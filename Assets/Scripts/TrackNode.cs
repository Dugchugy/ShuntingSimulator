using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackNode
{
    //stores the points that make up the track segment
    public Vector3[] points;

    //stores refernces to the next and previous nodes on the track
    public TrackNode Next;
    public TrackNode Prev;

    //a float to store the length of the track segment
    public float length;

    //an int to determine how many repitions to do when calculating length
    public const int PERECISION = 100;

    //creates a new track segment with the specifed points array
    public TrackNode(Vector3[] TPs){
        //sets the track points
        points = TPs;

        //sets next and prev to null (no links)
        Next = null;
        Prev = null;

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
            length += Vector3.Distance(FindPoint(points, ((float) (i-1))/reps), FindPoint(points, ((float) (i))/reps));
        }
    }

    //fidns the point on the specifed curve using the t value
    public Vector3 findPointT(float t){
        //calls the static findPoint with the points array and the t value
        return FindPoint(points, t);
    }

    //finds the vector3 position associated with the distance along the track provided
    public Vector3 FindPoint(float l){
        //returns the position along the track given by the t value found by divding the distance by length
        return FindPoint(points, findT(l));
    }

    public float findT(float l){

        //creates a varaible to stor the previous vector
        Vector3 prePoint = findPointT(0);

        //creates a vari8able tos store the current vector
        Vector3 curPoint = prePoint;

        for(float t = 1.0f/PERECISION; t < 1.0f + (1.0f/PERECISION); t += 1.0f/PERECISION){
            //finds the current point
            curPoint = findPointT(t);

            //finds the distance between the current point and the previous point
            float dist = Vector3.Distance(curPoint, prePoint);

            if(dist > l){
                
                return ((t - (1/PERECISION)) + ((l/dist) * (1.0f/PERECISION)));
            }else{
                l = l - dist;

            }

            //sets the prevPoint to the current point (for the next cycle)
            prePoint = curPoint;
        }

        Debug.Log("full output");

        return 1;
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
