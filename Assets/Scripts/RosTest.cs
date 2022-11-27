using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosSailTwist = RosMessageTypes.UnitySailor.UnitySailTwistMsg;

public class RosTest : MonoBehaviour
{

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<RosSailTwist>("twist", TwistChange);
    }

    void TwistChange(RosSailTwist twistMessage) 
    {
        // Debug.Log(twistMessage);
        cube.transform.rotation = Quaternion.Euler(new Vector3(0,45,0));
    }
}
