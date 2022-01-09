using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowConenctionPoint : MonoBehaviour
{
    public GameObject Car;
    public int Index;

    private Connection c;

    // Start is called before the first frame update
    void Start()
    {
        c = Car.GetComponent<Connection>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = c.ends[Index].pos;
    }
}
