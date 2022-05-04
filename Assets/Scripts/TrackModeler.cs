using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrackModeler
{
    static void ModelTrack(Vector3[][] points, string TrackName){

        //creates a new mesh to edit
        Mesh cloneMesh = new Mesh();

        //give the new mesh two submeshes
        cloneMesh.subMeshCount = 2;

        //names the mesh track
        cloneMesh.name = TrackName;
    }
}
