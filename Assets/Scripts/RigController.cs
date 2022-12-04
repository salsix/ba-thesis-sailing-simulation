using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosSailTwist = RosMessageTypes.UnitySailor.UnitySailTwistMsg;
using RosMessageTypes.UnitySailor;



public class RigController : MonoBehaviour
{
    ROSConnection ros;
    
    public GameObject mast;
    public GameObject rig;
    public float currentRot = 0;
    private float goalRot = 0;
    private int rotDir;
    private float rotSpeed = 50;
    private bool isRotating = false;


    // stuff for publishing position
    public string topicName = "pos_rot";
    public GameObject hull;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;


    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<RosSailTwist>("twist", TwistChange);
    }

    void RotateRig(bool clockwise) {
        rotDir = clockwise ? 1 : -1;
        isRotating = true;
        goalRot += rotDir;
    }

    void TwistChange(RosSailTwist twistMessage) 
    {
        if (twistMessage.twist >= -90 && twistMessage.twist <= 90 && twistMessage.twist != goalRot) {
            goalRot = twistMessage.twist;
            rotDir = goalRot < currentRot ? -1 : 1;
            isRotating = true;
        }
    }

    private void Update() {
        // on A press, rotate rig counter clockwise
        if (Input.GetKey("a")) RotateRig(false);

        // on D press, rotate rig clockwise
        if (Input.GetKey("d")) RotateRig(true);
    
        if (isRotating) {
            if (rotDir == 1 && currentRot < goalRot) {
                rig.transform.RotateAround(mast.transform.position, mast.transform.up, rotSpeed * Time.deltaTime);
                currentRot = currentRot + rotSpeed * Time.deltaTime;
            } else if (rotDir == -1 && currentRot > goalRot) {
                rig.transform.RotateAround(mast.transform.position, mast.transform.up, -rotSpeed * Time.deltaTime);
                currentRot = currentRot - rotSpeed * Time.deltaTime;
            } else {
                isRotating = false;
            }
        }
    }
}