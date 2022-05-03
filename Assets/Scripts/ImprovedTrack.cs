using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImprovedTrack : MonoBehaviour
{
    [SerializeField] private Material TIEMAT;
    [SerializeField] private Material RAILMAT;

    public const int PERECISION = 50;

    public Vector3[][] points = new Vector3[1][] {new Vector3[] {new Vector3(-10, 0, 0), new Vector3(-5, 0, 0), new Vector3(-5, 0, 5), new Vector3(0, 0, 5)}};

    public float Length = 0;

    //the name of the file to pull the track from
    public string TrackName = "Test";



    // Start is called before the first frame update
    void Start()
    {
        points = TrackLoader.LoadTrack("Tracks/" + TrackName);

        //TrackLoader.LoadTrack("Tracks/Test");

        //reads the current mesh filter
        MeshFilter meshF = GetComponent<MeshFilter>();

        //creates a new mesh to edit
        Mesh cloneMesh = new Mesh();

        //give the new mesh two submeshes
        cloneMesh.subMeshCount = 2;

        //names the mesh track
        cloneMesh.name = "Track";

        //sets the mesh as the current mesh in the mesh filter
        meshF.mesh = cloneMesh;

        //loops for every track section in hte track points array
        for(int j = 0; j < points.Length; j++){

            //estimates the current length of the track segment to the specified PERECISION;
            float Len = readLength(PERECISION, j);

            Length += Len;

            //defines lenth as the length floored to an int
            int lenth = (int) (Len);

            //generates a fractional offset from the start that the ties generate at
            float offset = (Len - lenth) / 2.0f;

            //creates ends for the begining of the rails
            CreateRailEnds(FindPoint(points[j], 0), cloneMesh, new Vector2(0.2f, 0.1f), FindDir(points[j], 0), new Vector2(0.1f, 0.7f));

            //loops through all the required ties
            for(int i = 0; i <= lenth; i++){

                //estimates the t value based on the distance along the curve
                float t = FindTLength(offset + (float) (i), PERECISION, j);

                //estimates the previous t value or assumes it to be zero
                float t2;

                if(i == 0){
                    t2 = 0.0f;
                }else{
                    t2 = FindTLength(offset + (float) (i - 1), PERECISION, j);
                }

                //generates a section of rails for the track
                CreateRailSection(t2, t, cloneMesh, new Vector2(0.2f, 0.1f), new Vector2(0.1f, 0.7f), j);

                //creates a tie under the rails
                CreateRect(FindPoint(points[j], t), cloneMesh, new Vector3(0.1f, 0.1f, 1.0f), FindDir(points[j], t));
            }

            //creates the final segment
            CreateRailSection(FindTLength(offset + lenth, PERECISION, j), 1.0f, cloneMesh, new Vector2(0.2f, 0.1f), new Vector2(0.1f, 0.7f), j);

            //caps off the ends of the rails
            CreateRailEnds(FindPoint(points[j], 1), cloneMesh, new Vector2(0.2f, 0.1f), FindDir(points[j], 1), new Vector2(0.1f, 0.7f));

        }

        MeshRenderer mr = GetComponent<MeshRenderer>();

        mr.materials = new Material[2] {TIEMAT, RAILMAT};
    }

    public Vector3 trackPosition(float pos){

        for(int i = 0; i < points.Length; i++){
            if(readLength(PERECISION, i) > pos){
                float t = FindTLength(pos, PERECISION, i);
                return(FindPoint(points[i], t));
            }

            pos -= readLength(PERECISION, i);
        }

        return(FindPoint(points[points.Length - 1], 1));
    }

    public float ClosestPos(float start, float end, float Distance, int reps)
    {

        float current = start;

        Vector3 startpos = trackPosition(start);

        for(int i = 1; i < reps + 1; i++){

            int mult = 1;

            float currentDist = Vector3.Distance(trackPosition(current), startpos);

            if(currentDist > Distance){// && currentDist < 5 * Distance){
                mult = -1;
            }

            if(start > end && start - (3 * Distance) < end){
                mult *= -1;
            }

            current += (Distance/i) * mult;

            if(current < 0){
                current += Length;
            }else if(current > Length){
                current -= Length;
            }
        }


        /*
        //creates varaibels to store the current distance and closest point
        float dist = 100000;
        Vector3 closest = points[0][0];

        //creates some variables to store the distance from the start of the track
        float trackDist = 0;
        float closestDist = 0;

        Vector3 current = closest;

        Vector3 prev = trackPosition(0);

        for (int i = 0; i < points.Length; i++)
        {
            for(int j = 0; j < reps; j++)
            {
                current = FindPoint(points[i], ((float) j) / reps);

                if(Vector3.Distance(current, p) < dist){
                    closest = current;
                    dist = Vector3.Distance(current, p);

                    closestDist = trackDist;
                }

                trackDist += Vector3.Distance(prev, current);

                prev = current;
            }
        }
        */

        return (current);
    }

    void CreateRailEnds(Vector3 pos, Mesh Fmesh, Vector2 size, Vector3 x, Vector2 offset){
        //defines y as the vertical direction (positive up)
        Vector3 y = Vector3.up;

        //finds z based off of x and y
        Vector3 z = Vector3.Cross(x, y).normalized;

        for(int i = 0; i < 2; i++){
            //1
            List<Vector3> newVertices = new List<Vector3>(Fmesh.vertices);
            List<Vector2> newUVs = new List<Vector2>(Fmesh.uv);
            List<int> newTriangles = new List<int>(Fmesh.GetTriangles(1));

            int addval = Fmesh.vertices.Length;

            Vector3 vert1 = new Vector3(0, 0, 0);
            Vector3 vert2 = new Vector3(0, 0, 0);
            Vector3 vert3 = new Vector3(0, 0, 0);
            Vector3 vert4 = new Vector3(0, 0, 0);

            switch(i){
            case 0:
                vert1 = y * size.x + y * offset.x + z * size.y + z * offset.y + pos;
                vert2 = y * offset.x + z * size.y + z * offset.y + pos;
                vert3 = y * offset.x - z * size.y + z * offset.y + pos;
                vert4 = y * size.x + y * offset.x - z * size.y + z * offset.y + pos;
                break;
            case 1:
                vert1 = y * size.x + y * offset.x + z * size.y - z * offset.y + pos;
                vert2 = y * offset.x + z * size.y - z * offset.y + pos;
                vert3 = y * offset.x - z * size.y - z * offset.y + pos;
                vert4 = y * size.x + y * offset.x - z * size.y - z * offset.y + pos;
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
            Fmesh.SetTriangles(newTriangles.ToArray(), 1);
        }
    }

    void CreateRailSection(float t1, float t2, Mesh Fmesh, Vector2 size, Vector2 offset, int j){

        Vector3 pos1 = FindPoint(points[j], t1);
        Vector3 x1 = FindDir(points[j], t1);

        Vector3 pos2 = FindPoint(points[j], t2);
        Vector3 x2 = FindDir(points[j], t2);

        Vector3 y = Vector3.up;

        Vector3 z1 = Vector3.Cross(x1, y).normalized;
        Vector3 z2 = Vector3.Cross(x2, y).normalized;

        for(int i = 0; i < 8; i++){
            //1
            List<Vector3> newVertices = new List<Vector3>(Fmesh.vertices);
            List<Vector2> newUVs = new List<Vector2>(Fmesh.uv);
            List<int> newTriangles = new List<int>(Fmesh.GetTriangles(1));

            int addval = Fmesh.vertices.Length;

            Vector3 vert1 = new Vector3(0, 0, 0);
            Vector3 vert2 = new Vector3(0, 0, 0);
            Vector3 vert3 = new Vector3(0, 0, 0);
            Vector3 vert4 = new Vector3(0, 0, 0);

            switch(i){
             //creates squares for each side of the rectangualr prism

            case 0:
            //creates a square for the "front" of the prism

                vert4 = y * offset.x + z1 * size.y + z1 * offset.y + pos1;
                vert3 = y * offset.x + z2 * size.y + z2 * offset.y + pos2;
                vert2 = y * size.x + y * offset.x + z2 * size.y + z2 * offset.y + pos2;
                vert1 = y * size.x + y * offset.x + z1 * size.y + z1 * offset.y + pos1;
                break;


            case 1:
                vert1 = y * offset.x - z1 * size.y + z1 * offset.y + pos1;
                vert2 = y * offset.x - z2 * size.y + z2 * offset.y + pos2;
                vert3 = y * size.x + y * offset.x - z2 * size.y + z2 * offset.y + pos2;
                vert4 = y * size.x + y * offset.x - z1 * size.y + z1 * offset.y + pos1;
                break;
            case 2:
                vert4 = y * size.x + y * offset.x + z1 * size.y + z1 * offset.y + pos1;
                vert3 = y * size.x + y * offset.x + z2 * size.y + z2 * offset.y + pos2;
                vert2 = y * size.x + y * offset.x - z2 * size.y + z2 * offset.y + pos2;
                vert1 = y * size.x + y * offset.x - z1 * size.y + z1 * offset.y + pos1;
                break;
            case 3:
                vert1 = y * offset.x + z1 * size.y + z1 * offset.y + pos1;
                vert2 = y * offset.x + z2 * size.y + z2 * offset.y + pos2;
                vert3 = y * offset.x - z2 * size.y + z2 * offset.y + pos2;
                vert4 = y * offset.x - z1 * size.y + z1 * offset.y + pos1;
                break;

            case 4:
            //creates a square for the "front" of the prism

                vert4 = y * offset.x + z1 * size.y - z1 * offset.y + pos1;
                vert3 = y * offset.x + z2 * size.y - z2 * offset.y + pos2;
                vert2 = y * size.x + y * offset.x + z2 * size.y - z2 * offset.y + pos2;
                vert1 = y * size.x + y * offset.x + z1 * size.y - z1 * offset.y + pos1;
                break;

            case 5:
                vert1 = y * offset.x - z1 * size.y - z1 * offset.y + pos1;
                vert2 = y * offset.x - z2 * size.y - z2 * offset.y + pos2;
                vert3 = y * size.x + y * offset.x - z2 * size.y - z2 * offset.y + pos2;
                vert4 = y * size.x + y * offset.x - z1 * size.y - z1 * offset.y + pos1;
                break;
            case 6:
                vert4 = y * size.x + y * offset.x + z1 * size.y - z1 * offset.y + pos1;
                vert3 = y * size.x + y * offset.x + z2 * size.y - z2 * offset.y + pos2;
                vert2 = y * size.x + y * offset.x - z2 * size.y - z2 * offset.y + pos2;
                vert1 = y * size.x + y * offset.x - z1 * size.y - z1 * offset.y + pos1;
                break;
            case 7:
                vert1 = y * offset.x + z1 * size.y - z1 * offset.y + pos1;
                vert2 = y * offset.x + z2 * size.y - z2 * offset.y + pos2;
                vert3 = y * offset.x - z2 * size.y - z2 * offset.y + pos2;
                vert4 = y * offset.x - z1 * size.y - z1 * offset.y + pos1;
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
            Fmesh.SetTriangles(newTriangles.ToArray(), 1);
        }

    }

    void CreateRect(Vector3 pos, Mesh Fmesh, Vector3 size, Vector3 x){

        //defines y as the vertical direction (positive up)
        Vector3 y = Vector3.up;

        //finds z based off of x and y
        Vector3 z = Vector3.Cross(x, y).normalized;

        for(int i = 0; i < 6; i++){
            //1
            List<Vector3> newVertices = new List<Vector3>(Fmesh.vertices);
            List<Vector2> newUVs = new List<Vector2>(Fmesh.uv);
            List<int> newTriangles = new List<int>(Fmesh.GetTriangles(0));

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
            Fmesh.SetTriangles(newTriangles.ToArray(), 0);
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

    public float readLength(int reps, int j){
        //creates a varaible to store the length
        float length = 0;

        //uses the reps variable to subdivide the curve into linear segments
        for(int i = 1; i < reps; i++){
            //adds the distance between the two points to the length
            length += Vector3.Distance(FindPoint(points[j], ((float) (i-1))/reps), FindPoint(points[j], ((float) (i))/reps));
        }

        //returns the length value
        return length;
    }

    public float FindTLength(float dist, int reps, int j){
        //creates a variable to store the Length
        float length = 0;

        //uses the reps varaible to subdivide the curve into linear segments
        for(int i = 1; i < reps; i++){
            //stores the distance between the two points
            float segDist = Vector3.Distance(FindPoint(points[j], ((float) (i-1))/reps), FindPoint(points[j], ((float) (i))/reps));

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
