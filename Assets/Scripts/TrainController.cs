using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float SPEED = 100;

    public float position = 30;

    //public ImprovedTrack track;

    private Connection c;

    public bool Auto = false;

    // Start is called before the first frame update
    void Start()
    {

        //track = GameObject.Find("TrackSegment").GetComponent<ImprovedTrack>();
        c = GetComponent<Connection>();

        position = c.startDist[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ensures that the current node is always valid
        if(c.ends[0].curNode == null){
            c.ends[0].curNode = c.track.Head;
        }

        Debug.Log("Moved Train");

        float posChange = 0;

        if(Auto){
            posChange = SPEED * Time.fixedDeltaTime * -1;
        }else{
            posChange = Input.GetAxis("Vertical") * SPEED * Time.fixedDeltaTime * -1;
        }

        //Debug.Log("Point Before movement: " + c.ends[0].pos);

    /*
        Debug.Log("Predicted point before movemnt: " + c.ends[0].curNode.FindPoint(c.ends[0].dist));*/

        //Debug.Log("PointDist before movement: " + c.ends[0].dist);

        //Debug.Log("DistChange: " + posChange);

        //moves the connected point
        TrackManager.MovePoint(ref c.ends[0], posChange);


        //Debug.Log("point after movement: " + c.ends[0].pos);

        //Debug.Log("Predicted point after movemnt: " + c.ends[0].curNode.FindPoint(c.ends[0].dist));

        //Debug.Log("Pointdist after movement: " + c.ends[0].dist);

        //calls c to update its connections
        c.UpdateConnections();

        //Debug.Log("pointdist after connections update: " + c.ends[0].dist);


        //checks if there is anything attached to the connection end being moved
        if(c.ConnectConnect[0] != null){
            //ensures that the current node is always valid
            if(c.ConnectConnect[0].ends[1].curNode == null){
                c.ConnectConnect[0].ends[1].curNode = c.ConnectConnect[1].track.Head;
            }

            //moves the item attached to the specifed connection end
            TrackManager.MovePoint(ref c.ConnectConnect[0].ends[1], posChange);

            //updates the conected objects connections
            c.ConnectConnect[0].UpdateConnections();

        }

        //Debug.Log("pointdist after second conenctions update: "  + c.ends[0].dist);

        //Debug.Log("point after connection Updated: " +  c.ends[0].pos);
    }
}

