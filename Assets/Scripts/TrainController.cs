using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float SPEED = 100;

    public float position = 30;

    public ImprovedTrack track;

    private Connection c;

    // Start is called before the first frame update
    void Start()
    {

        track = GameObject.Find("TrackSegment").GetComponent<ImprovedTrack>();
        c = GetComponent<Connection>();

        position = c.startDist[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position += Input.GetAxis("Vertical") * SPEED * Time.fixedDeltaTime * -1;

        position = position % track.Length;

        if(position < 0){
            position += track.Length;

            Debug.Log(track.Length);
        }
        if(position > track.Length){
            position -= track.Length;
        }

        //moves the connected point
        c.ends[0].dist = position;

        //updates the connections connected to the moved point
        c.ConnectConnect[0].ends[c.connectIndex[0]].dist = position;

        //updates thier connections
        c.ConnectConnect[0].UpdateConnections();
    }
}
