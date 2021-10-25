using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedTrack : MonoBehaviour
{
    public Vector3[] points;



    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshF = GetComponent<MeshFilter>();

        Mesh cloneMesh = new Mesh();



        for(int i = 0; i < 11; i++){

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 FindPoint(Vector3[] pts, float t){

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
