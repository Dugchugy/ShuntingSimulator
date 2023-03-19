using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    //a pointer to the first segment in the track
    public TrackNode Head;

    public string trackName = "MenuTrack1";

    //defines the materials to be used to build the track
    [SerializeField] private Material TIEMAT;
    [SerializeField] private Material RAILMAT;

    // Start is called before the first frame update
    void Start()
    {
        //moves this object to (0,0,0) modeling is always right
        transform.position = new Vector3(0,0,0);

        Head = TrackLoader.LoadTrack("Tracks/" + trackName);
    }

    void FixedUpdate(){
        //Debug.Log("point for 1.8: " + Head.FindPoint(1.8f));
        //Debug.Log("point for 2.0: " + Head.FindPoint(2.0f));
        //Debug.Log("t for 1.8: " + Head.findT(1.8f));
        //Debug.Log("t for 2.0: " + Head.findT(2.0f));
        //Debug.Log("head length: " + Head.length);
    }

    //moves a track point a specifed distance
    public static bool MovePoint(ref TrackPoint p, float dist){
        //adds the dist to move to the points dist
        p.dist += dist;

        //checks if the point has moved off the top of its current node
        if(p.dist > p.curNode.length){

            //checks if this node has another node after it
            if(p.curNode.Next != null){
                //moves p to the next node
                p.dist -= p.curNode.length;
                p.curNode = p.curNode.Next;
            }else{
                //moves p to the end of the node and signals that it was not moved fully
                p.dist = p.curNode.length;
                return true;
            }

        }

        //checks if the point has moved off the bottom of its current node
        if(p.dist < 0){

            //checks if this node has another node before it
            if(p.curNode.Prev != null){
                //moves to the previous node
                p.curNode = p.curNode.Prev;
                p.dist += p.curNode.length;
            }else{
                //moves p to the end of the node and signals that it was node moved fully
                p.dist = 0;
                return true;
            }
        }

        //updates the points position
        p.pos = p.curNode.FindPoint(p.dist);

        //Debug.Log("outputPos: " + p.pos);

        return false;

    }

    //finds the distance between two TrackPoints
    public static float findDist(TrackPoint a, TrackPoint b){
        //checks if both nodes are on the same segment of track
        if(a.curNode == b.curNode){
            //returns the differnce in distance
            return (b.dist - a.dist);
        }else{

            //calculates the starting distances to use for a and b
            float adist = - a.dist;
            float bdist = - b.dist;

            //finds the starting nodes to use for a and b
            TrackNode aNode = a.curNode;
            TrackNode bNode = b.curNode;

            //creates a bool to control how long to loop for
            bool loop = true;

            //loops until the shortest distance is found
            while(loop){

                //moves a to the next node
                adist += aNode.length;
                aNode = aNode.Next;

                //checks if the program has moves from node a to node b
                if(aNode == b.curNode){
                    //adds the distance into node b that point b is
                    adist += b.dist;

                    //stops further looping
                    loop = false;

                    //returns adist
                    return adist; 
                }

                //moves b to the next node
                bdist += bNode.length;
                bNode = bNode.Next;

                //checks if node b has looped around to node a
                if(bNode == a.curNode){
                    //adds the distance into node a that point a is
                    bdist += a.dist;

                    //stops further looping
                    loop = false;

                    //returns -bdist (the distance from a to b)
                    return -bdist;
                }
            }

            return 0;

        }
    }
}
