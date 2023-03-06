using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPoint
{
    public float dist;
    public Vector3 pos;
    public TrackNode curNode;

    public TrackPoint(float d, Vector3 p, TrackNode c)
    {
        dist = d;
        pos = p;
        curNode = c;
    }


}
