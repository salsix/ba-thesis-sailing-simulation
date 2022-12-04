using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosRudderTwist = RosMessageTypes.UnitySailor.UnityRudderTwistMsg;
using RosMessageTypes.UnitySailor;

public class RudderController : MonoBehaviour
{
    ROSConnection ros;

    public GameObject rudder;
    public float currentRot = 0;
    private float goalRot = 0;
    private int rotDir;
    private float rotSpeed = 50;

    private float publishMessageFrequency = 0.5f;
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<RosRudderTwist>("rudder_twist", ROSrotateRudder);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("currentRot: " + currentRot);
        Debug.Log("goalRot: " + goalRot);

        // Handle Rotation by arrow keys
        if (Input.GetKey("left")) ManualRotateRudder(1, -1);
        if (Input.GetKey("right")) ManualRotateRudder(1, 1);

        // Rudder cannot rotate past 90 degrees in either direction
        if (currentRot < -45) currentRot = -45;
        if (currentRot > 45) currentRot = 45;
        if (goalRot < -45) goalRot = -45;
        if (goalRot > 45) goalRot = 45;

        // Rotation Loop
        if (rotDir != 0) {
            if (rotDir == 1 && currentRot < goalRot) {
                currentRot += rotSpeed * Time.deltaTime;
            } else if (rotDir == -1 && currentRot > goalRot) {
                currentRot -= rotSpeed * Time.deltaTime;
            } else {
                rotDir = 0;
            }
        }
    }

    void ManualRotateRudder(float deltaAngle, int newRotDir) {
        rotDir = newRotDir;
        goalRot = currentRot + rotDir;
    }

    void ROSrotateRudder(RosRudderTwist twistMessage) 
    {
        goalRot = twistMessage.twist;
        rotDir = goalRot < currentRot ? -1 : 1;
    }
}
