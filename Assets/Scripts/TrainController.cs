using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float SPEED = 100;

    public float position = 0;

    public ImprovedTrack track;

    // Start is called before the first frame update
    void Start()
    {
        track = GameObject.Find("TrackSegment").GetComponent<ImprovedTrack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position += Input.GetAxis("Vertical") * SPEED * Time.fixedDeltaTime;

        position = position % track.Length;

        if(position < 0){
            position += track.Length;
        }

        transform.position = track.trackPosition(position);
    }
}
