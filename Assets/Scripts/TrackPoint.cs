using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPoint
{
    public float dist;
    public Vector3 pos;
    public int TrackID = 0;

    public TrackPoint(float d, Vector3 p)
    {
        dist = d;
        pos = p;
    }


}
