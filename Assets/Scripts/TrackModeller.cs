using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrackModeller
{

    public static int[][] VERTEX_TRANGLE_INDEXS = new int[][] {
        new int[] {0, 1, 3}, 
        new int[] {0, 2, 3}, 
        new int[] {0, 2, 6},
        new int[] {0, 4, 6},
        new int[] {0, 1, 5},
        new int[] {0, 4, 5},
        new int[] {7, 5, 1},
        new int[] {7, 3, 1},
        new int[] {7, 5, 4},
        new int[] {7, 6, 4},
        new int[] {7, 3, 2},
        new int[] {7, 6, 2}
    };

    //generates a model for the specifed node and adds it to the provided mesh
    public static bool modelNode(Mesh m, TrackNode node){
        return true;
    }

    public static void CreateRect(Vector3 pos, Mesh fmesh, Vector3 size, Vector3 x, int SM){

        //defines y as the vertical direction (positive up)
        Vector3 y = Vector3.up;

        //finds z based off of x and y
        Vector3 z = Vector3.Cross(x, y).normalized;

        //creates an array to store the vertexs of the rectange
        int[] vertIndexs = new int[8];

        //creates list for all the vertexs
        List<Vector3> verts = new List<Vector3>();
        //reads the vertex list from the mesh
        fmesh.GetVertices(verts);
        //creates a list for all the UVs
        List<Vector2> UVs = new List<Vector2>();
        //reads the uv list from the mesh
        fmesh.GetUVs(0, UVs);
        //creates a list for all the triangles
        List<int> Triangles = new List<int>();
        //reads the triangles list from the mesh
        fmesh.GetTriangles(Triangles, SM);

        //creates an int array to control whether or not to multiply the number by -1
        int[] neg = new int[] {1,1,1};

        //loops for each vertex of the prism
        for(int i = 0; i < 8; i++){

            //finds the position of the current vertex
            Vector3 curVert = pos + (x * neg[0] * size.x) + (y * neg[1] * size.y) + (z * neg[2] * size.z);

            //searchs the vertex list to see if this vertex already exists (if so, its index will be recorded)
            vertIndexs[i] = verts.FindIndex(x => x == curVert);

            //checks if the list doesn't contain a vertex that matches the new one
            if(vertIndexs[i] == -1){
                //sets verIndexs to the index after the last in the list
                vertIndexs[i] = verts.Count;

                //adds the new vertex to the end of the list
                verts.Add(curVert);

                UVs.Add(new Vector2(1,1));
            }

            //inverts neg 0
            neg[0] *= -1;

            //if neg[0] has been inverted twice, invert neg[1]
            if(neg[0] > 0){
                neg[1] *= -1;

                //if  neg[1] has been inverted twice, invert neg[2]
                if(neg[1] > 0){
                    neg[2] *= -1;
                }
            }
        }

        //now that all the vertexs have been found/generated the 12 triangles can be specifed
        for(int i = 0; i < 12; i++){
            for(int j = 0; i < 3; j++){
                Triangles.Add(vertIndexs[VERTEX_TRANGLE_INDEXS[i][j]]);
            }
        }

        //rectange added to model
    }

    //models a rectangular prism at the given point with the given size oriented with x as the forwards direction and y as up
    public static void CreateRect(Vector3 pos, Mesh Fmesh, Vector3 size, Vector3 x){

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
    
}
