using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImprovedTrack : MonoBehaviour
{
    public const int PERECISION = 100;

    public Vector3[] points;

    public float Length;



    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshF = GetComponent<MeshFilter>();

        Mesh cloneMesh = new Mesh();

        Length = readLength(PERECISION);

        cloneMesh.name = "Track";

        meshF.mesh = cloneMesh;

        int lenth = (int) (Length);

        for(int i = 0; i < lenth; i++){
            float t = FindTLength((float) (i), PERECISION);

            CreateRect(FindPoint(points, t), cloneMesh, new Vector3(0.1f, 0.1f, 0.5f), FindDir(points, t));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateRect(Vector3 pos, Mesh Fmesh, Vector3 size, Vector3 x){

        //defines y as the vertical direction (positive up)
        Vector3 y = Vector3.up;

        //finds z based off of x and y
        Vector3 z = Vector3.Cross(x, y).normalized;

        //checks the values of x, y and z:
        Debug.LogFormat("x: {0}, y: {1}, z: {2}", x, y, z);

        for(int i = 0; i < 6; i++){
            //1
            List<Vector3> newVertices = new List<Vector3>(Fmesh.vertices);
            List<Vector2> newUVs = new List<Vector2>(Fmesh.uv);
            List<int> newTriangles = new List<int>(Fmesh.triangles);

            int addval = Fmesh.vertices.Length;

            Vector3 vert1 = new Vector3(0, 0, 0);
            Vector3 vert2 = new Vector3(0, 0, 0);
            Vector3 vert3 = new Vector3(0, 0, 0);
            Vector3 vert4 = new Vector3(0, 0, 0);

            switch(i){
             //creates squares for each side of the rectangualr prism

            case 0:
            //creates a square for the "front" of the prism
                vert1 = x *-1 * size.x + y * -1 * size.y + z * size.z + pos;
                vert2 = x * -1 * size.x + y * size.y + z * size.z + pos;
                vert3 = x * size.x + y * size.y + z * size.z + pos;
                vert4 = x * size.x + y * -1 * size.y + z * size.z + pos;
                break;

            case 1:
                vert4 = x * -1 * size.x + y * -1 * size.y + z * -1 * size.z + pos;
                vert3 = x * -1 * size.x + y * size.y + z * -1 * size.z + pos;
                vert2 = x * size.x + y * size.y + z * -1 * size.z + pos;
                vert1 = x * size.x + y * -1 * size.y + z * -1 * size.z + pos;
                break;

            case 2:
                vert4 = x * -1 * size.x + y * -1 * size.y + z * size.z + pos;
                vert3 = x * -1 * size.x + y * size.y + z * size.z + pos;
                vert2 = x * -1 * size.x + y * size.y + z * -1 * size.z + pos;
                vert1 = x * -1 * size.x + y * -1 * size.y + z * -1 * size.z + pos;
                break;

            case 3:
                vert1 = x * size.x + y * -1 * size.y + z * size.z + pos;
                vert2 = x * size.x + y * size.y + z * size.z + pos;
                vert3 = x * size.x + y * size.y + z * -1 * size.z + pos;
                vert4 = x * size.x + y * -1 * size.y + z * -1 * size.z + pos;
                break;

            case 4:
                vert4 = x * size.x + y * -1 * size.y + z * size.z + pos;
                vert3 = x * -1 * size.x + y * -1 * size.y + z * size.z + pos;
                vert2 = x * -1 * size.x + y * -1 * size.y + z * -1 * size.z + pos;
                vert1 = x * size.x + y * -1 * size.y + z * -1 * size.z + pos;
                break;

            case 5:
                vert1 = x * size.x + y * size.y + z * size.z + pos;
                vert2 = x * -1 * size.x + y * size.y + z * size.z + pos;
                vert3 = x * -1 * size.x + y * size.y + z * -1 * size.z + pos;
                vert4 = x * size.x + y * size.y + z * -1 * size.z + pos;
                break;

            }

            //2
            newVertices.Add(vert1);
            newVertices.Add(vert2);
            newVertices.Add(vert3);
            newVertices.Add(vert4);

            //3
            newUVs.Add(new Vector2(1, 0));
            newUVs.Add(new Vector2(1, 1));
            newUVs.Add(new Vector2(0, 1));
            newUVs.Add(new Vector2(0, 0));

            //4
            newTriangles.Add(2 + addval);
            newTriangles.Add(1 + addval);
            newTriangles.Add(0 + addval);

            //5
            newTriangles.Add(3 + addval);
            newTriangles.Add(2 + addval);
            newTriangles.Add(0 + addval);

            Fmesh.vertices = newVertices.ToArray();
            Fmesh.uv = newUVs.ToArray();
            Fmesh.triangles = newTriangles.ToArray();
        }
    }

    public static Vector3 FindDir(Vector3[] pts, float t){
        switch(pts.Length){
            case 2:
                return((pts[1] - pts[0]).normalized);
            case 4:
                Vector3 retvect = new Vector3(0, 0, 0);
                retvect += pts[0] * (-3 * t * t + 6 * t - 3);
                retvect += pts[1] * (9 * t * t - 12 * t + 3);
                retvect += pts[2] * (-9 * t * t + 6 * t);
                retvect += pts[3] * (3 * t * t);
                return(retvect.normalized);
        }

        return(new Vector3(0, 0, 0));
    }

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

    public float readLength(int reps){
        //creates a varaible to store the length
        float length = 0;

        //uses the reps variable to subdivide the curve into linear segments
        for(int i = 1; i < reps; i++){
            //adds the distance between the two points to the length
            length += Vector3.Distance(FindPoint(points, ((float) (i-1))/reps), FindPoint(points, ((float) (i))/reps));
        }

        //returns the length value
        return length;
    }

    public float FindTLength(float dist, int reps){
        //creates a variable to store the Length
        float length = 0;

        //uses the reps varaible to subdivide the curve into linear segments
        for(int i = 1; i < reps; i++){
            //stores the distance between the two points
            float segDist = Vector3.Distance(FindPoint(points, ((float) (i-1))/reps), FindPoint(points, ((float) (i))/reps));

            //adds  the distance to the current Length
            length += segDist;

            //checks if the current length of the arc is longer than the desired Distance
            if(length > dist){
                //calculated the difference betweent he used t values
                float Tdist = (((float) (i))/reps) - (((float) i - 1)/reps);

                float desT = Tdist * ((dist - (length - segDist))/segDist);

                desT += (((float) i - 1)/reps);

                return(desT);

            }
        }

        return(1.0f);
    }
}
