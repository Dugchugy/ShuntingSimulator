using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogeeController : MonoBehaviour
{
    public Mesh SingleBogee;
    public Mesh DoubleBogee;
    public Mesh LeadMesh;

    public bool lead = false;

    private MeshFilter mf;
    private Connection c;

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Connection>();
        mf = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!lead){
            if(c.connected[1] != null){
                mf.mesh = DoubleBogee;
            }else{
                mf.mesh = SingleBogee;
            }
        }else{
            mf.mesh = LeadMesh;
        }
    }
}
