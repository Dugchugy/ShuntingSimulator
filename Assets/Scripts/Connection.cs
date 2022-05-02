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

    public Vector3 offset = new Vector3();

    public GameObject[] connected = new GameObject[2];

    public int[] connectIndex = new int[2];
    private Connection[] ConnectConnect = new Connection[2];

    public float[] startDist = new float[2];

    // Start is called before the first frame update
    void Start()
    {
        //finds the current track manager
        track = GameObject.Find("TrackSegment").GetComponent<ImprovedTrack>();

        ends = new TrackPoint[2];
        prevends = new Vector3[2];

        for(int i = 0; i < 2; i++)
        {
            ends[i] = new TrackPoint(startDist[i], transform.position);
            prevends[i] = ends[i].pos;
        }

        for(int i = 0; i < 2; i++){
            if(connected[i] != null){
                ConnectConnect[i] = connected[i].GetComponent<Connection>();
            }else{
                connected[i] = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //updates  the position of the cart
        transform.position = (ends[0].pos + ends[1].pos) / 2;

        transform.position += offset;


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

        UpdateConnections();
    }

    public void UpdateConnections(){

        //loops for each connected index
        for (int i = 0; i < 2; i++){
            //checks if the point has moved
            if (ends[i].pos != prevends[i])
            {
                ends[i].pos = track.trackPosition(ends[i].dist);

                //finds the endex of the other point
                int a  = Math.Abs(i - 1);

                //finds the sign of the track distance between points
                float DistSign = Math.Sign(ends[a].dist - ends[i].dist);

                //checks if it should loop the rest of the ways around the track
                if(Math.Abs(ends[a].dist - ends[i].dist) > 3 * length){
                    DistSign *= -1;
                }

                //moves the other point
                ends[a].dist = ends[i].dist + (length * DistSign);

                //loops the point if it has gone off the track
                ends[a].dist = ensureTrack(ends[a].dist);

                //move the pysical point to the new track point
                ends[a].pos = track.trackPosition(ends[a].dist);

                //prevents updating for this movement
                prevends[a] = ends[a].pos;

                if(ConnectConnect[a] != null){
                    ConnectConnect[a].ends[connectIndex[a]].dist = ends[a].dist;

                    ConnectConnect[a].UpdateConnections();
                }
            }

            //prevents updateing again for this movement
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
