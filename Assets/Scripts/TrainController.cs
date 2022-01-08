using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float SPEED = 100;

    public float position = 0;

    public ImprovedTrack track;

    private Connection c;

    // Start is called before the first frame update
    void Start()
    {
        track = GameObject.Find("TrackSegment").GetComponent<ImprovedTrack>();
        c = GetComponent<Connection>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position += Input.GetAxis("Vertical") * SPEED * Time.fixedDeltaTime;

        position = position % track.Length;

        if(position < 0){
            position += track.Length;
        }

        c.ends[0].dist = position;
    }
}
