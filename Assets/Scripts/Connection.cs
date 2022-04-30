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

        //double angle = (Math.Atan2(diff.x, diff.z)) * (180 / Math.PI);
        //transform.eulerAngles = new Vector3(-90, (float)(angle), 0);

        transform.LookAt(transform.position + diff);

        //Debug.Log("DeltaTime: " + Time.deltaTime);
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
                //finds the endex of the other point
                int a  = Math.Abs(i - 1);

                //finds the sign of the track distance between points
                float DistSign = Math.Sign(ends[a].dist - ends[i].dist);


                if(Math.Abs(ends[a].dist - ends[i].dist) > 3 * length){
                    DistSign *= -1;
                }

                Debug.Log(DistSign);

                ends[a].dist = ends[i].dist + (length * DistSign);

                ends[a].dist = ensureTrack(ends[a].dist);

                ends[a].pos = track.trackPosition(ends[a].dist);

                prevends[a] = ends[a].pos;
            }

            prevends[i] = ends[i].pos;
        }
    }

    private float ensureTrack(float x){
        if(x > track.Length){
            x -= track.Length;
        }
        if(x < 0){
            x += track.Length;
        }

        return(x);
    }
}
