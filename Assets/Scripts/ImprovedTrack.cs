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

        cloneMesh.name = "Track";

        meshF.mesh = cloneMesh;

        for(int i = 0; i < 11; i++){
            CreateRect(FindPoint(points, i / 10.0f), cloneMesh, new Vector3(0.1f, 0.1f, 0.5f));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateRect(Vector3 pos, Mesh Fmesh, Vector3 size){
        for(int i = 0; i < 6; i++){
            //1
            List<Vector3> newVertices = new List<Vector3>(Fmesh.vertices);
            List<Vector2> newUVs = new List<Vector2>(Fmesh.uv);
            List<int> newTriangles = new List<int>(Fmesh.triangles);

            int addval = Fmesh.vertices.Length;

            int xmult = i - 1;

            // corners of quad
            Vector3 vert1 = new Vector3(-.5f, -.5f, 0) + pos;
            Vector3 vert2 = new Vector3(-.5f, .5f, 0) + pos;
            Vector3 vert3 = new Vector3(.5f, .5f, 0) + pos;
            Vector3 vert4 = new Vector3(.5f, -.5f, 0) + pos;

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
