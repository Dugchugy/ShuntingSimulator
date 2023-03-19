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

    public TrackManager track;

    public Vector3 offset = new Vector3();

    public bool debugLog = false;

    public GameObject[] connected = new GameObject[2];

    public int[] connectIndex = new int[2];
    public Connection[] ConnectConnect = new Connection[2];

    public float[] startDist = new float[2];

    // Start is called before the first frame update
    void Start()
    {
        //finds the current track manager
        track = GameObject.Find("TrackSegment").GetComponent<TrackManager>();

        ends = new TrackPoint[2];
        prevends = new Vector3[2];

        for(int i = 0; i < 2; i++)
        {
            ends[i] = new TrackPoint(startDist[i], transform.position, track.Head);
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

        //adds the position offset
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
            if(ends[i].curNode == null){
                ends[i].curNode = track.Head;
            }
            ends[i].pos = ends[i].curNode.FindPoint(ends[i].dist);
        }

        //UpdateConnections();
    }

    public void UpdateConnections(){
        if(debugLog){
            Debug.Log("update connections running");
        }

        //checks if the track links haven't been defined and defines them
        for(int i = 0; i < 2; i++){
            if(ends[i].curNode == null){
                ends[i].curNode = track.Head;
            }
        }


        //loops for each connected index
        for (int i = 0; i < 2; i++){
            //checks if the point has moved
            if (ends[i].pos != prevends[i])
            {
                if(debugLog){
                    Debug.Log("movement detected on index " + i);
                    Debug.Log("current pos: " + ends[i].pos);
                    Debug.Log("previous pos: " + prevends[i]);
                }


                //loops until the positions have settled
                bool loops = true;
                while(loops){
                    if(debugLog){
                        Debug.Log("now correcting for index " + i);
                    }
                    //finds the index of the other point
                    int a  = Math.Abs(i - 1);

                    //finds out how far apart the ends are
                    float endDist = TrackManager.findDist(ends[i], ends[a]);

                    //encompasses positive endDist
                    if(endDist > 0){
                        //moves the point so it is in the right place
                        loops = TrackManager.MovePoint(ref ends[a], length - endDist);
                    }else{
                        //moves the point so it is in the right place
                        loops = TrackManager.MovePoint(ref ends[a], -endDist - length);
                    }

                    //move the pysical point to the new track point
                    ends[a].pos = ends[a].curNode.FindPoint(ends[a].dist);

                    //prevents updating for this movement
                    prevends[a] = ends[a].pos;

                    //checks if there is something conencted to this segment
                    if(ConnectConnect[a] != null){
                        //updates the conencted objects ends point to be equal to the changed one
                        ConnectConnect[a].ends[connectIndex[a]] = ends[a];

                        //queues the connected object to update its connections
                        ConnectConnect[a].UpdateConnections();
                    }
                    

                    //marks that this change has been acounted for and updates prevends
                    prevends[i] = ends[i].pos;

                    //if the loop is continueing, changes so its now altering the other point
                    if(loops){
                        i = a;
                    }
                }

            }
        }
    }

    /*
    public void UpdateConnections(){

        //loops for each connected index
        for (int i = 0; i < 2; i++){
            //checks if the point has moved
            if (ends[i].pos != prevends[i])
            {
                ends[i].pos = track.trackPosition(ends[i].dist);

                //finds the index of the other point
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
    */
}
