using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Connection : MonoBehaviour
{
    //a variable to sture the length of the item
    public float length;

    public TrackPoint[] ends;

    public Vector3[] prevends;

    public ImprovedTrack track;

    // Start is called before the first frame update
    void Start()
    {
        //finds the current track manager
        track = GameObject.Find("TrackSegment").GetComponent<ImprovedTrack>();

        ends = new TrackPoint[2];
        prevends = new Vector3[2];

        for(int i = 0; i < 2; i++)
        {
            ends[i] = new TrackPoint(0, transform.position);
            prevends[i] = ends[i].pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //updates  the position of the cart
        transform.position = (ends[0].pos + ends[1].pos) / 2;


        Vector3 diff = ends[0].pos - ends[1].pos;

        //double angle = (Math.Atan2(diff.z, diff.x)) * (180 / Math.PI);
        //transform.eulerAngles = new Vector3(-90, (float)(angle), 0);

        transform.LookAt(transform.position + diff);

        Debug.Log("DeltaTime: " + Time.deltaTime);
    }

    void FixedUpdate()
    {

        for(int i = 0; i < 2; i++)
        {
            ends[i].pos = track.trackPosition(ends[i].dist);
        }

        for (int i = 0; i < 2; i++){
            if (ends[i].pos != prevends[i])
            {
                int a  = Math.Abs(i - 1);

                ends[a].dist = track.ClosestPos(ends[i].dist, ends[a].dist, length, 10);
                ends[a].pos = track.trackPosition(ends[a].dist);

                prevends[a] = ends[a].pos;
            }

            prevends[i] = ends[i].pos;
        }




    }
}
