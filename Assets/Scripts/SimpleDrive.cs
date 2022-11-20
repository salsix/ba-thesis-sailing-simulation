using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDrive : MonoBehaviour
{
    public GameObject rudder;
    private Rigidbody rudderRB;
    private Rigidbody boatRB;
    private bool driving;

    // Start is called before the first frame update
    void Start()
    {
        boatRB = this.GetComponent<Rigidbody>();
        rudderRB = rudder.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) driving = !driving;

        if (Input.GetKey("left")) Turn(true);
        if (Input.GetKey("right")) Turn(false);
    }

    void FixedUpdate()
    {
        if (driving)
        {
            boatRB.AddForce(-transform.right * 20000f);
            Debug.Log(this.GetComponent<Renderer>().bounds.size.x);
        }
    }

    void Turn(bool turnLeft) {
        float turnSpeed = turnLeft ? -20000f : 20000f;
        rudderRB.AddForce(transform.up * turnSpeed);
        Debug.Log("turn");
    }

}
 